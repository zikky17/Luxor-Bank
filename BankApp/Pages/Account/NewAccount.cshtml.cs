using AutoMapper;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Net.Mail;
using System.Reflection.Emit;

namespace BankApp.Pages.Account
{
    [BindProperties]
    public class NewAccountModel(IAccountService service, IMapper mapper) : PageModel
    {

        private readonly IAccountService _accountService = service;
        private readonly IMapper _mapper = mapper;

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public AccountViewModel AccountVM { get; set; }
        //public int AccountId { get; set; }
        //[Required]
        //public string Frequency { get; set; } = null!;
        //public DateOnly Created { get; set; }
        //public decimal Balance { get; set; }
        //public ICollection<Transaction> Transactions { get; set; }

        public decimal Amount { get; set; }

        public void OnGet(int customerId, string firstName, string lastName)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var account = new ServiceLibrary.Data.Account();
                _mapper.Map(AccountVM, account);

                _accountService.CreateAccount(CustomerId, account);


                return RedirectToPage("/Customer/CustomerDetails", new { customerId = CustomerId, firstName = FirstName, lastName = LastName });
            }

            return Page();
        }
    }
}