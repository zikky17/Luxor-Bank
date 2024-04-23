using BankApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;

namespace ServiceLibrary.Services
{
    public class MoneyLaunderingService(ApplicationDbContext context) : IMoneyLaunderingService
    {
        private readonly ApplicationDbContext _context = context;

        public static ApplicationDbContext GetDbContext()
        {
            return new ApplicationDbContext();
        }

       

    }
}
