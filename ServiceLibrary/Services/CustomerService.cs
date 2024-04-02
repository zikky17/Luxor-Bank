using BankApp.ViewModels;
using BankWeb.Data;
using ServiceLibrary.Interfaces;

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

        public List<CustomerViewModel> GetAllCustomers(string sortColumn, string sortOrder, int pageNumber, string q)
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

            var firstItemIndex = (pageNumber - 1) * 50;

            query = query.Skip(firstItemIndex).Take(50);

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
