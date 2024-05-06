using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;

namespace BankApp.Pages
{
    [BindProperties]
    public class DepositModel(IAccountService service, ICustomerService customerService) : PageModel
    {
        private readonly IAccountService _accountService = service;
        private readonly ICustomerService _customerService = customerService;

        public List<CustomerViewModel> Customers { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public List<AccountViewModel> Accounts { get; set; }

        public decimal AccountBalance { get; set; }

        public int AccountId { get; set; }

        public int CustomerId { get; set; }

        [Range(100, 25000, ErrorMessage = "Amount must be between 100 and 25000.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(100)]
        public string Comment { get; set; }

        public StatusMessage DepositResult { get; set; }


        public void OnGet(int customerId, int accountId)
        {
            CustomerId = customerId;
            Customers = _customerService.GetCustomerDetails(customerId);
            Accounts = _customerService.GetAccountInfo(customerId);
            AccountBalance = _customerService.GetBalance(accountId);
            AccountId = accountId;
            DepositResult = StatusMessage.None;

            foreach (var c in Customers)
            {
                FirstName = c.FirstName;
                LastName = c.LastName;
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var depositResult = _accountService.Deposit(Amount, AccountId, Comment);

                switch (depositResult)
                {
                    case StatusMessage.CantFindAccount:
                        ModelState.AddModelError("Comment", "Unknown account number.");
                        break;

                    case StatusMessage.Approved:
                        ViewData["Message"] = "Deposit was successful!";
                        TempData["Message"] = ViewData["Message"];
                        AccountBalance = _customerService.GetBalance(AccountId);
                        return Page();

                    case StatusMessage.MessageRequired:
                        ModelState.AddModelError("Comment", "Please enter a comment for this deposit.");
                        break;

                    case StatusMessage.IncorrectAmount:
                        ModelState.AddModelError("Amount", "Please enter a correct amount between 100 - 10,000");
                        break;
                }

            }

            AccountBalance = _customerService.GetBalance(AccountId);
            return Page();
        }
    }
}
