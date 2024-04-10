using Azure;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace BankApp.Pages.Account
{
    [BindProperties]
    public class TransferModel : PageModel
    {

        public TransferModel(IAccountService service, ICustomerService customerService)
        {
            _accountService = service;
            _customerService = customerService;
        }

        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;

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
        public string SelectedFirstName { get; set; }
        public string SelectedLastName { get; set; }
        public List<AccountViewModel> SelectedAccount { get; set; }
        public int TransferAccountId { get; set; }


        public void OnGet(int customerId, int accountId, string firstName, string lastName, int selectedId, string selectedFirstname, string selectedLastName, decimal accountBalance)
        {
            CustomerId = customerId;
            AccountId = accountId;
            Accounts = _customerService.GetAccountInfo(customerId);
            AccountBalance = _customerService.GetBalance(accountId);
            FirstName = firstName;
            LastName = lastName;
            SelectedId = selectedId;
            SelectedFirstName = selectedFirstname;
            SelectedLastName = selectedLastName;

            SelectedAccount = _customerService.GetAccountInfo(selectedId);

            Customers = _customerService.GetAllCustomers();

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var selectedAccount = _customerService.GetAccountInfo(SelectedId);

                var withdrawTransaction = new Transaction
                {
                    AccountId = AccountId,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Type = "Debit",
                    Operation = Comment,
                    Amount = TransferAmount
                };

                var withdraw = _accountService.Withdraw(withdrawTransaction);

                TransferAccountId = selectedAccount.First().AccountId;

                var deposit = _accountService.Deposit(TransferAmount, TransferAccountId, Comment);
            }

            return RedirectToPage("Index");
        }

    }
}
