using BankApp.ViewModels;
using ServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Interfaces
{
    public interface ICustomerService
    {
        public Dictionary<string, int> GetCustomersPerCountry();

        public Dictionary<string, int> GetAccountsPerCountry();

            public Dictionary<string, decimal> GetBalancePerCountry();

        List<CustomerViewModel> GetAllCustomersSorted(string sortColumn, string sortOrder, int pageSize, int pageNumber, string q, out int totalCustomersCount);

        public List<CustomerViewModel> GetAllCustomers();

        List<CustomerViewModel> GetCustomerDetails(int customerId);

        public List<AccountViewModel> GetAccountInfo(int customerId);

        public decimal GetBalance(int customerId);

        public void CreateCustomer(Customer customer);

        public void UpdateCustomer(Customer customer);  

    }
}
