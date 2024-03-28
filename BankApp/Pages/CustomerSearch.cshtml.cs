using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Models;

namespace BankApp.Pages
{
    public class CustomerSearchModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<CustomerViewModel> Customers { get; set; }
        public int CustomerId { get; set; }

        public CustomerSearchModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Customers = _context.Customers
              .Select(c => new CustomerViewModel
              {
                  CustomerId = c.CustomerId,
              }).ToList();

            foreach (var customer in Customers)
            {
                CustomerId = customer.CustomerId;
            }
        }
    }
}
