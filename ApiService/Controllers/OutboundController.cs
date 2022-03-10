using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiService.Models;
using ApiService.Services.Interfaces;
using ApiService.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            var cacheKey = "OutboundSmsKey";
            GenericResponse response=null;
            try
            {
                if (outBoundSmsRequest != null)
                {
                    int.TryParse(_configuration.GetSection("Redis").GetSection("AbsoluteExpirationForOutbound").Value, out int absoluteExpiryTime);
                    int.TryParse(_configuration.GetSection("Redis").GetSection("SlidingExpirationForOutbound").Value, out int slidingExpiryTime);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddHours(absoluteExpiryTime))
                        .SetSlidingExpiration(TimeSpan.FromHours(slidingExpiryTime));

                    string serializedMessagesList;
                    var messageList = new List<PhoneNumber>();
                    var redisMessagesList = await _distributedCache.GetAsync(cacheKey);
                    if (redisMessagesList != null)
                    {
                        serializedMessagesList = Encoding.UTF8.GetString(redisMessagesList);
                        messageList = JsonConvert.DeserializeObject<List<PhoneNumber>>(serializedMessagesList);
                    }
                    var fromNumber = outBoundSmsRequest.From;
                    var toNumber = outBoundSmsRequest.To;
                    var message = messageList.Where(p => p.From == outBoundSmsRequest.From && p.To == outBoundSmsRequest.To).ToList();
                    var messageStop = messageList.FirstOrDefault(p => p.From == outBoundSmsRequest.From && p.To == outBoundSmsRequest.To && p.Text.Contains("Stop"));
                    if (messageStop != null)
                    {
                        return BadRequest(new
                        {
                            error = $"Sms From {fromNumber} to {toNumber} blocked by Stop Request"
                        });
                    }
                    if (message.Count != 0 && message.Count >= 50)
                    {
                        return BadRequest(new
                        {
                            error = $"Limit reached For from {fromNumber}"
                        });
                    }


                    response= _phoneService.InOutboundSms(outBoundSmsRequest);
                    
                    messageList.Add(outBoundSmsRequest);
                    serializedMessagesList = JsonConvert.SerializeObject(messageList);
                    redisMessagesList = Encoding.UTF8.GetBytes(serializedMessagesList);
                    await _distributedCache.SetAsync(cacheKey, redisMessagesList, options);
                }
                return Ok(new
                {
                    message = response.Message,
                    error=response.Error,
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
