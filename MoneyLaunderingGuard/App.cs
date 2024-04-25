
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;

namespace MoneyLaunderingGuard
{
    public class App
    {
        private readonly ApplicationDbContext _context = DatabaseService.GetDbContext();

        public List<Transaction> SuspiciousSingleTransactions { get; set; }
        public List<Customer> CustomersSweden { get; set; }
        public List<Customer> CustomersNorway { get; set; }
        public List<Customer> CustomersFinland { get; set; }
        public List<Customer> CustomersDenmark { get; set; }

        public void Run()
        {
            var maxSingleTransaction = 15000;
            SuspiciousSingleTransactions = GetSuspiciousSingleTransaction(maxSingleTransaction);

            CustomersSweden = GetCustomersSortedByCountry("Sweden");
            CustomersNorway = GetCustomersSortedByCountry("Norway");
            CustomersFinland = GetCustomersSortedByCountry("Finland");
            CustomersDenmark = GetCustomersSortedByCountry("Denmark");

            var directoryPath = "../../../SuspiciousTransactions";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            WriteTransactionsToFile(SuspiciousSingleTransactions, directoryPath, "SuspiciousTransactions.txt");

            WriteTransactionsToFile(GetSuspiciousTransactionsThreeLastDays(GetTransactionsByCountry(CustomersSweden)), directoryPath, "Sweden.txt");
            WriteTransactionsToFile(GetSuspiciousTransactionsThreeLastDays(GetTransactionsByCountry(CustomersNorway)), directoryPath, "Norway.txt");
            WriteTransactionsToFile(GetSuspiciousTransactionsThreeLastDays(GetTransactionsByCountry(CustomersFinland)), directoryPath, "Finland.txt");
            WriteTransactionsToFile(GetSuspiciousTransactionsThreeLastDays(GetTransactionsByCountry(CustomersDenmark)), directoryPath, "Denmark.txt");

            Console.WriteLine($"Suspicious transactions saved to: {directoryPath}");
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

        public Customer GetCustomerByAccountId(int accountId)
        {
            var customer = _context.Dispositions
            .Include(d => d.Customer)
            .Where(d => d.AccountId == accountId)
            .Select(d => d.Customer)
            .First();

            return customer;
        }
    }
}