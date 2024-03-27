using BankWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Pages
{
    public class CountryModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CountryModel (ApplicationDbContext context)
        {
            _context = context;
        }

        public string Country { get; set; }
        public Dictionary<string, decimal> TopTenCustomers { get; set; }


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
                .GroupBy(d => new { d.Customer.Givenname, d.Customer.Surname })
                .OrderByDescending(g => g.Sum(d => d.Account.Balance))
                .Take(10)
                .ToDictionary(g => $"{g.Key.Givenname} {g.Key.Surname}", g => g.Sum(d => d.Account.Balance));

            return Page();
        }
    }
}
