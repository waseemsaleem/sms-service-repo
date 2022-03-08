using System.Collections.Generic;
using ApiService.Models;

namespace ApiService.Services.Interfaces
{
    public interface IAccountService
    {
        IEnumerable<Account> GetAll();
        Account Login(Account account);
        Account Save(Account account);
    }
}
