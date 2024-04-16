using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using ServiceLibrary.ViewModels;

namespace BankApp.Pages.User
{
    [BindProperties]
    public class EditUserModel(IUserService service) : PageModel
    {
        private readonly IUserService _userService = service;

        public string UserName { get; set; }

        public string UserId { get; set; }

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




