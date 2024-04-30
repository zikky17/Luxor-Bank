
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;

namespace MoneyLaunderingGuard
{
    public class App
    {
        private readonly ApplicationDbContext _context = DatabaseService.GetDbContext();
        public DateOnly LastReportRunTime { get; set; }

        public void Run()
        {


            var directoryPath = "../../../SuspiciousTransactions";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            ProcessCountry("Sweden", directoryPath);
            ProcessCountry("Norway", directoryPath);
            ProcessCountry("Finland", directoryPath);
            ProcessCountry("Denmark", directoryPath);

            Console.WriteLine("All 4 reports are done!");
            Console.WriteLine($"Suspicious transactions saved to folder: 'SuspiciousTransactions'");

            LastReportRunTime = DateOnly.FromDateTime(DateTime.Today);
        }

        private void ProcessCountry(string country, string directoryPath)
        {
            var customers = GetCustomersByCountry(country);
            var transactions = GetTransactionsForCustomers(customers);
            var suspiciousTransactions = GetSuspiciousTransactions(transactions);

            Console.WriteLine($"Getting report from {country}....");
            Console.WriteLine($"Amount of new suspicious transactions are: {suspiciousTransactions.Count}");
            WriteTransactionsToFile(suspiciousTransactions, directoryPath, $"{country}.txt");
        }

        private List<Customer> GetCustomersByCountry(string country)
        {
            return _context.Customers.Where(c => c.Country == country).ToList();
        }

        private List<Transaction> GetTransactionsForCustomers(List<Customer> customers)
        {
            var customerIds = customers.Select(c => c.CustomerId);
            return _context.Dispositions
                .Where(d => customerIds.Contains(d.CustomerId))
                .SelectMany(d => d.Account.Transactions)
                .ToList();
        }

        private List<Transaction> GetSuspiciousTransactions(List<Transaction> transactions)
        {
            var maxTotalTransactions = 23000;
            var maxSingleTransaction = 15000;
            var lastThreeDays = DateOnly.FromDateTime(DateTime.Today.AddDays(-3));
            var lastReportRunTime = LastReportRunTime;

            var startDate = lastReportRunTime == default(DateOnly) ? lastThreeDays : lastReportRunTime;

            var suspiciousTransactions = transactions
                .Where(t => t.Date >= startDate && t.Date <= DateOnly.FromDateTime(DateTime.Today))
                .GroupBy(t => t.AccountId)
                .Where(group => group.Sum(t => t.Amount) > maxTotalTransactions || group.Any(t => t.Amount > maxSingleTransaction))
                .SelectMany(group => group)
                .ToList();

            var highAmountTransactions = transactions
                .Where(t => t.Amount > maxSingleTransaction && !suspiciousTransactions.Contains(t))
                .ToList();

            suspiciousTransactions.AddRange(highAmountTransactions);

            return suspiciousTransactions;
        }


        private void WriteTransactionsToFile(List<Transaction> transactions, string directoryPath, string fileName)
        {
            var filePath = Path.Combine(directoryPath, fileName);
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var transaction in transactions)
                {
                    var customer = GetCustomerByAccountId(transaction.AccountId);
                    writer.WriteLine($"Customer Name: {customer.Givenname} {customer.Surname}");
                    writer.WriteLine($"Transaction ID: {transaction.TransactionId}");
                    writer.WriteLine($"Amount: {transaction.Amount}");
                    writer.WriteLine($"Date: {transaction.Date}");
                    writer.WriteLine($"Account Number: {transaction.AccountId}");
                    writer.WriteLine();
                }
            }
        }

        private Customer GetCustomerByAccountId(int accountId)
        {
            return _context.Dispositions
                .Include(d => d.Customer)
                .Where(d => d.AccountId == accountId)
                .Select(d => d.Customer)
                .First();
        }
    }

}