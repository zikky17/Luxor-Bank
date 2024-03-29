using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ServiceLibrary.Interfaces;

namespace ServiceLibrary.Services
{
    public class AccountService : IAccountService
    {

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;
        public List<AccountViewModel> Accounts { get; set; }

        public List<AccountViewModel> GetAccountInfo(int accountId)
        {
            Accounts = _context.Dispositions
               .Include(d => d.Account)
               .Where(d => d.AccountId == accountId)
               .Select(d => new AccountViewModel
               {
                   AccountId = d.AccountId,
                   Created = d.Account.Created,
                   Balance = d.Account.Balance,
                   Transactions = d.Account.Transactions.ToList()
               })
               .ToList();
            return Accounts;
        }
    }
}
