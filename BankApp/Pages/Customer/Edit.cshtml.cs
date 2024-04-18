using AutoMapper;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.Customer
{
    [BindProperties]
    public class EditModel(ICustomerService service, IAccountService accountService, IMapper mapper) : PageModel
    {
        private readonly IAccountService _accountService = accountService;
        private readonly ICustomerService _customerService = service;
        private readonly IMapper _mapper = mapper;

        public List<AccountViewModel> Accounts { get; set; }

        public CustomerViewModel CustomerVM { get; set; }

        public List<int> AccountIds { get; set; }

        public int CustomerId { get; set; }

        public void OnGet(int customerId)
        {

            CustomerId = customerId;
            CustomerVM = _customerService.GetCustomerDetails(customerId).First();
        }

        public IActionResult OnPost(int customerId)
        {
            if (ModelState.IsValid)
            {
                var customer = _customerService.GetCustomerById(customerId);
                _mapper.Map(CustomerVM, customer);

                customer.CustomerId = customerId;

                _customerService.UpdateCustomer(customer);
                ViewData["Message"] = "Customer updated successfully!";
                return Page();
            }
            return Page();
        }

        public IActionResult OnPostDelete(int customerId)
        {
            Accounts = _customerService.GetAccountInfo(customerId);
            foreach (var account in Accounts)
            {
                AccountIds.Add(account.AccountId);
            }
            _accountService.DeleteAllAccounts(AccountIds);
            _customerService.DeleteCustomer(customerId);
            ViewData["Message"] = "Customer deleted successfully!";
            return RedirectToPage("Index");
        }

    }
}
