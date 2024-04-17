using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.ViewModels
{
    public class TransactionViewModel
    {
        public int AccountId { get; set; }

        public DateOnly Date { get; set; }

        public string Operation { get; set; }

        public string Type { get; set; }

        public decimal Amount { get; set; }
    }
}
