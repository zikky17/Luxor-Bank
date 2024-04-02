using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;
using System.Drawing.Printing;

namespace BankApp.Pages
{
    public class CustomersModel : PageModel
    {
        public CustomersModel(ICustomerService service)
        {
            _customerService = service;
        }

        public ICustomerService _customerService { get; set; }
        public List<CustomerViewModel> Customers { get; set; }

        public int CustomerId { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string Q { get; set; }


        public void OnGet(int customerId, string sortColumn, string sortOrder, int pageNumber, string q)
        {
            Q = q;
            SortColumn = sortColumn;
            SortOrder = sortOrder;



            if (pageNumber == 0)
            {
                pageNumber = 1;
                CurrentPage = pageNumber;

            }

            CurrentPage = pageNumber;
            Customers = _customerService.GetAllCustomers(sortColumn, sortOrder, pageNumber, q);

            var totalCustomers = _customerService.GetTotalCustomersCount();
            TotalPages = (int)Math.Ceiling((double)totalCustomers / 50);
        }
    }
}
