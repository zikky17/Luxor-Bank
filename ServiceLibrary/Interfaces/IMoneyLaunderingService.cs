using ServiceLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Interfaces
{
    public interface IMoneyLaunderingService
    {
        public static ApplicationDbContext GetDbContext()
        {
            return new ApplicationDbContext();
        }


    }
}
