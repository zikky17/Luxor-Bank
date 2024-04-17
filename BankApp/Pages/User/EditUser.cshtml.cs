using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using ServiceLibrary.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.User
{
    [BindProperties]
    public class EditUserModel(IUserService service) : PageModel
    {
        private readonly IUserService _userService = service;

        public string UserName { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "You must choose a role.")]
        public string[] Role { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*()_+])[A-Za-z\d!@#$%^&*()_+]{8,}$", ErrorMessage = "Password must be at least 8 characters long, start with an uppercase letter, and contain at least one special character")]
        public string Password { get; set; }


        public void OnGet(string userId)
        {
            var user = _userService.GetUser(userId).First();
            UserName = user.UserName;
            UserId = userId;
        }

        public IActionResult OnPost(string userId)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUser(userId).First();
                {
                    user.UserName = UserName;
                }

                var updatedUser = new IdentityUser
                {
                    UserName = user.UserName
                };

                _userService.UpdateUser(updatedUser);

                ViewData["Message"] = "User updated successfully!";
                return Page();
            }
            return Page();
        }


    }
}




