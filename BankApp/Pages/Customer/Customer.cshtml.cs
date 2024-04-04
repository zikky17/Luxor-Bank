using BankApp.Data;
using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;
using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection.Emit;

namespace BankApp.Pages.Customer
{
    public class CustomerModel : PageModel
    {

        public CustomerModel(ICustomerService service)
        {
            _customerService = service;
        }

        private readonly ICustomerService _customerService;
        public List<CustomerViewModel> Customers { get; set; }
        public List<AccountViewModel> Accounts { get; set; }
        public decimal TotalBalance { get; set; }


        public void OnGet(int customerId)
        {
            (Customers, Accounts, TotalBalance) = _customerService.GetCustomerDetails(customerId);
        }
    }
}

