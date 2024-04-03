using System.Collections.Generic;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;
using ServiceLibrary.Services;

namespace BankApp.Pages
{
    public class DepositModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;

        [BindProperty]
        public List<CustomerViewModel> Customers { get; set; }
        [BindProperty]
        public List<AccountViewModel> Accounts { get; set; }
        [BindProperty]
        public decimal TotalBalance { get; set; }
        [BindProperty]
        public int AccountId { get; set; }
        [BindProperty]
        public decimal DepositAmount { get; set; }

        public DepositModel(IAccountService service, ICustomerService customerService)
        {
            _accountService = service;
            _customerService = customerService;
        }

        public void OnGet(int customerId, int accountId)
        {
            (Customers, Accounts, TotalBalance) = _customerService.GetCustomerDetails(customerId);
            AccountId = accountId;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var transaction = new Transaction
            {
                AccountId = AccountId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = "Credit",
                Operation = "Deposit from customer",
                Amount = DepositAmount,
            };

            var depositResult = _accountService.Deposit(transaction, AccountId);

            if (depositResult)
            {
                TotalBalance += DepositAmount;
                return Page();
            }
            else
            {

                return Page();
            }
        }
    }
}
