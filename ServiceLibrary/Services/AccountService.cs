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
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<AccountViewModel> GetAccountInfo(int accountId)
        {
            var query = _context.Dispositions
                .Include(d => d.Account)
                .Where(d => d.AccountId == accountId)
                .Select(d => new AccountViewModel
                {
                    AccountId = d.AccountId,
                    Created = d.Account.Created,
                    Balance = d.Account.Balance,
                    Transactions = d.Account.Transactions.ToList()
                });

          

            var sortedAccounts = query.ToList();
            return sortedAccounts;
        }
    }


}