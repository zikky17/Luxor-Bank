using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankApp.Pages
{
    public class TestModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        public List<CustomerViewModel> Customers { get; set; }

        public TestModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet(string sortColumn, string sortOrder)
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

        }
    }
}
