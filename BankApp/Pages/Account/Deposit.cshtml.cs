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
    public class DepositModel : PageModel
    {
        public DepositModel(IAccountService service, ICustomerService customerService)
        {
            _accountService = service;
            _customerService = customerService;
        }

        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;

        public List<CustomerViewModel> Customers { get; set; }

        public List<AccountViewModel> Accounts { get; set; }

        public decimal AccountBalance { get; set; }

        public int AccountId { get; set; }

        public int CustomerId { get; set; }

        [Range(100, 10000)]
        public decimal DepositAmount { get; set; }

        [Required]
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
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var depositResult = _accountService.Deposit(DepositAmount, AccountId, Comment);

                if (depositResult == StatusMessage.Approved)
                {
                    ViewData["Message"] = "Deposit was successful!";
                    AccountBalance = _customerService.GetBalance(AccountId);
                    return Page();
                }

                if (depositResult == StatusMessage.MessageRequired)
                {
                    ModelState.AddModelError("Comment", "Please enter a comment for this deposit.");
                }

                if (depositResult == StatusMessage.IncorrectAmount)
                {
                    ModelState.AddModelError("DepositAmount", "Please enter a correct amount between 100 - 10,000");
                }
            }

            AccountBalance = _customerService.GetBalance(AccountId);
            return Page();
        }
    }
}
