using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;


namespace BankWeb.Pages
{
   
    public class IndexModel(ICustomerService service, ICountryService countryService) : PageModel
    {

        private readonly ICustomerService _customerService = service;
        private readonly ICountryService _countryService = countryService;

        public List<string> Countries { get; set; }
        public List<int> CustomersByCountry { get; set; }
        public List<decimal> BalancePerCountry { get; set; }
        public List<int> AccountsPerCountry { get; set; }

        public void OnGet()
        {
            Countries = _countryService.GetCountries();
            CustomersByCountry = _countryService.GetCustomersPerCountry(Countries);

            AccountsPerCountry = _customerService.GetAccountsPerCountry(Countries);

            BalancePerCountry = _customerService.GetBalancePerCountry(Countries);

        }


    }
}

