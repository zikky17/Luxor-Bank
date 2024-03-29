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

namespace BankApp.Pages
{
    public class AccountModel : PageModel
    {
        public AccountModel(IAccountService service)
        {
            _accountService = service;
        }

        private readonly IAccountService _accountService;
        public List<AccountViewModel> Accounts { get; set; }


        public void OnGet(int accountId)
        {
            Accounts = _accountService.GetAccountInfo(accountId);
        }
    }
}
