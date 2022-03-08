using System.Collections.Generic;
using System.Linq;
using ApiService.Models;
using ApiService.Services.Interfaces;

namespace ApiService.Services
{
    public class AccountService : IAccountService
    {
        private readonly accountdbContext _context;
        public AccountService(accountdbContext context)
        {
            _context = context;
        }

        public IEnumerable<Account> GetAll()
        {
            var account = _context.Accounts.ToList();
            return account;
        }

        public Account Login(Account account)
        {
            _context.Add(account);
            _context.SaveChanges();
            return account;
        }

        public Account Save(Account account)
        {
            _context.Add(account);
            _context.SaveChanges();
            return account;
        }
    }
}
