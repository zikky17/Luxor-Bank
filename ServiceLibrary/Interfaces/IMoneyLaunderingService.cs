using ServiceLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Interfaces
{
    public interface IMoneyLaunderingService
    {
        public List<Customer> GetCustomersSortedByCountry(string country);
        public List<Transaction> GetSuspiciousSingleTransaction(int maxAmount);
        public List<Transaction> GetTransactionsByCountry(List<Customer> customers);
        public List<Transaction> GetSuspiciousTransactionsThreeLastDays(List<Transaction> transactions);

    }
}
