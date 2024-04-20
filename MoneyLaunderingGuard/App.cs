
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;

namespace MoneyLaunderingGuard
{
    public class App
    {
        private readonly ApplicationDbContext _context = MoneyLaunderingService.GetDbContext();

        public List<Transaction> SuspiciousSingleTransactions { get; set; }

        public List<Account> Accounts { get; set; }
        public List<Customer> CustomersSweden { get; set; }
        public List<Customer> CustomersNorway { get; set; }
        public List<Customer> CustomersFinland { get; set; }
        public List<Customer> CustomersDenmark { get; set; }

        public List<Transaction> TransactionsSweden { get; set; }
        public List<Transaction> TransactionsNorway { get; set; }
        public List<Transaction> TransactionsFinland { get; set; }
        public List<Transaction> TransactionsDenmark { get; set; }

        public List<Transaction> SuspiciousTransactionsSweden { get; set; }
        public List<Transaction> SuspiciousTransactionsNorway { get; set; }
        public List<Transaction> SuspiciousTransactionsFinland { get; set; }
        public List<Transaction> SuspiciousTransactionsDenmark { get; set; }


        public void Run()
        {
            var maxSingleTransaction = 15000;
            SuspiciousSingleTransactions = GetSuspiciousSingleTransaction(maxSingleTransaction);
            
            CustomersSweden = GetCustomersSortedByCountry("Sweden");
            CustomersNorway = GetCustomersSortedByCountry("Norway");
            CustomersFinland = GetCustomersSortedByCountry("Finland");
            CustomersDenmark = GetCustomersSortedByCountry("Denmark");

            TransactionsSweden = GetTransactionsByCountry(CustomersSweden);
            TransactionsNorway = GetTransactionsByCountry(CustomersNorway);
            TransactionsFinland = GetTransactionsByCountry(CustomersFinland);
            TransactionsDenmark = GetTransactionsByCountry(CustomersDenmark);

            SuspiciousTransactionsSweden = GetSuspiciousTransactionsThreeLastDays(TransactionsSweden);
            SuspiciousTransactionsDenmark = GetSuspiciousTransactionsThreeLastDays(TransactionsDenmark);
            SuspiciousTransactionsNorway = GetSuspiciousTransactionsThreeLastDays(TransactionsNorway);
            SuspiciousTransactionsFinland = GetSuspiciousTransactionsThreeLastDays(TransactionsFinland);


            var directoryPath = "../../../SuspiciousTransactions";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, "SuspiciousTransactions.txt");
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var transaction in SuspiciousSingleTransactions)
                {
                    writer.WriteLine($"Transaction ID: {transaction.TransactionId}, Amount: {transaction.Amount}, Date: {transaction.Date}");
                    writer.WriteLine($"Account Number: {transaction.AccountId}");
                    writer.WriteLine();
                }
            }

            Console.WriteLine($"Suspicious transactions saved to: {filePath}");
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