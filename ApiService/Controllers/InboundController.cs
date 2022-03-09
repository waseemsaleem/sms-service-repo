using ApiService.Models;
using ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InboundController : ControllerBase
    {
        private readonly IPhoneService _phoneService;
        private readonly ILogger<InboundController> _log;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        private readonly accountdbContext _context;
        public InboundController(IPhoneService phoneService, IConfiguration configuration, IDistributedCache distributedCache, ILogger<InboundController> log, accountdbContext context)
        {
            _phoneService = phoneService;
            _configuration = configuration;
            _distributedCache = distributedCache;
            _log = log;
            _context = context;
        }

        [HttpPost]
        [Route("sms")]
        public async Task<IActionResult> InboundSms([FromBody] PhoneNumber smsRequest)
        {
            var cacheKey = "inboundSmsKey";
            try
            {
                if (smsRequest != null)
                {

                    int.TryParse(_configuration.GetSection("Redis").GetSection("AbsoluteExpiration").Value, out int absoluteExpiryTime);
                    int.TryParse(_configuration.GetSection("Redis").GetSection("SlidingExpiration").Value, out int slidingExpiryTime);
                    string serializedMessagesList;
                    var messageList = new List<PhoneNumber>();
                    var redisMessagesList = await _distributedCache.GetAsync(cacheKey);
                    if (redisMessagesList != null)
                    {
                        serializedMessagesList = Encoding.UTF8.GetString(redisMessagesList);
                        messageList = JsonConvert.DeserializeObject<List<PhoneNumber>>(serializedMessagesList);
                    }
                    else
                    {
                        messageList = _phoneService.GetAll();
                        serializedMessagesList = JsonConvert.SerializeObject(messageList);
                        redisMessagesList = Encoding.UTF8.GetBytes(serializedMessagesList);
                        var options1 = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(absoluteExpiryTime))
                            .SetSlidingExpiration(TimeSpan.FromHours(slidingExpiryTime));
                        await _distributedCache.SetAsync(cacheKey, redisMessagesList, options1);
                    }
                    var fromNumber = smsRequest.From;
                    var toNumber = smsRequest.To;
                    //var message = messageList.Where(p => p.From == smsRequest.From && p.To == smsRequest.To).FirstOrDefault();
                    var message = messageList.Where(p => p.From == smsRequest.From && p.To == smsRequest.To).ToList();

                    foreach (var item in message)
                    {
                        if (!string.IsNullOrEmpty(item.From) && !string.IsNullOrEmpty(item.To))
                        {
                            if (smsRequest.To == item.To && smsRequest.From == item.From)// && item.MessageCount >= 50)
                            {
                                return BadRequest(new
                                {
                                    error = $"Sms From {fromNumber} to {toNumber} blocked by Stop Request"
                                });
                            }

                        }

                    }
                    _phoneService.InOutboundSms(smsRequest);
                    var data = _phoneService.GetAll();
                    messageList = data;
                    serializedMessagesList = JsonConvert.SerializeObject(messageList);
                    redisMessagesList = Encoding.UTF8.GetBytes(serializedMessagesList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddHours(absoluteExpiryTime))
                        .SetSlidingExpiration(TimeSpan.FromHours(slidingExpiryTime));
                    await _distributedCache.SetAsync(cacheKey, redisMessagesList, options);
                }
                return Ok(new
                {
                    message = "Inbound SMS OK",
                    phoneNumberObject = smsRequest
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
