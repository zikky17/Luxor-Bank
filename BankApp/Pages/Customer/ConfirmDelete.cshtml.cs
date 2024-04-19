using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;

namespace BankApp.Pages.Customer
{
    public class ConfirmDeleteModel(IAccountService accountService, ICustomerService service) : PageModel
    {
        private readonly IAccountService _accountService = accountService;
        private readonly ICustomerService _customerService = service;

        public int CustomerId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<AccountViewModel> Accounts { get; set; }
        public CustomerViewModel CustomerVM { get; set; }
        public List<int> AccountIds { get; set; } = new List<int>();

        public void OnGet(int customerId, string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            CustomerId = customerId;
        }

        public IActionResult OnPost(int customerId)
        {
            Accounts = _customerService.GetAccountInfo(customerId);
            foreach (var account in Accounts)
            {
                AccountIds.Add(account.AccountId);
            }
            _accountService.DeleteAllAccounts(AccountIds);
            _customerService.DeleteCustomer(customerId);
            ViewData["Message"] = "Customer deleted successfully!";
            return RedirectToPage("Index");
        }
    }
}