using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;
using ServiceLibrary.Services;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Net.Mail;
using System.Reflection.Emit;

namespace BankApp.Pages.Account
{
    [BindProperties]
    public class NewAccountModel : PageModel
    {

        private readonly ICustomerService _customerService;

        public NewAccountModel(ICustomerService service)
        {
            _customerService = service;
        }

        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        [Required]
        public string Frequency { get; set; } = null!;
        public DateOnly Created { get; set; }
        public decimal Balance { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public decimal Amount { get; set; }

        public void OnGet(int customerId)
        {
            CustomerId = customerId;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var account = new ServiceLibrary.Models.Account
                {
                    Frequency = Frequency,
                    Balance = Balance,
                    Created = DateOnly.FromDateTime(DateTime.Now),
                };

                _customerService.CreateAccount(CustomerId, account);
             
                return RedirectToPage("Index");
            }

            return Page();
        }

    }
}
