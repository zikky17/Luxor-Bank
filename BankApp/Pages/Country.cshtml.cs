using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;
using System.Collections.Generic;
using System.Linq;

namespace BankApp.Pages
{
    public class CountryModel(ICountryService service) : PageModel
    {

        private readonly ICountryService _countryService = service;

        public string Country { get; set; }

        public List<Disposition> Dispositions { get; set; }

        public List<(string FullName, decimal Balance, int CustomerId)> TopTenCustomers { get; set; }

        public IActionResult OnGet(string country)
        {

            Country = country;

            Dispositions = _countryService.GetDispositions(Country);

            TopTenCustomers = _countryService.GetTopTenCustomers(Dispositions);
               

            return Page();
        }
    }
}
