using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MoneyLaunderingGuard
{
    public class App
    {
        private readonly ApplicationDbContext _context = DatabaseService.GetDbContext();
        public DateOnly LastReportRunTime { get; set; }

        public void Run()
        {
            string filePath = "../../../LastReportRunTime.txt";
            if (File.Exists(filePath))
            {
                string lastRunTimeString = File.ReadAllText(filePath);
                if (DateOnly.TryParse(lastRunTimeString, out DateOnly lastRunTime))
                {
                    LastReportRunTime = lastRunTime;
                }
                else
                {
                    LastReportRunTime = DateOnly.FromDateTime(DateTime.Today);
                }
            }
            else
            {
                LastReportRunTime = DateOnly.FromDateTime(DateTime.Today);
            }

            ProcessReports();

            File.WriteAllText(filePath, LastReportRunTime.ToString());
        }

        private void ProcessReports()
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
            Console.WriteLine($"Getting report from {country}....");
            var customers = GetCustomersByCountry(country);
            var transactions = GetTransactionsForCustomers(customers);
            var suspiciousTransactions = GetSuspiciousTransactions(transactions);

            Console.WriteLine($"Amount of new suspicious transactions are: {suspiciousTransactions.Count}");
            AppendTransactionsToFile(suspiciousTransactions, directoryPath, $"{country}.txt");
        }

        private List<Customer> GetCustomersByCountry(string country)
        {
            var customers = _context.Customers.Where(c => c.Country == country).ToList();
            return customers;
        }

        private List<Transaction> GetTransactionsForCustomers(List<Customer> customers)
        {
            var customerIds = customers.Select(c => c.CustomerId);
            var startDate = LastReportRunTime == default(DateOnly) ? DateOnly.FromDateTime(DateTime.Today.AddDays(-3)) : LastReportRunTime;
            return _context.Dispositions
                .Where(d => customerIds.Contains(d.CustomerId))
                .SelectMany(d => d.Account.Transactions.Where(t => t.Date >= startDate))
                .ToList();
        }

        private List<Transaction> GetSuspiciousTransactions(List<Transaction> transactions)
        {
            var maxTotalTransactions = 23000;
            var maxSingleTransaction = 15000;

            var suspiciousTransactions = transactions
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

        private void AppendTransactionsToFile(List<Transaction> transactions, string directoryPath, string fileName)
        {
            var filePath = Path.Combine(directoryPath, fileName);
            using (StreamWriter writer = new StreamWriter(filePath, true))
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

