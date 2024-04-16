using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.Account
{
    [BindProperties]
    public class WithdrawModel(IAccountService service, ICustomerService customerService) : PageModel
    {
        public List<CustomerViewModel> Customer { get; set; }

        public int CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public List<AccountViewModel> Accounts { get; set; }

        public decimal AccountBalance { get; set; }

        public int AccountId { get; set; }

        [Required]
        public decimal WithdrawAmount { get; set; }

        private readonly IAccountService _accountService = service;
        private readonly ICustomerService _customerService = customerService;

        public void OnGet(int customerId, int accountId, decimal accountBalance)
        {
            Customer = _customerService.GetCustomerDetails(customerId);
            CustomerId = customerId;
            Accounts = _customerService.GetAccountInfo(customerId);
            AccountBalance = _customerService.GetBalance(accountId);
            AccountId = accountId;

            foreach(var c in Customer)
            {
                FirstName = c.FirstName;
                LastName = c.LastName;
            }
        }

        public IActionResult OnPost()
        {
            AccountBalance = _customerService.GetBalance(AccountId);
            if (ModelState.IsValid)
            {

                if (WithdrawAmount > AccountBalance)
                {
                    ModelState.AddModelError("WithdrawAmount", "Withdraw amount cannot exceed account balance.");
                    return Page();
                }

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
