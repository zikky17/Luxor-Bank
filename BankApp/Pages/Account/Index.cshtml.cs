using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using BankApp.Infrastructure.Paging;
using ServiceLibrary.ViewModels;

namespace BankApp.Pages.Account
{
    [BindProperties]
    public class IndexModel(IAccountService service) : PageModel
    {
        private readonly IAccountService _accountService = service;
        public AccountViewModel Account { get; set; }
        public decimal? AccountBalance { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }

        public void OnGet(int accountId, string firstName, string lastName, int customerId)
        {
            FirstName = firstName;
            LastName = lastName;
            CustomerId = customerId;

            AccountId = accountId;
            Account = _accountService.GetAccountInfo(accountId);

            AccountBalance = Account.Balance;

            Transactions = _accountService.GetTransactions(accountId);

        }

        public IActionResult OnPost(decimal accountBalance)
        {
            AccountBalance = accountBalance;

            if (AccountBalance > 0)
            {
                ModelState.AddModelError("AccountBalance", "Account balance must be 0 before deleting the account. Make a withdraw or a transfer.");
                return Page();
            }

            _accountService.DeleteAccount(AccountId);
            return RedirectToPage("/Customer/CustomerDetails", new { customerId = CustomerId, firstName = FirstName, lastName = LastName });

            return Page();
        }

        public IActionResult OnGetShowMore(int accountId, int pageNo)
        {
            var allTransactions = _accountService.GetTransactions(accountId)
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Date)
                .AsQueryable();

            var pagedTransactions = allTransactions.GetPaged(pageNo, 20);

            var listOfTransactions = pagedTransactions.Results
                .Select(t => new TransactionViewModel
                {
                    Date = t.Date,
                    Operation = t.Operation,
                    Type = t.Type,
                    Amount = t.Amount
                }).ToList();

            return new JsonResult(new { transactions = listOfTransactions });
        }


    }
}
