using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using System.ComponentModel.DataAnnotations;

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
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal AccountBalance { get; set; }

        [Required(ErrorMessage = "Enter an receiving account Id.")]
        public int TransferAccountId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(100)]
        public string Comment { get; set; }

        public int SelectedId { get; set; }
        public List<AccountViewModel> SelectedAccount { get; set; }


        public void OnGet(int customerId, int accountId, string firstName, string lastName, int selectedId)
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
                if (Amount > AccountBalance)
                {
                    ModelState.AddModelError("Amount", "Transfer amount cannot exceed account balance.");
                    return Page();
                }

                var withdrawTransaction = new Transaction
                {
                    AccountId = AccountId,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Type = "Debit",
                    Operation = Comment,
                    Amount = Amount
                };

                var message = _accountService.Deposit(Amount, TransferAccountId, Comment);

                if (message == StatusMessage.CantFindAccount)
                {
                    ModelState.AddModelError("TransferAccountId", "Unknown account number.");
                    return Page();
                }
                else if (message != StatusMessage.Approved)
                {
                    ModelState.AddModelError("Comment", "Transfer was not successful.");
                    return Page();
                }

                _accountService.Withdraw(withdrawTransaction);

                ViewData["Message"] = "Transfer was successful!";
                TempData["Message"] = ViewData["Message"];
                AccountBalance = _customerService.GetBalance(AccountId);
                return Page();
            }

            return Page();
        }


    }
}
