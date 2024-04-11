using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using System.Collections.Generic;
using System.Linq;

namespace BankApp.Pages
{
    public class CountryModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CountryModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string Country { get; set; }
        public List<(string FullName, decimal Balance, int CustomerId)> TopTenCustomers { get; set; }

        public IActionResult OnGet(string country)
        {
            if (country == null)
            {
                return RedirectToPage("/Index");
            }

            Country = country;

            var dispositions = _context.Dispositions
                .Include(d => d.Account)
                .Include(d => d.Customer)
                .Where(d => d.Account.Balance > 0 && d.Customer.Country == country)
                .ToList();

            TopTenCustomers = dispositions
                .GroupBy(d => new { d.Customer.Givenname, d.Customer.Surname, d.CustomerId })
                .OrderByDescending(g => g.Sum(d => d.Account.Balance))
                .Take(10)
                .Select(g => (FullName: $"{g.Key.Givenname} {g.Key.Surname}", Balance: g.Sum(d => d.Account.Balance), CustomerId: g.Key.CustomerId))
                .ToList();

            return Page();
        }
    }
}
