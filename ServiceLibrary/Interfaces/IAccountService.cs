using BankApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Interfaces
{
    public interface IAccountService
    {
        List<AccountViewModel> GetAccountInfo(int accountId);
    }
}
