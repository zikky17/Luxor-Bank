using Microsoft.AspNetCore.Identity;
using ServiceLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Interfaces
{
    public interface IUserService
    {
        public void CreateUser(string userName, string password, string[] roles);

        public List<UserViewModel> GetUsers();

        public List<UserViewModel> GetUser(string userId);

        public void UpdateUser(IdentityUser user);
    }
}
