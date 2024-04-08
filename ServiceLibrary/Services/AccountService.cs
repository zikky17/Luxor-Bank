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

        public StatusMessage Deposit(decimal amount, int accountId, string comment)
        {

            var account = _context.Accounts.First(a => a.AccountId == accountId);

            if (amount < 100 || amount > 10000)
            {
                return StatusMessage.IncorrectAmount;
            }

            if (comment == null)
            {
                return StatusMessage.MessageRequired;
            }

            account.Balance += amount;

            var transaction = new Transaction
            { 
                AccountId = accountId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = "Credit",
                Operation = comment,
                Amount = amount
            };

            _context.Transactions.Add(transaction);

            _context.SaveChanges();

            return StatusMessage.Approved;

        }

        public StatusMessage Withdraw(Transaction transaction)
        {
            var account = _context.Accounts.First(a => a.AccountId == transaction.AccountId);

            if (account.Balance < transaction.Amount)
            {
                return StatusMessage.TooLowBalance;
            }

            if (transaction.Amount < 100 || transaction.Amount > 10000)
            {
                return StatusMessage.IncorrectAmount;
            }

            account.Balance -= transaction.Amount;

            _context.Transactions.Add(transaction);

            _context.SaveChanges();

            return StatusMessage.Approved;
        }
    }
}

