
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;

namespace MoneyLaunderingGuard
{
    public class App()
    {
        private readonly IAccountService _accountService;
        private readonly IMoneyLaunderingService _moneyLaunderingService;

        public List<Transaction> SuspiciousSingleTransactions { get; set; }

        public List<Account> Accounts { get; set; }
        public List<Customer> CustomersSweden { get; set; }
        public List<Customer> CustomersNorway { get; set; }
        public List<Customer> CustomersFinland { get; set; }
        public List<Customer> CustomersDenmark { get; set; }

        public List<Transaction> TransactionsSweden { get; set; }
        public List<Transaction> TransactionsNorway { get; set; }
        public List<Transaction> TransactionsFinland { get; set; }
        public List<Transaction> TransactionsDenmark { get; set; }

        public List<Transaction> SuspiciousTransactionsSweden { get; set; }
        public List<Transaction> SuspiciousTransactionsNorway { get; set; }
        public List<Transaction> SuspiciousTransactionsFinland { get; set; }
        public List<Transaction> SuspiciousTransactionsDenmark { get; set; }


        public void Run()
        {
            var maxSingleTransaction = 15000;
            SuspiciousSingleTransactions = _moneyLaunderingService.GetSuspiciousSingleTransaction(maxSingleTransaction);
            
            CustomersSweden = _moneyLaunderingService.GetCustomersSortedByCountry("Sweden");
            CustomersNorway = _moneyLaunderingService.GetCustomersSortedByCountry("Norway");
            CustomersFinland = _moneyLaunderingService.GetCustomersSortedByCountry("Finland");
            CustomersDenmark = _moneyLaunderingService.GetCustomersSortedByCountry("Denmark");

            TransactionsSweden = _moneyLaunderingService.GetTransactionsByCountry(CustomersSweden);
            TransactionsNorway = _moneyLaunderingService.GetTransactionsByCountry(CustomersNorway);
            TransactionsFinland = _moneyLaunderingService.GetTransactionsByCountry(CustomersFinland);
            TransactionsDenmark = _moneyLaunderingService.GetTransactionsByCountry(CustomersDenmark);

            SuspiciousTransactionsSweden = _moneyLaunderingService.GetSuspiciousTransactionsThreeLastDays(TransactionsSweden);
            SuspiciousTransactionsDenmark = _moneyLaunderingService.GetSuspiciousTransactionsThreeLastDays(TransactionsDenmark);
            SuspiciousTransactionsNorway = _moneyLaunderingService.GetSuspiciousTransactionsThreeLastDays(TransactionsNorway);
            SuspiciousTransactionsFinland = _moneyLaunderingService.GetSuspiciousTransactionsThreeLastDays(TransactionsFinland);

        }
    }
}