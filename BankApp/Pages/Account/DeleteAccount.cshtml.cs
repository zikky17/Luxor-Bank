using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;

namespace BankApp.Pages.Account
{
    public class DeleteAccountModel : PageModel
    {

        private readonly IAccountService _accountService;

        public DeleteAccountModel(IAccountService service)
        {
            _accountService = service;
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal AccountBalance { get; set; }

        public IActionResult OnGet(int accountId, int customerId, string firstName, string lastName, decimal accountBalance)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            AccountBalance = accountBalance;

            if (AccountBalance > 0)
            {
                ModelState.AddModelError(string.Empty, "Account balance must be 0 before deleting the account.");
                return Page();
            }

            _accountService.DeleteAccount(accountId);
            return RedirectToPage("/Customer/CustomerDetails", new { customerId = CustomerId, firstName = FirstName, lastName = LastName });
        }
    }
}
