using AutoMapper;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace BankApp.Pages.Customer
{
    [BindProperties]
    public class CreateModel(ICustomerService service, IAccountService accountService, IMapper mapper) : PageModel
    {
        private readonly ICustomerService _customerService = service;
        private readonly IAccountService _accountService = accountService;
        private readonly IMapper _mapper = mapper;

        public string Gender { get; set; } = null!;

        [Required (ErrorMessage = "This field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter a name between 2 - 50 characters.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter a name between 2 - 50 characters.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter an address between 2 - 50 characters.")]
        public string Streetaddress { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter a city between 2 - 50 characters.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Enter a ZIP code between 2 - 10 characters.")]
        public string Zipcode { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Country { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(2)]
        public string CountryCode { get; set; } = null!;

        public DateOnly? Birthday { get; set; }

        public string? NationalId { get; set; }

        public string? Telephonecountrycode { get; set; }

        public string? Telephonenumber { get; set; }

        public string? Emailaddress { get; set; }

        public CustomerViewModel Customer { get; set; }

        public void OnGet()
        {
          
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                //var customer = new ServiceLibrary.Data.Customer
                //{
                //    Givenname = FirstName,
                //    Surname = LastName,
                //    Gender = Gender,
                //    Streetaddress = Streetaddress,
                //    City = City,
                //    Zipcode = Zipcode,
                //    Country = Country,
                //    CountryCode = CountryCode,
                //    Birthday = Birthday,
                //    NationalId = NationalId,
                //    Telephonecountrycode = Telephonecountrycode,
                //    Telephonenumber = Telephonenumber,
                //    Emailaddress = Emailaddress,
                //};

                var customer = new ServiceLibrary.Data.Customer();
                //_mapper.Map(CustomerViewModel, customer);

                _customerService.CreateCustomer(customer);
                var newAccount = new ServiceLibrary.Data.Account
                {
                    Frequency = "AfterTransaction",
                    Created = DateOnly.FromDateTime(DateTime.Now)
                };
                _accountService.CreateAccount(customer.CustomerId, newAccount);
                ViewData["Message"] = "Customer created successfully!";

                return Page();
            }

            return Page();
        }

    }
}
