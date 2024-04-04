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

    public enum StatusMessage
    {
        Approved,
        TooLowBalance,
        IncorrectAmount,
        MessageRequired
    }

    public interface IAccountService
    {
        List<AccountViewModel> GetAccountInfo(int accountId);

        StatusMessage Deposit(decimal amount, int accountId, string comment);

        StatusMessage Withdraw(Transaction transaction, int accountId);
    }
}
