using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;
using ServiceLibrary.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.User
{
    [BindProperties]
    public class CreateUserModel(IUserService service) : PageModel
    {

        private readonly IUserService _userService = service;

        public List<UserViewModel> Users { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*()_+])[A-Za-z\d!@#$%^&*()_+]{8,}$", ErrorMessage = "Password must be at least 8 characters long, start with an uppercase letter, and contain at least one special character")]

        public string Password { get; set; }

        public bool ConfirmPassword { get; set; } = true;

        public string[] Role { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {

                var userName = UserName;
                var password = Password;
                var role = Role;

                _userService.CreateUser(userName, password, role);

                ViewData["Message"] = "User created successfully!";
                return Page();
            }
            return Page();
        }
    }
}
