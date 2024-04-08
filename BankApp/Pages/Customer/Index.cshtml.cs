using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace BankApp.Pages.Customer
{
    public class CustomersModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public CustomersModel(ICustomerService customerService)
        {
            _customerService = customerService;
            PageSize = 50;
        }

        public List<CustomerViewModel> Customers { get; set; }

        public int CustomerId { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string Q { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; }



        public void OnGet(string sortColumn, string sortOrder, int pageNumber, string q)
        {

            Q = q;
            SortColumn = sortColumn;
            SortOrder = sortOrder;
            CurrentPage = pageNumber;

            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }

            if (PageSize < 1)
            {
                PageSize = 50;
            }

            Customers = _customerService.GetAllCustomersSorted(sortColumn, sortOrder, PageSize, CurrentPage, q, out int totalCustomersCount);

            TotalPages = totalCustomersCount == 0 ? 1 : (int)Math.Ceiling((double)totalCustomersCount / PageSize);
        }

    }
}
