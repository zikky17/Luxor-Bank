using BankApp.ViewModels;
using BankWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Models;
using System;
using System.Collections.Generic;

namespace BankApp.Pages.Customer
{
    public class CustomersModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public CustomersModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public List<CustomerViewModel> Customers { get; set; }

        public int CustomerId { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string Q { get; set; }
        public int PageSize { get; set; } = 50;

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

            Customers = _customerService.GetAllCustomers(sortColumn, sortOrder, PageSize, CurrentPage, q, out int totalCustomersCount);

            TotalPages = totalCustomersCount == 0 ? 1 : (int)Math.Ceiling((double)totalCustomersCount / PageSize);
        }
    }
}
