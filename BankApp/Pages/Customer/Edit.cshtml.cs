using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.Customer
{
    [BindProperties]
    public class EditModel(ICustomerService service, IAccountService accountService) : PageModel
    {
        private readonly IAccountService _accountService = accountService;
        private readonly ICustomerService _customerService = service;

        public int CustomerId { get; set; }

        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter a name between 2-50 characters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter a last name between 2-50 characters")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter an address between 2-50 characters")]
        public string Streetaddress { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter a city between 2-50 characters")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Enter a Zip Code between 2-10 characters")]
        public string Zipcode { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Country { get; set; } = null!;

        [StringLength(2)]
        public string CountryCode { get; set; } = null!;

        public DateOnly? Birthday { get; set; }

        public string? NationalId { get; set; }

        public string? TelephoneCountryCode { get; set; }

        public string? Telephonenumber { get; set; }

        public string? Emailaddress { get; set; }

        public List<AccountViewModel> Accounts { get; set; }

        public List<int> AccountIds { get; set; }

        public void OnGet(int customerId)
        {
            var customer = _customerService.GetCustomerDetails(customerId).First();

            CustomerId = customerId;
            Gender = customer.Gender;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Streetaddress = customer.Address;
            City = customer.City;
            Zipcode = customer.ZipCode;
            Country = customer.Country;
            CountryCode = customer.CountryCode;
            Birthday = customer.Birthday;
            NationalId = customer.NationalId;
            TelephoneCountryCode = customer.Telephonecountrycode;
            Telephonenumber = customer.Telephonenumber;
            Emailaddress = customer.Email;

        }

        public IActionResult OnPost()
        {
            if(ModelState.IsValid)
            {
                var customer = _customerService.GetCustomerById(CustomerId);
                {
                    customer.Givenname = FirstName;
                    customer.Surname = LastName;
                    customer.Gender = Gender;
                    customer.Streetaddress = Streetaddress;
                    customer.City = City;
                    customer.Zipcode = Zipcode;
                    customer.Country = Country;
                    customer.CountryCode = CountryCode;
                    customer.Birthday = Birthday;
                    customer.NationalId = NationalId;
                    customer.Telephonecountrycode = TelephoneCountryCode;
                    customer.Telephonenumber = Telephonenumber;
                    customer.Emailaddress = Emailaddress;
                };

                _customerService.UpdateCustomer(customer);
                ViewData["Message"] = "Customer updated successfully!";
                return Page();
            }
            return Page();
        }

        public IActionResult OnPostDelete(int customerId)
        {
            Accounts = _customerService.GetAccountInfo(CustomerId);
            foreach(var account in Accounts)
            {
                AccountIds.Add(account.AccountId);
            }
            _accountService.DeleteAllAccounts(AccountIds);
            _customerService.DeleteCustomer(customerId);
            ViewData["Message"] = "Customer deleted successfully!";
            return RedirectToPage("Index");
        }

    }
}
