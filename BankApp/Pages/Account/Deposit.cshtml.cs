using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;
using ServiceLibrary.Services;

namespace BankApp.Pages
{
    [BindProperties]
    public class DepositModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;

        public DepositModel(IAccountService service, ICustomerService customerService)
        {
            _accountService = service;
            _customerService = customerService;
        }

        public List<CustomerViewModel> Customers { get; set; }

        public List<AccountViewModel> Accounts { get; set; }

        public decimal TotalBalance { get; set; }
 
        public int AccountId { get; set; }

        [Range(100, 10000)]
        public decimal DepositAmount { get; set; }

        [Required]
        public string Comment { get; set; }


        public void OnGet(int customerId, int accountId)
        {
            (Customers, Accounts, TotalBalance) = _customerService.GetCustomerDetails(customerId);
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
                    Type = "Credit",
                    Operation = Comment,
                    Amount = DepositAmount,
                };

                var depositResult = _accountService.Deposit(transaction, AccountId);

                if (depositResult)
                {
                    return Page();
                }
                else
                {

                    return Page();
                }
            }
            return Page();
        }
    }
}
