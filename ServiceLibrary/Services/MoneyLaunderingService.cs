using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;

namespace ServiceLibrary.Services
{
    public class MoneyLaunderingService : IMoneyLaunderingService
    {
        private readonly ApplicationDbContext _context;

        public MoneyLaunderingService()
        {
            
        }

        public MoneyLaunderingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Customer> GetCustomersSortedByCountry(string country)
        {
            var customers = _context.Customers.Where(c => c.Country == country).ToList();
            return customers;
        }

        public List<Transaction> GetSuspiciousSingleTransaction(int maxAmount)
        {
            var transactions = _context.Transactions.Where(t => t.Amount > maxAmount).ToList();
            return transactions;
        }

        public List<Transaction> GetTransactionsByCountry(List<Customer> customers)
        {
            foreach (var customer in customers)
            {
                var transactions = _context.Dispositions
                 .Where(d => customers.Select(c => c.CustomerId).Contains(d.CustomerId))
                 .SelectMany(d => d.Account.Transactions)
                 .ToList();
                return transactions;

            }
            return null;
        }

        public List<Transaction> GetSuspiciousTransactionsThreeLastDays(List<Transaction> transactions)
        {
            var maxTotalTransactions = 23000;
            var today = DateOnly.FromDateTime(DateTime.Today);
            var threeDaysAgo = today.AddDays(-3);

            var suspiciousTransactions = transactions
            .Where(t => t.Date >= threeDaysAgo)
                .GroupBy(t => t.AccountId)
                .Where(group => group.Sum(t => t.Amount) > maxTotalTransactions)
                .SelectMany(group => group)
                .ToList();

            return suspiciousTransactions;
        }
    }
}
