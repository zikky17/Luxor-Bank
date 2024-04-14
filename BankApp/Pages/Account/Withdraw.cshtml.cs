using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;

namespace BankApp.Pages.Account
{
    [BindProperties]
    public class WithdrawModel(IAccountService service, ICustomerService customerService) : PageModel
    {
        public List<CustomerViewModel> Customers { get; set; }

        public List<AccountViewModel> Accounts { get; set; }

        public decimal AccountBalance { get; set; }

        public int AccountId { get; set; }

        public decimal WithdrawAmount { get; set; }

        private readonly IAccountService _accountService = service;
        private readonly ICustomerService _customerService = customerService;

        public void OnGet(int customerId, int accountId, decimal accountBalance)
        {
            Customers = _customerService.GetCustomerDetails(customerId);
            Accounts = _customerService.GetAccountInfo(customerId);
            AccountBalance = _customerService.GetBalance(accountId);
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

                _accountService.Withdraw(transaction);
                ViewData["Message"] = "Withdraw was successful!";
                AccountBalance = _customerService.GetBalance(AccountId);
                return Page();


            }
            return Page();
        }
    }
}
