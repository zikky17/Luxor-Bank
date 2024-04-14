using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Services
{
    public class CountryService(ApplicationDbContext context) : ICountryService
    {

        private readonly ApplicationDbContext _context = context;

        public List<Disposition> GetDispositions(string country)
        {
            var dispositions = _context.Dispositions
            .Include(d => d.Account)
            .Include(d => d.Customer)
             .Where(d => d.Account.Balance > 0 && d.Customer.Country == country)
             .ToList();

            return dispositions;
        }

        public List<(string FullName, decimal Balance, int CustomerId)> GetTopTenCustomers(List<Disposition> dispositions)
        {
            var customers = dispositions
                .GroupBy(d => new { d.Customer.Givenname, d.Customer.Surname, d.CustomerId })
                .OrderByDescending(g => g.Sum(d => d.Account.Balance))
                .Take(10)
                .Select(g => (FullName: $"{g.Key.Givenname} {g.Key.Surname}", Balance: g.Sum(d => d.Account.Balance), CustomerId: g.Key.CustomerId))
                .ToList();

            return customers;
        }
    }
}
