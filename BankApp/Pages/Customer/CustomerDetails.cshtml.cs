using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ServiceLibrary.Interfaces;
using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection.Emit;

namespace BankApp.Pages.Customer
{
    public class CustomerDetailsModel : PageModel
    {

        public CustomerDetailsModel(ICustomerService service, IAccountService accountService)
        {
            _customerService = service;
            _accountService = accountService;
        }

        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;

        public int AccountId { get; set; }
        public List<CustomerViewModel> Customers { get; set; }
        public List<AccountViewModel> Accounts { get; set; }
        public decimal TotalBalance { get; set; }

        public void OnGet(int customerId)
        {
            Customers = _customerService.GetCustomerDetails(customerId);
            Accounts = _customerService.GetAccountInfo(customerId);
            TotalBalance = _customerService.GetTotalBalance(customerId);
        }
    }
}

