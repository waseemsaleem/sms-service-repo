using Microsoft.AspNetCore.Mvc;
using System;
using ApiService.Models;
using ApiService.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutboundController : ControllerBase
    {
        private readonly accountdbContext _context;
        private readonly IPhoneService _phoneService;
        private readonly ILogger<InboundController> _log;
        private readonly IDistributedCache _distributedCache;
        public OutboundController(accountdbContext context, 
            IPhoneService phoneService,
            IDistributedCache distributedCache,
            ILogger<InboundController> log)
        {
            _context = context;
            _phoneService = phoneService;
            _log = log;
            _distributedCache = distributedCache;
        }

        [HttpPost]
        [Route("AddOutboundSms")]
        public IActionResult AddOutboundSms([FromBody] PhoneNumber phoneNumberObject)
        {
            PhoneNumber phoneEntity = new PhoneNumber();
            try
            {
                if (phoneNumberObject != null)
                {
                    if (string.IsNullOrEmpty(phoneNumberObject.From))
                    {
                        return BadRequest(new { error = "From Number is  Missing or Empty!" });
                    }

                    if (phoneNumberObject.From.Length < 6)
                    {
                        return BadRequest(new { error = "From Number should be minimum 6 characters!" });
                    }

                    if (phoneNumberObject.From.Length > 16)
                    {
                        return BadRequest(new { error = "From Number should Not be more than   16 characters!" });
                    }

                    if (string.IsNullOrEmpty(phoneNumberObject.To))
                    {
                        return BadRequest(new { error = "To Number is Empty or Missing!" });

                    }

                    if (phoneNumberObject.To.Length < 6)
                    {
                        return BadRequest(new { error = "To Number should be minimum 6 characters!" });
                    }

                    if (phoneNumberObject.To.Length > 16)
                    {
                        return BadRequest(new { error = "To Number should Not be more than  minimum 16 characters!" });
                    }

                    //phoneEntity.To = phoneNumberObject.To;
                    //phoneEntity.From = phoneNumberObject.From;
                    //phoneEntity.AccountId = phoneNumberObject.AccountId;
                    //phoneEntity.Number = phoneNumberObject.Number;
                    //phoneEntity.Text = phoneNumberObject.Text;
                    _phoneService.Add(phoneNumberObject);

                }
                return Ok(new
                {
                    message = "Outbound SMS OK",
                    phoneNumberObject = phoneNumberObject
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
