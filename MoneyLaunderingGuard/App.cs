using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyLaunderingGuard
{
    public class App
    {

        private readonly ApplicationDbContext _context;
        public List<Transaction> SuspiciousTransactions { get; set; }

        public List<Account> Accounts { get; set; }

        public App(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Run()
        {

            var timeWindow = 72;
            var maxSingleTransaction = 15000;
            var maxTotalTransactions = 23000;

            SuspiciousTransactions = _context.Transactions
                .Where(t => t.Amount > maxSingleTransaction)
                .ToList();



            Accounts = _context.Accounts.ToList();

        }
    }
}