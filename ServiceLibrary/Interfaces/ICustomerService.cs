using BankApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Interfaces
{
    public interface ICustomerService
    {
        List<CustomerViewModel> GetAllCustomers(string sortColumn, string sortOrder, int pageNumber, string q);

        (List<CustomerViewModel>, List<AccountViewModel>, decimal) GetCustomerDetails(int customerId);

        int GetTotalCustomersCount();
    }
}
