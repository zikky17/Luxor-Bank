using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;
using ServiceLibrary.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace BankApp.Pages.Customer
{
    [BindProperties]
    public class CreateModel : PageModel
    {

        public CreateModel(ICustomerService service, IAccountService accountService)
        {
            _customerService = service;
            _accountService = accountService;
        }

        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;

        public string Gender { get; set; } = null!;

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter a name between 2 - 50 characters.")]
        public string Givenname { get; set; } = null!;

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter a name between 2 - 50 characters.")]
        public string Surname { get; set; } = null!;

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter an address between 2 - 50 characters.")]
        public string Streetaddress { get; set; } = null!;

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter a city between 2 - 50 characters.")]
        public string City { get; set; } = null!;

        [StringLength(10, MinimumLength = 2, ErrorMessage = "Enter a ZIP code between 2 - 10 characters.")]
        public string Zipcode { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [StringLength(2)]
        public string CountryCode { get; set; } = null!;

        public DateOnly? Birthday { get; set; }

        public string? NationalId { get; set; }

        public string? Telephonecountrycode { get; set; }

        public string? Telephonenumber { get; set; }

        public string? Emailaddress { get; set; }

        public void OnGet()
        {
          
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var customer = new ServiceLibrary.Models.Customer
                {
                    Givenname = Givenname,
                    Surname = Surname,
                    Gender = Gender,
                    Streetaddress = Streetaddress,
                    City = City,
                    Zipcode = Zipcode,
                    Country = Country,
                    CountryCode = CountryCode,
                    Birthday = Birthday,
                    NationalId = NationalId,
                    Telephonecountrycode = Telephonecountrycode,
                    Telephonenumber = Telephonenumber,
                    Emailaddress = Emailaddress,
                };

                _customerService.CreateCustomer(customer);
                var newAccount = new ServiceLibrary.Models.Account
                {
                    Frequency = "AfterTransaction",
                    Created = DateOnly.FromDateTime(DateTime.Now)
                };
                _accountService.CreateAccount(customer.CustomerId, newAccount);

                return RedirectToPage("Index");
            }

            return Page();
        }

    }
}
