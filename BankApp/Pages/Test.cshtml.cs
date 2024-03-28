using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;

namespace BankApp.Pages
{
    public class TestModel : PageModel
    {

        public ICustomerService _customerService { get; set; }
        public List<CustomerViewModel> Customers { get; set; }

        public TestModel(ICustomerService service)
        {
            _customerService = service;
        }

        public void OnGet(string sortColumn, string sortOrder)
        {

            Customers = _customerService.GetAllCustomers(sortColumn, sortOrder);
        }
    }
}
