﻿using BankWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Services
{
    public class DataInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public DataInitializer(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public void SeedData()
        {
            _dbContext.Database.Migrate();
            SeedRoles();
            SeedUsers();
        }

        private void SeedUsers()
        {
            AddUserIfNotExists("richard.chalk@systementor.se", "Hejsan123#", new string[] { "Admin" });
            AddUserIfNotExists("richard.chalk@customer.systementor.se", "Hejsan123#", new string[] { "Cashier" });
        }

        private void SeedRoles()
        {
            AddRoleIfNotExisting("Admin");
            AddRoleIfNotExisting("Cashier");
        }

        private void AddRoleIfNotExisting(string roleName)
        {
            var role = _dbContext.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
            {
                _dbContext.Roles.Add(new IdentityRole { Name = roleName, NormalizedName = roleName });
                _dbContext.SaveChanges();
            }
        }

        private void AddUserIfNotExists(string userName, string password, string[] roles)
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
    }
    }
