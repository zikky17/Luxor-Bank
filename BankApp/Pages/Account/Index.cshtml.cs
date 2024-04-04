using BankApp.Data;
using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace BankApp.Pages.Account
{
    public class AccountModel : PageModel
    {
        public AccountModel(IAccountService service)
        {
            _accountService = service;
        }

        private readonly IAccountService _accountService;
        public List<AccountViewModel> Accounts { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CustomerId { get; set; }

        public void OnGet(int accountId, string firstName, string lastName, int customerId)
        {
            FirstName = firstName;
            LastName = lastName;
            CustomerId = customerId;

            Accounts = _accountService.GetAccountInfo(accountId);

            foreach (var account in Accounts)
            {
                account.Transactions = account.Transactions.OrderByDescending(t => t.Date).ToList();
            }
        }
    }
}
