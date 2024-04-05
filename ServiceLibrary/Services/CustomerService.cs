using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                NationalId = c.NationalId,
                FirstName = c.Givenname,
                LastName = c.Surname,
                Address = c.Streetaddress,
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
                case "NationalId":
                    query = sortOrder == "asc" ? query.OrderBy(s => s.NationalId) : query.OrderByDescending(s => s.NationalId);
                    break;
                case "Address":
                    query = sortOrder == "asc" ? query.OrderBy(s => s.Address) : query.OrderByDescending(s => s.Address);
                    break;
            }

            totalCustomersCount = query.Count();

            int skipAmount = (pageNumber - 1) * pageSize;
            query = query.Skip(skipAmount).Take(pageSize);

            return query.ToList();
        }

        public List<CustomerViewModel> GetCustomerDetails(int customerId)
        {
            var customer = _context.Customers
                .Where(c => c.CustomerId == customerId)
                .Select(c => new CustomerViewModel
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.Givenname,
                    LastName = c.Surname,
                    Gender = c.Gender,
                    Address = c.Streetaddress,
                    City = c.City,
                    ZipCode = c.Zipcode,
                    Country = c.Country,
                    CountryCode = c.CountryCode,
                    Birthday = c.Birthday,
                    Telephonecountrycode = c.Telephonecountrycode,
                    Telephonenumber = c.Telephonenumber,
                    Email = c.Emailaddress
                }).ToList();

            return customer;
        }

        public List<AccountViewModel> GetAccountInfo(int customerId)
        {
            var accounts = _context.Dispositions
              .Where(d => d.CustomerId == customerId)
              .Select(d => new AccountViewModel
              {
                  AccountId = d.AccountId
              }).ToList();
            return accounts;
        }

        public decimal GetBalance(int customerId)
        {

            var totalBalance = _context.Dispositions
                .Where(d => d.CustomerId == customerId)
                .Select(d => d.Account.Balance)
                .Sum();
            return totalBalance;
        }

        public void CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            CreateAccount(customer.CustomerId);
        }

        public void CreateAccount(int customerId)
        {
            var newAccount = new Account
            {
                Frequency = "Monthly",
                Created = DateOnly.FromDateTime(DateTime.Now)
            };
            _context.Accounts.Add(newAccount);
            _context.SaveChanges();
            CreateDisposition(customerId, newAccount.AccountId);
        }

        public void CreateDisposition(int customerId, int accountId)
        {
            var newDisposition = new Disposition
            {
                CustomerId = customerId,
                AccountId = accountId,
                Type = "Owner"
            };

            _context.Dispositions.Add(newDisposition);
            _context.SaveChanges();
        }
    }
}
