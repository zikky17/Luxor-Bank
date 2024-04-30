using ServiceLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Interfaces
{
    public interface ICountryService
    {
        public List<string> GetCountries();

        public List<int> GetCustomersPerCountry(List<string> countries);

        public List<Disposition> GetDispositions(string country);

        public List<(string FullName, decimal Balance, int CustomerId)> GetTopTenCustomers(List<Disposition> dispositions);
    }
}
