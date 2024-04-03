using BankApp.ViewModels;
using ServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Transaction = ServiceLibrary.Models.Transaction;

namespace ServiceLibrary.Interfaces
{
    public interface IAccountService
    {
        List<AccountViewModel> GetAccountInfo(int accountId);

        bool Deposit(Transaction transaction);
    }
}
