using AutoMapper;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace BankApp.Pages.Customer
{
    [BindProperties]
    public class CreateModel(ICustomerService service, IAccountService accountService, IMapper mapper) : PageModel
    {
        private readonly ICustomerService _customerService = service;
        private readonly IAccountService _accountService = accountService;
        private readonly IMapper _mapper = mapper;

        public CustomerViewModel CustomerVM { get; set; }

        public void OnGet()
        {
          
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {

                var customer = new ServiceLibrary.Data.Customer();
                _mapper.Map(CustomerVM, customer);

                _customerService.CreateCustomer(customer);
                var newAccount = new ServiceLibrary.Data.Account
                {
                    Frequency = "AfterTransaction",
                    Created = DateOnly.FromDateTime(DateTime.Now)
                };
                _accountService.CreateAccount(customer.CustomerId, newAccount);
                ViewData["Message"] = "Customer created successfully!";
                TempData["Message"] = ViewData["Message"];
                return RedirectToPage("Index");;
            }

            return Page();
        }

    }
}
