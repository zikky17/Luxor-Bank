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
            _dbContext = context;
        }

        private readonly ApplicationDbContext _dbContext;
        public List<CustomerViewModel> Customers { get; set; }

        public List<CustomerViewModel> GetAllCustomers(string sortColumn, string sortOrder)
        {
            var query = _dbContext.Customers.Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerId,
                FirstName = c.Givenname,
                LastName = c.Surname,
                Country = c.Country,

            });


            if (sortColumn == "FirstName")
                if (sortOrder == "asc")
                    query = query.OrderBy(s => s.FirstName);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(s => s.FirstName);

            if (sortColumn == "LastName")
                if (sortOrder == "asc")
                    query = query.OrderBy(s => s.LastName);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(s => s.LastName);

            if (sortColumn == "Country")
                if (sortOrder == "asc")
                    query = query.OrderBy(s => s.Country);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(s => s.Country);


            Customers = query.ToList();
            return Customers;
        }
    }
}
