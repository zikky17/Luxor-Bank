﻿using BankApp.ViewModels;
using ServiceLibrary.Data;
using ServiceLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Transaction = ServiceLibrary.Data.Transaction;

namespace ServiceLibrary.Interfaces
{

    public enum StatusMessage
    {
        None,
        Approved,
        TooLowBalance,
        IncorrectAmount,
        MessageRequired,
        CantFindAccount
    }

    public interface IAccountService
    {
        AccountViewModel GetAccountInfo(int accountId);

        public List<TransactionViewModel> GetTransactions(int accountId);

        StatusMessage Deposit(decimal amount, int accountId, string comment);

        StatusMessage Withdraw(Transaction transaction);

        public void CreateAccount(int customerId, Account newAccount);

        public void CreateDisposition(int customerId, int accountId);

        public void DeleteAccount(int accountId);

        public void DeleteAllAccounts(List<int> accounts);

    }
}
