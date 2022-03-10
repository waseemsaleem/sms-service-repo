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
using ApiService.ViewModels;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            var cacheKey = "InboundSmsKey";
            GenericResponse response = null;

            try
            {
                if (smsRequest != null)
                {
                    int.TryParse(_configuration.GetSection("Redis").GetSection("AbsoluteExpirationForInbound").Value, out int absoluteExpiryTime);
                    int.TryParse(_configuration.GetSection("Redis").GetSection("SlidingExpirationForInbound").Value, out int slidingExpiryTime);
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
                    else
                    {
                        messageList = _phoneService.GetAll().Select(p => new PhoneNumber()
                        {
                            To = p.To,
                            From = p.From
                        }).ToList();
                        serializedMessagesList = JsonConvert.SerializeObject(messageList);
                        redisMessagesList = Encoding.UTF8.GetBytes(serializedMessagesList);
                        await _distributedCache.SetAsync(cacheKey, redisMessagesList, options);
                    }

                    response = _phoneService.InOutboundSms(smsRequest);

                    messageList.Add(new PhoneNumber()
                    {
                        From = smsRequest.From,
                        To = smsRequest.To
                    });
                    serializedMessagesList = JsonConvert.SerializeObject(messageList);
                    redisMessagesList = Encoding.UTF8.GetBytes(serializedMessagesList);
                    await _distributedCache.SetAsync(cacheKey, redisMessagesList, options);
                }
                return Ok(new
                {
                    message = response.Message,
                    error = response.Error,
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
