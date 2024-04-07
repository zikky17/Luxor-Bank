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
        public decimal TotalBalance { get; set; }
        public decimal TransferAmount { get; set; }
        public string Q { get; set; }
        public int CurrentPage { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public int PageSize { get; set; } = 50;
        public int TotalPages { get; set; }

        [Required]
        [StringLength(100)]
        public string Comment { get; set; }

        public int SelectedId { get; set; }
        public string SelectedFirstName { get; set; }
        public string SelectedLastName { get; set; }
        public List<AccountViewModel> SelectedAccount { get; set; }


        public void OnGet(int customerId, int accountId, string sortColumn, string sortOrder, int pageNumber,
           string q, string firstName, string lastName, int selectedId, string selectedFirstname, string selectedLastName)
        {
            CustomerId = customerId;
            AccountId = accountId;
            Accounts = _customerService.GetAccountInfo(customerId);
            TotalBalance = _customerService.GetBalance(customerId);
            AccountId = accountId;
            Q = q;
            SortColumn = sortColumn;
            SortOrder = sortOrder;
            CurrentPage = pageNumber;
            FirstName = firstName;
            LastName = lastName;
            SelectedId = selectedId;
            SelectedFirstName = selectedFirstname;
            SelectedLastName = selectedLastName;

            SelectedAccount = _customerService.GetAccountInfo(selectedId);

            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }

            Customers = _customerService.GetAllCustomers(sortColumn, sortOrder, PageSize, CurrentPage, q, out int totalCustomersCount);

            TotalPages = totalCustomersCount == 0 ? 1 : (int)Math.Ceiling((double)totalCustomersCount / PageSize);
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
                    Type = "Credit",
                    Operation = Comment,
                    Amount = TransferAmount
                };

                var withdraw = _accountService.Withdraw(withdrawTransaction, AccountId);

                AccountId = selectedAccount.First().AccountId;

                var deposit = _accountService.Deposit(TransferAmount, AccountId, Comment);
            }

            return RedirectToPage("Transfer");
        }

    }
}
