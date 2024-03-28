using BankApp.Data;
using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace BankApp.Pages
{
    public class AccountModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<AccountViewModel> Accounts { get; set; }

        public AccountModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(int accountId)
        {
            Accounts = _context.Dispositions
                .Include(d => d.Account)
                .Where(d => d.AccountId == accountId)
                .Select(d => new AccountViewModel
                {
                    AccountId = d.AccountId,
                    Created = d.Account.Created,
                    Balance = d.Account.Balance,
                    Transactions = d.Account.Transactions.ToList()
                })
                .ToList();
        }
    }
}
