using BankApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using Transaction = ServiceLibrary.Data.Transaction;

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

        public void CreateAccount(int customerId, Account newAccount)
        {

            _context.Accounts.Add(newAccount);
            _context.SaveChanges();
            CreateDisposition(customerId, newAccount.AccountId);
        }

        public void CreateDisposition(int customerId, int accountId)
        {
            var newDisposition = new Disposition
            {
                CustomerId = customerId,
                AccountId = accountId,
                Type = "Owner"
            };

            _context.Dispositions.Add(newDisposition);
            _context.SaveChanges();
        }

        public void DeleteAccount(int accountId)
        {
            var account = _context.Accounts.Where(a => a.AccountId == accountId).First();
            var disposition = _context.Dispositions.Where(d => d.AccountId == accountId).First();

            foreach (var transaction in _context.Transactions.Where(t => t.AccountId == accountId))
            {
                _context.Transactions.Remove(transaction);
            }

            _context.Dispositions.Remove(disposition);
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }
    }
}

