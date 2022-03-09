using ApiService.Models;
using ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
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
        public InboundController(IPhoneService phoneService, IConfiguration configuration, IDistributedCache distributedCache, ILogger<InboundController> log)
        {
            _phoneService = phoneService;
            _configuration = configuration;
            _distributedCache = distributedCache;
            _log = log;
        }

        [HttpPost]
        [Route("sms")]
        public IActionResult InboundSms([FromBody] PhoneNumber smsRequest)
        {
            var cacheKey = "inboundSms";
            try
            {
                if (smsRequest != null)
                {
                    int.TryParse(_configuration.GetSection("Redis").GetSection("AbsoluteExpiration").Value, out int absoluteExpiryTime);
                    int.TryParse(_configuration.GetSection("Redis").GetSection("SlidingExpiration").Value, out int slidingExpiryTime);

                    var serializedMessageObject = JsonConvert.SerializeObject(smsRequest);
                    var redisMessageObject = Encoding.UTF8.GetBytes(serializedMessageObject);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddHours(absoluteExpiryTime))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiryTime));
                    _distributedCache.SetAsync(cacheKey, redisMessageObject, options);
                    _phoneService.InOutboundSms(smsRequest);
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
