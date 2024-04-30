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


        public List<string> GetCountries()
        {
            var countries = _context.Customers.Select(c => c.Country).Distinct().ToList();
            return countries;
        }

        public List<int> GetCustomersPerCountry(List<string> countries)
        {
            var customersByCountry = new List<int>();

            foreach (var country in countries)
            {
                var customerCount = _context.Customers.Count(c => c.Country == country);
                customersByCountry.Add(customerCount);
            }

            return customersByCountry;
        }

        public List<Disposition> GetDispositions(string country)
        {
            return _context.Dispositions
                .Include(d => d.Account)
                .Include(d => d.Customer)
                .Where(d => d.Account.Balance > 0 && d.Customer.Country == country)
                .ToList();
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
