using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.Customer
{
    [BindProperties]
    public class EditModel : PageModel
    {
        public EditModel(ICustomerService service, IAccountService accountService)
        {
            _customerService = service;
            _accountService = accountService;
        }

        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;

        public int CustomerId { get; set; }

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

        public List<AccountViewModel> Accounts { get; set; }

        public List<int> AccountIds { get; set; }

        public void OnGet(int customerId)
        {
            var customer = _customerService.GetCustomerDetails(customerId).First();

            CustomerId = customerId;
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
                var customer = _customerService.GetCustomerById(CustomerId);
                {
                    customer.Givenname = Givenname;
                    customer.Surname = Surname;
                    customer.Gender = Gender;
                    customer.Streetaddress = Streetaddress;
                    customer.City = City;
                    customer.Zipcode = Zipcode;
                    customer.Country = Country;
                    customer.CountryCode = CountryCode;
                    customer.Birthday = Birthday;
                    customer.NationalId = NationalId;
                    customer.Telephonecountrycode = Telephonecountrycode;
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
