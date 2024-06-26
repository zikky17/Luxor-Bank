﻿using Microsoft.AspNetCore.Identity;
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
        public bool CreateUser(string userName, string password, string[] roles);

        public List<UserViewModel> GetUsers();

        public List<UserViewModel> GetUserVM(string userId);

        public IdentityUser GetUser(string userId);

        public void UpdateUser(string userId, string userName, string[] roles);

        public void DeleteUser(string userId);
    }
}
