using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Services
{
    public class CustomerService : ICustomerService
    {

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;
        public List<CustomerViewModel> Customers { get; set; }

        public List<CustomerViewModel> GetAllCustomers(string sortColumn, string sortOrder)
        {
            var query = _context.Customers.Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerId,
                FirstName = c.Givenname,
                LastName = c.Surname,
                Country = c.Country,
            });

            switch (sortColumn)
            {
                case "FirstName":
                    query = sortOrder == "asc" ? query.OrderBy(s => s.FirstName) : query.OrderByDescending(s => s.FirstName);
                    break;
                case "LastName":
                    query = sortOrder == "asc" ? query.OrderBy(s => s.LastName) : query.OrderByDescending(s => s.LastName);
                    break;
                case "Country":
                    query = sortOrder == "asc" ? query.OrderBy(s => s.Country) : query.OrderByDescending(s => s.Country);
                    break;
                case "Id":
                    query = sortOrder == "asc" ? query.OrderBy(s => s.CustomerId) : query.OrderByDescending(s => s.CustomerId);
                    break;
            }

            return query.ToList();
        }


        public (List<CustomerViewModel>, List<AccountViewModel>, decimal) GetCustomerDetails(int customerId)
        {
            var customers = _context.Customers
                .Where(c => c.CustomerId == customerId)
                .Select(c => new CustomerViewModel
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.Givenname,
                    LastName = c.Surname,
                    Address = c.Streetaddress,
                    City = c.City,
                    ZipCode = c.Zipcode,
                    Country = c.Country,
                    Birthday = c.Birthday,
                    Email = c.Emailaddress
                }).ToList();

            var accounts = _context.Dispositions
                .Where(d => d.CustomerId == customerId)
                .Select(d => new AccountViewModel
                {
                    AccountId = d.AccountId
                }).ToList();

            var totalBalance = _context.Dispositions
                .Where(d => d.CustomerId == customerId)
                .Select(d => d.Account.Balance)
                .Sum();

            return (customers, accounts, totalBalance);
        }

    }
}
