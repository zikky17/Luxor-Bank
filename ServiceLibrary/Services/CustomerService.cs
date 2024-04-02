using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLibrary.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CustomerViewModel> GetAllCustomers(string sortColumn, string sortOrder, int pageSize, int pageNumber, string q, out int totalCustomersCount)
        {
            var query = _context.Customers.Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerId,
                FirstName = c.Givenname,
                LastName = c.Surname,
                City = c.City
            });

            if (!string.IsNullOrEmpty(q))
            {
                if (int.TryParse(q, out int customerId))
                {
                    query = query.Where(c =>
                        c.City.Contains(q) ||
                        c.FirstName.Contains(q) ||
                        c.LastName.Contains(q) ||
                        c.CustomerId == customerId);
                }
                else
                {
                    query = query.Where(c =>
                        c.City.Contains(q) ||
                        c.FirstName.Contains(q) ||
                        c.LastName.Contains(q));
                }
            }

            switch (sortColumn)
            {
                case "FirstName":
                    query = sortOrder == "asc" ? query.OrderBy(s => s.FirstName) : query.OrderByDescending(s => s.FirstName);
                    break;
                case "LastName":
                    query = sortOrder == "asc" ? query.OrderBy(s => s.LastName) : query.OrderByDescending(s => s.LastName);
                    break;
                case "City":
                    query = sortOrder == "asc" ? query.OrderBy(s => s.City) : query.OrderByDescending(s => s.City);
                    break;
                case "Id":
                    query = sortOrder == "asc" ? query.OrderBy(s => s.CustomerId) : query.OrderByDescending(s => s.CustomerId);
                    break;
            }

            totalCustomersCount = query.Count();

            int skipAmount = (pageNumber - 1) * pageSize;
            query = query.Skip(skipAmount).Take(pageSize);

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
