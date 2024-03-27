using System.Collections.Generic;
using System.Linq;
using BankApp.Data;
using BankWeb.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace BankWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Dictionary<string, int> CustomersPerCountry { get; set; }
        public Dictionary<string, int> AccountsPerCountry { get; set; }
        public Dictionary<string, decimal> BalancePerCountry { get; set; }


        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            CustomersPerCountry = _context.Customers
                .GroupBy(c => c.Country)
                .ToDictionary(g => g.Key, g => g.Count());


            AccountsPerCountry = _context.Accounts
                .Where(a => a.AccountId != null)
                .SelectMany(a => a.Dispositions)
                .Select(d => new { d.AccountId, Country = d.Customer.Country })
                .GroupBy(d => d.Country)
                .ToDictionary(g => g.Key, g => g.Select(d => d.AccountId).Distinct().Count());

            BalancePerCountry = _context.Dispositions
                .Include(d => d.Account)
                .Where(d => d.Account.Balance > 0)
                .GroupBy(d => d.Customer.Country)
                .ToDictionary(g => g.Key, g => g.Sum(d => d.Account.Balance));


            Response.Headers[HeaderNames.CacheControl] = "public,max-age=60";
            Response.Headers[HeaderNames.Vary] = "Accept-Encoding,User-Agent,Country";
        }


    }
}

