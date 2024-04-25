using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Services
{
    public class DatabaseService : IDatabaseService
    {
        public static ApplicationDbContext GetDbContext()
        {
            return new ApplicationDbContext();
        }
    }
}
