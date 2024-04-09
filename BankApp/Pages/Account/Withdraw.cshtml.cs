using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;

namespace BankApp.Pages.Account
{
    [BindProperties]
    public class WithdrawModel : PageModel
    {
        public List<CustomerViewModel> Customers { get; set; }

        public List<AccountViewModel> Accounts { get; set; }

        public decimal TotalBalance { get; set; }

        public int AccountId { get; set; }

        public decimal WithdrawAmount { get; set; }

        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;

        public WithdrawModel(IAccountService service, ICustomerService customerService)
        {
            _accountService = service;
            _customerService = customerService;
        }

        public void OnGet(int customerId, int accountId)
        {
            Customers = _customerService.GetCustomerDetails(customerId);
            Accounts = _customerService.GetAccountInfo(customerId);
            TotalBalance = _customerService.GetBalance(customerId);
            AccountId = accountId;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var transaction = new Transaction
                {
                    AccountId = AccountId,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Type = "Debit",
                    Operation = "Withdraw from customer",
                    Amount = WithdrawAmount,
                };

                var withdrawResult = _accountService.Withdraw(transaction);
                return RedirectToPage("Index");

              
            }
            return Page();
        }
    }
}
