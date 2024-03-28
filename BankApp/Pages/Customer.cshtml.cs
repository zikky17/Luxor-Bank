using BankApp.Data;
using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ServiceLibrary.Models;
using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection.Emit;

namespace BankApp.Pages
{
    public class CustomerModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public List<CustomerViewModel> Customers { get; set; }
        public List<AccountViewModel> Accounts { get; set; }
        public decimal TotalBalance { get; set; }

        public CustomerModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(int customerId)
        {
            Customers = _context.Customers
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

            Accounts = _context.Dispositions
            .Where(d => d.CustomerId == customerId)
            .Select(d => new AccountViewModel
            {
                AccountId = d.AccountId
            }).ToList();


            TotalBalance = _context.Dispositions
                               .Where(d => d.CustomerId == customerId)
                               .Select(d => d.Account.Balance)
                               .Sum();
        }
    }
}
