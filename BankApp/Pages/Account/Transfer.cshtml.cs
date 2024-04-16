using Azure;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace BankApp.Pages.Account
{
    [BindProperties]
    public class TransferModel(IAccountService service, ICustomerService customerService) : PageModel
    {
        private readonly IAccountService _accountService = service;
        private readonly ICustomerService _customerService = customerService;

        public List<CustomerViewModel> Customers { get; set; }
        public List<AccountViewModel> Accounts { get; set; }

        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal AccountBalance { get; set; }
        public decimal TransferAmount { get; set; }

        [Required]
        [StringLength(100)]
        public string Comment { get; set; }

        public int SelectedId { get; set; }
        public List<AccountViewModel> SelectedAccount { get; set; }
        public int TransferAccountId { get; set; }


        public void OnGet(int customerId, int accountId, string firstName, string lastName, int selectedId, decimal accountBalance)
        {
            CustomerId = customerId;
            AccountId = accountId;
            Accounts = _customerService.GetAccountInfo(customerId);
            AccountBalance = _customerService.GetBalance(accountId);
            FirstName = firstName;
            LastName = lastName;
            SelectedId = selectedId;

            SelectedAccount = _customerService.GetAccountInfo(selectedId);

            Customers = _customerService.GetAllCustomers();

        }

        public IActionResult OnPost()
        {
            AccountBalance = _customerService.GetBalance(AccountId);
            if (ModelState.IsValid)
            {
                if (TransferAmount > AccountBalance)
                {
                    ModelState.AddModelError("TransferAmount", "Transfer amount cannot exceed account balance.");
                    return Page();
                }

                var withdrawTransaction = new Transaction
                {
                    AccountId = AccountId,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Type = "Debit",
                    Operation = Comment,
                    Amount = TransferAmount
                };

                var withdraw = _accountService.Withdraw(withdrawTransaction);

                var deposit = _accountService.Deposit(TransferAmount, TransferAccountId, Comment);
                ViewData["Message"] = "Transfer was successful!";
                AccountBalance = _customerService.GetBalance(AccountId);
                return Page();
            }

            return Page();
        }

    }
}
