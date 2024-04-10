using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using ServiceLibrary.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.Account
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        public IndexModel(IAccountService service)
        {
            _accountService = service;
        }

        private readonly IAccountService _accountService;
        public List<AccountViewModel> Accounts { get; set; }
        public decimal AccountBalance { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CustomerId { get; set; }
        public int AccountId { get; set; }

        public void OnGet(int accountId, string firstName, string lastName, int customerId)
        {
            FirstName = firstName;
            LastName = lastName;
            CustomerId = customerId;
            AccountId = accountId;
            Accounts = _accountService.GetAccountInfo(accountId);

            foreach(var account in Accounts)
            {
                AccountBalance = account.Balance;
            }


            foreach (var account in Accounts)
            {
                account.Transactions = account.Transactions.OrderByDescending(t => t.Date).ToList();
            }
        }

        public IActionResult OnPost(decimal accountBalance)
        {
            AccountBalance = accountBalance;

            if (ModelState.IsValid)
            {
                if (AccountBalance > 0)
                {
                    ModelState.AddModelError(string.Empty, "Account balance must be 0 before deleting the account.");
                    return Page();
                }

                _accountService.DeleteAccount(AccountId);
                return RedirectToPage("/Customer/CustomerDetails", new { customerId = CustomerId, firstName = FirstName, lastName = LastName });
            }

            return Page();
        }
    }
}
