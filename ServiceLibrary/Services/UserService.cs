using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using ServiceLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Services
{
    public class UserService(UserManager<IdentityUser> manager, ApplicationDbContext dbContext) : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager = manager;
        private readonly IdentityDbContext _dbContext = dbContext;

        public string Role { get; set; }

        public void CreateUser(string userName, string password, string[] roles)
        {
            if (_userManager.FindByEmailAsync(userName).Result != null) return;

            var user = new IdentityUser
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true
            };

            _userManager.CreateAsync(user, password).Wait();
            _userManager.AddToRolesAsync(user, roles).Wait();
        }

        public List<UserViewModel> GetUsers()
        {
            var users = (from user in _dbContext.Users
                         join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                         join role in _dbContext.Roles on userRole.RoleId equals role.Id
                         select new UserViewModel
                         {
                             UserId = user.Id,
                             UserName = user.UserName,
                             Role = role.Id == "4db3745d-e74c-410e-bdac-56bf52ac56d6" ? "Admin" : "Cashier"
                         }).ToList();

            return users;
        }

        public List<UserViewModel> GetUser(string userId)
        {
            var user = _dbContext.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserViewModel                 
                {
                    UserId = u.Id,
                    UserName = u.UserName
                }).ToList();      

            return user;
        }

        public void UpdateUser(IdentityUser user)
        {
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }


    }
}
