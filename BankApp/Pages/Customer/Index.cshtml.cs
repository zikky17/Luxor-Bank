using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace BankApp.Pages.Customer
{
    public class CustomersModel(ICustomerService customerService) : PageModel
    {
        private readonly ICustomerService _customerService = customerService;

        public List<CustomerViewModel> Customers { get; set; }

        public int CustomerId { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int AmountOfCustomers { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string Q { get; set; }
        [BindProperty]
        public int PageSize { get; set; }



        public void OnGet(string sortColumn, string sortOrder, int pageNumber, string q, int pageSize)
        {
            if (pageSize > 0)
            {
                PageSize = pageSize;

            }
            if (PageSize == 0)
            {
                PageSize = 50;
            }


            Q = q;
            SortColumn = sortColumn;
            SortOrder = sortOrder;
            CurrentPage = pageNumber;

            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }

            Customers = _customerService.GetAllCustomersSorted(sortColumn, sortOrder, PageSize, CurrentPage, q, out int totalCustomersCount);
            AmountOfCustomers = _customerService.GetNumberOfCustomers();
            TotalPages = totalCustomersCount == 0 ? 1 : (int)Math.Ceiling((double)totalCustomersCount / PageSize);
        }

    }
}
