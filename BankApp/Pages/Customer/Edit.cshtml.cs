using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.Customer
{
    [BindProperties]
    public class EditModel : PageModel
    {
        public EditModel(ICustomerService service)
        {
            _customerService = service;
        }

        private readonly ICustomerService _customerService;

        public string Gender { get; set; } = null!;

        [StringLength(50, MinimumLength = 2)]
        public string Givenname { get; set; } = null!;

        [StringLength(50, MinimumLength = 2)]
        public string Surname { get; set; } = null!;

        [StringLength(50, MinimumLength = 2)]
        public string Streetaddress { get; set; } = null!;

        [StringLength(50, MinimumLength = 2)]
        public string City { get; set; } = null!;

        [StringLength(50, MinimumLength = 2)]
        public string Zipcode { get; set; } = null!;

        [StringLength(50, MinimumLength = 2)]
        public string Country { get; set; } = null!;

        [StringLength(2)]
        public string CountryCode { get; set; } = null!;

        public DateOnly? Birthday { get; set; }

        public string? NationalId { get; set; }

        public string? Telephonecountrycode { get; set; }

        public string? Telephonenumber { get; set; }

        public string? Emailaddress { get; set; }

        public void OnGet(int customerId)
        {
            var customer = _customerService.GetCustomerDetails(customerId).First();

            Gender = customer.Gender;
            Givenname = customer.FirstName;
            Surname = customer.LastName;
            Streetaddress = customer.Address;
            City = customer.City;
            Zipcode = customer.ZipCode;
            Country = customer.Country;
            CountryCode = customer.CountryCode;
            Birthday = customer.Birthday;
            NationalId = customer.NationalId;
            Telephonecountrycode = customer.Telephonecountrycode;
            Telephonenumber = customer.Telephonenumber;
            Emailaddress = customer.Email;

        }

        public IActionResult OnPost()
        {
            if(ModelState.IsValid)
            {
                var customer = new ServiceLibrary.Data.Customer
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

                _customerService.UpdateCustomer(customer);
                ViewData["Message"] = "Customer updated successfully!";
                return Page();
            }
            return Page();
        }

    }
}
