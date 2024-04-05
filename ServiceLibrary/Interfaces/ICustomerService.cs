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
        List<CustomerViewModel> GetAllCustomers(string sortColumn, string sortOrder, int pageSize, int pageNumber, string q, out int totalCustomersCount);

        (List<CustomerViewModel>, List<AccountViewModel>, decimal) GetCustomerDetails(int customerId);

        public void CreateCustomer(Customer customer);

    }
}
