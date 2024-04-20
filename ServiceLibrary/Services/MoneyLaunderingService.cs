using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;

namespace ServiceLibrary.Services
{
    public class MoneyLaunderingService : IMoneyLaunderingService
    {

        public static ApplicationDbContext GetDbContext()
        {
            return new ApplicationDbContext();
        }


    }
}
