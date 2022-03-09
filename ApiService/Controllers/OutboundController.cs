using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiService.Models;
using ApiService.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutboundController : ControllerBase
    {
        private readonly IPhoneService _phoneService;
        private readonly ILogger<OutboundController> _log;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        public OutboundController(IPhoneService phoneService, IConfiguration configuration, IDistributedCache distributedCache, ILogger<OutboundController> log)
        {
           
            _phoneService = phoneService;
            _configuration = configuration;
            _distributedCache = distributedCache;
            _log = log;
        }

        [HttpPost]
        [Route("sms")]
        public async Task<IActionResult> OutboundSms([FromBody] PhoneNumber outBoundSmsRequest)
        {
            var cacheKey = "outboundSms";
            try
            {
                if (outBoundSmsRequest != null)
                {
                    int.TryParse(_configuration.GetSection("Redis").GetSection("AbsoluteExpiration").Value, out int absoluteExpiryTime);
                    int.TryParse(_configuration.GetSection("Redis").GetSection("SlidingExpiration").Value, out int slidingExpiryTime);
                    string outBoundSerializedMessagesList;
                    var outBoundMessageList = new List<PhoneNumber>();
                    var redisOutboundMessagesList = await _distributedCache.GetAsync(cacheKey);
                    if (redisOutboundMessagesList != null)
                    {
                        outBoundSerializedMessagesList = Encoding.UTF8.GetString(redisOutboundMessagesList);
                        outBoundMessageList = JsonConvert.DeserializeObject<List<PhoneNumber>>(outBoundSerializedMessagesList);
                    }
                    else
                    {
                        outBoundMessageList = _phoneService.GetAll();
                        outBoundSerializedMessagesList = JsonConvert.SerializeObject(outBoundMessageList);
                        redisOutboundMessagesList = Encoding.UTF8.GetBytes(outBoundSerializedMessagesList);
                        var options1 = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(absoluteExpiryTime))
                            .SetSlidingExpiration(TimeSpan.FromHours(slidingExpiryTime));
                        await _distributedCache.SetAsync(cacheKey, redisOutboundMessagesList, options1);
                    }
                    var fromNumber = outBoundSmsRequest.From;
                    var toNumber = outBoundSmsRequest.To;
                    //var message = messageList.Where(p => p.From == smsRequest.From && p.To == smsRequest.To).ToList();

                    foreach (var message in outBoundMessageList)
                    {
                        if (!string.IsNullOrEmpty(message.From) && !string.IsNullOrEmpty(message.To))
                        {
                            if (outBoundSmsRequest.To == message.To && outBoundSmsRequest.From == message.From)
                            {
                                return BadRequest(new
                                {
                                    error = $"Sms From {fromNumber} to {toNumber} blocked by Stop request"
                                });
                            }

                        }

                    }
                    _phoneService.InOutboundSms(outBoundSmsRequest);
                    var outBoundMessageData = _phoneService.GetAll();
                    outBoundMessageList = outBoundMessageData;
                    outBoundSerializedMessagesList = JsonConvert.SerializeObject(outBoundMessageList);
                    redisOutboundMessagesList = Encoding.UTF8.GetBytes(outBoundSerializedMessagesList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddHours(absoluteExpiryTime))
                        .SetSlidingExpiration(TimeSpan.FromHours(slidingExpiryTime));
                    await _distributedCache.SetAsync(cacheKey, redisOutboundMessagesList, options);
                }
                return Ok(new
                {
                    message = "Outbound SMS OK",
                    outBoundSmsObject = outBoundSmsRequest
                });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                return new JsonResult(ex);
            }

        }
    }
}
