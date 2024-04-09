using BankWeb.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;

namespace BankWeb.Pages
{
    public class IndexModel : PageModel
    {

        private readonly ICustomerService _customerService;

        public IndexModel(ICustomerService service)
        {
           _customerService = service;
        }

        public Dictionary<string, int> CustomersPerCountry { get; set; }
        public Dictionary<string, int> AccountsPerCountry { get; set; }
        public Dictionary<string, decimal> BalancePerCountry { get; set; }


        public void OnGet()
        {
            CustomersPerCountry = _customerService.GetCustomersPerCountry();

            AccountsPerCountry = _customerService.GetAccountsPerCountry();

            BalancePerCountry = _customerService.GetBalancePerCountry();

            Response.Headers[HeaderNames.CacheControl] = "public,max-age=60";
            Response.Headers[HeaderNames.Vary] = "Accept-Encoding,User-Agent,Country";
        }


    }
}

