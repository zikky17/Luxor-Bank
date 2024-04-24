using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;
using ServiceLibrary.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace BankApp.Pages.User
{
    [BindProperties]
    public class CreateUserModel(IUserService service) : PageModel
    {

        private readonly IUserService _userService = service;

        public List<UserViewModel> Users { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*()_+])[A-Za-z\d!@#$%^&*()_+]{8,}$", ErrorMessage = "Password must be at least 8 characters long, start with an uppercase letter, and include at least one special character.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "You must choose a role.")]
        public string[] Role { get; set; }

        public bool CreateStatus { get; set; }

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

                var status = _userService.CreateUser(userName, password, role);

                    switch (status)
                {
                    case false:
                        ModelState.AddModelError("CreateStatus", "User with this email already exists.");
                        return Page();
                    case true:
                        ViewData["Message"] = "User created successfully!";
                        TempData["Message"] = ViewData["Message"];
                        return RedirectToPage("Index");
                }      
            }
            return Page();
        }
    }
}
