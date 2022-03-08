using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using ApiService.Models;
using ApiService.Services.Interfaces;
using ApiService.ViewModels;
using Microsoft.Extensions.Logging;

namespace ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly accountdbContext _context;
        readonly ILogger<AccountController> _log;
        private readonly IAccountService _accountService;
        public AccountController(accountdbContext context, ILogger<AccountController> log, IAccountService accountService)
        {
            _context = context;
            _log = log;
            _accountService = accountService;
        }
        [HttpPost]
        [Route("UserLogin")]
        public IActionResult UserLogin([FromBody] AccountViewModel accountModel)
        {
            Account user = null;
            if (!string.IsNullOrEmpty(accountModel.AuthId) && !string.IsNullOrEmpty(accountModel.Username))
            {
                user = _context.Accounts.FirstOrDefault(a => a.Username == accountModel.Username && a.AuthId == accountModel.AuthId);
                if (user == null)
                {
                    return BadRequest(new { message = "Username or Password is incorrect!" });
                }
                return Ok(user);
            }
            return BadRequest(new
            {
                error = "Username or Password is empty"
            });
        }
        [HttpPost]
        [Route("AddAccount")]
        public IActionResult AddAccount([FromBody] AccountViewModel accountVm)
        {
            try
            {
                Account account = new Account();

                if (accountVm != null)
                {
                    account.AuthId = accountVm.AuthId;
                    account.Username = accountVm.Username;
                    _accountService.Save(account);
                }
                return Ok(account);
            }
            catch (Exception ex)
            {

                _log.LogError(ex, ex.Message);
                return new JsonResult(ex);
            }
        }

        [HttpGet]
        [Route("GetAllAccounts")]
        public IActionResult GetAllAccounts()
        {
            try
            {
                var result = _accountService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                return new JsonResult(ex);
            }
        }
    }
}
