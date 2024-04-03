using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;
using Transaction = ServiceLibrary.Models.Transaction;

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

        public bool Deposit(Transaction transaction, int accountId)
        {
            try
            {
                var account = _context.Accounts.First(a => a.AccountId == accountId);

                account.Balance += transaction.Amount;

                _context.Transactions.Add(transaction);

                _context.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }

        public bool Withdraw(Transaction transaction, int accountId)
        {
            try
            {
                var account = _context.Accounts.First(a => a.AccountId == accountId);

                account.Balance -= transaction.Amount;

                _context.Transactions.Add(transaction);

                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }


}