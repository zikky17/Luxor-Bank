using Azure;
using BankApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using ServiceLibrary.ViewModels;
using Transaction = ServiceLibrary.Data.Transaction;

namespace ServiceLibrary.Services
{
    public class AccountService(ApplicationDbContext context) : IAccountService
    {
        private readonly ApplicationDbContext _context = context;

        public AccountViewModel GetAccountInfo(int accountId)
        {
            var query = _context.Dispositions
                .Include(d => d.Account)
                .Where(d => d.AccountId == accountId)
                .Select(d => new AccountViewModel
                {
                    AccountId = d.AccountId,
                    Created = d.Account.Created,
                    Balance = d.Account.Balance,
                    Transactions = d.Account.Transactions
                }).ToList()
                .First();

            return query;
        }

        public List<TransactionViewModel> GetTransactions(int accountId)
        {
            var transactions = _context.Dispositions
                .Include(d => d.Account)
                .ThenInclude(a => a.Transactions)
                .Where(d => d.AccountId == accountId)
                .SelectMany(d => d.Account.Transactions)
                .Select(t => new TransactionViewModel
                {
                    AccountId = t.AccountId,
                    Date = t.Date,
                    Operation = t.Operation,
                    Type = t.Type,
                    Amount = t.Amount
                })
                .ToList();

            return transactions;
        }

        public StatusMessage Deposit(decimal amount, int accountId, string comment)
        {

            var account = _context.Accounts.Find(accountId);

            if(account == null)
            {
                return StatusMessage.CantFindAccount;
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

            if (transaction.Amount > 0)
            {
                transaction.Amount = -transaction.Amount;
            }

            account.Balance += transaction.Amount;

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

        public void DeleteAllAccounts(List<int> accounts)
        {
            foreach (var account in accounts)
            {
                var accountToDelete = _context.Accounts.First(a => a.AccountId == account);

                if (accountToDelete != null)
                {
                    if (accountToDelete.Balance > 1)
                    {
                        throw new InvalidOperationException($"Cannot delete account {accountToDelete.AccountId}. Make sure the balance is set to 0 first.");
                    }

                    if (_context.PermenentOrders.Where(p => p.AccountId == account).Count() > 0)
                    {
                        var permanentOrders = _context.PermenentOrders.Where(p => p.AccountId == account).ToList();
                        foreach (var order in permanentOrders)
                        {
                            _context.PermenentOrders.Remove(order);
                        }
                    }

                    if (_context.Loans.Where(p => p.AccountId == account).Count() > 0)
                    {
                        var loans = _context.Loans.Where(p => p.AccountId == account).ToList();
                        foreach (var loan in loans)
                        {
                            _context.Loans.Remove(loan);
                        }
                    }

                    var disposition = _context.Dispositions.First(d => d.AccountId == account);
                    if (disposition != null)
                    {
                        foreach (var transaction in _context.Transactions.Where(t => t.AccountId == account))
                        {
                            _context.Transactions.Remove(transaction);
                        }

                        _context.Dispositions.Remove(disposition);
                        _context.Accounts.Remove(accountToDelete);
                        _context.SaveChanges();
                    }
                    else
                    {
                        throw new InvalidOperationException($"Disposition not found for account {accountToDelete.AccountId}.");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Account {account} not found.");
                }
            }
        }



    }
}


