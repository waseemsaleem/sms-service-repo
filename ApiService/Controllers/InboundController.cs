using Microsoft.AspNetCore.Mvc;
using System;
using ApiService.Models;
using ApiService.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboundController : ControllerBase
    {
        private readonly accountdbContext _context;
        private readonly IPhoneService _phoneService;
        private readonly ILogger<InboundController> _log;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        public InboundController(
            IPhoneService phoneService)
            
         
        {
            _phoneService = phoneService;

    
        }

        [HttpPost]
        [Route("InboundSMS")]
        public IActionResult InboundSms([FromBody] PhoneNumber smsRequest)
        {
            var cacheKey = "inboundSms";
            try
            {
                if (smsRequest != null)
                {
                    //string absoluteExpiryTime = _configuration.GetSection("Redis").GetSection("AbsoluteExpiration").Value;
                    //string slidingExpiryTime = _configuration.GetSection("Redis").GetSection("SlidingExpiration").Value;

                    //var serializedMessageObject = JsonConvert.SerializeObject(smsRequest);
                    //var redisMessageObject = Encoding.UTF8.GetBytes(serializedMessageObject);
                    //var options = new DistributedCacheEntryOptions()
                    //    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    //    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    //_distributedCache.SetAsync(cacheKey, redisMessageObject, options);
                    _phoneService.Save(smsRequest);
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

        //[HttpGet]
        //[Route("GetInboundSMS")]
        //public async Task<ActionResult<IEnumerable<PhoneNumber>>> GetInboundSMS()
        //{
        //    var cacheKey = "inboundSms";

        //    string serializedMessagesList;
        //    var messageList = new List<PhoneNumber>();
        //    var redisMessagesList = await _distributedCache.GetAsync(cacheKey);
        //    if (redisMessagesList != null)
        //    {
        //        serializedMessagesList = Encoding.UTF8.GetString(redisMessagesList);
        //        messageList = JsonConvert.DeserializeObject<List<PhoneNumber>>(serializedMessagesList);
        //    }
        //    else
        //    {
        //        messageList = await _context.PhoneNumbers.ToListAsync();
        //        serializedMessagesList = JsonConvert.SerializeObject(messageList);
        //        redisMessagesList = Encoding.UTF8.GetBytes(serializedMessagesList);
        //        var options = new DistributedCacheEntryOptions()
        //            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
        //            .SetSlidingExpiration(TimeSpan.FromMinutes(2));
        //        await _distributedCache.SetAsync(cacheKey, redisMessagesList, options);
        //    }
        //    return messageList;

        //}
    }
}
