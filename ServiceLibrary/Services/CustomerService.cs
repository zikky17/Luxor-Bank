using BankApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;


namespace ServiceLibrary.Services
{
    public class CustomerService(ApplicationDbContext context) : ICustomerService
    {
        private readonly ApplicationDbContext _context = context;

        public int GetCustomersPerCountry(string country)
        {
            var query = _context.Customers
                .Where(c => c.Country == country)
                .Count();
           
            return query;
        }

        public List<int> GetAccountsPerCountry(List<string> countries)
        {
            var accountsPerCountry = new List<int>();

            foreach (var country in countries)
            {
                var accountCount = _context.Dispositions
                    .Include(d => d.Account)
                    .Where(d => d.Account.Balance > 0 && d.Customer.Country == country)
                    .Select(d => d.AccountId)
                    .Distinct()
                    .Count();

                accountsPerCountry.Add(accountCount);
            }

            return accountsPerCountry;
        }

        public List<decimal> GetBalancePerCountry(List<string> countries)
        {
            var balancePerCountry = new List<decimal>();

            foreach (var country in countries)
            {
                var totalBalance = _context.Dispositions
                    .Include(d => d.Account)
                    .Where(d => d.Account.Balance > 0 && d.Customer.Country == country)
                    .Sum(d => d.Account.Balance);

                balancePerCountry.Add(totalBalance);
            }

            return balancePerCountry;
        }

        public int GetNumberOfCustomers()
        {
           var customers = _context.Customers.Count();
           return customers;
        }

        public List<CustomerViewModel> GetAllCustomersSorted(string sortColumn, string sortOrder, int pageSize, int pageNumber, string q, out int totalCustomersCount)
        {
            var query = _context.Customers.Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerId,
                NationalId = c.NationalId,
                FirstName = c.Givenname,
                LastName = c.Surname,
                Streetaddress = c.Streetaddress,
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
                    query = sortOrder == "asc" ? query.OrderBy(s => s.Streetaddress) : query.OrderByDescending(s => s.Streetaddress);
                    break;
            }

            totalCustomersCount = query.Count();

            int skipAmount = (pageNumber - 1) * pageSize;
            query = query.Skip(skipAmount).Take(pageSize);

            return query.ToList();
        }

        public List<CustomerViewModel> GetAllCustomers()
        {
            var query = _context.Customers.Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerId,
                FirstName = c.Givenname,
                LastName = c.Surname,
            });
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
                    Streetaddress = c.Streetaddress,
                    City = c.City,
                    Zipcode = c.Zipcode,
                    Country = c.Country,
                    CountryCode = c.CountryCode,
                    Birthday = c.Birthday,
                    Telephonecountrycode = c.Telephonecountrycode,
                    Telephonenumber = c.Telephonenumber,
                    Emailaddress = c.Emailaddress
                }).ToList();

            return customer;
        }

        public Customer GetCustomerById(int customerId)
        {
            var customer = _context.Customers.Where(c => c.CustomerId == customerId).First();
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

        public decimal GetTotalBalance(int customerId)
        {
            var totalBalance = _context.Dispositions
                .Where(d => d.CustomerId == customerId)
                .Select(d => d.Account.Balance)
                .Sum();
            return totalBalance;
        }

        public decimal GetBalance(int accountId)
        {

            var totalBalance = _context.Accounts
                .Where(a => a.AccountId == accountId)
                .Select(a => a.Balance)
                .Sum();
            return totalBalance;
        }

        public void CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

        }

        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        public void DeleteCustomer(int customerId)
        {
            var customerToDelete = _context.Customers.Where(c => c.CustomerId == customerId).First();

            _context.Remove(customerToDelete);
            _context.SaveChanges();
        }

    }
}
