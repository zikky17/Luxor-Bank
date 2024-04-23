using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using ServiceLibrary.Data;
using ServiceLibrary.Interfaces;
using ServiceLibrary.Services;
using ServiceLibrary.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.User
{
    [BindProperties]
    public class EditUserModel(IUserService service, IMapper mapper) : PageModel
    {
        private readonly IUserService _userService = service;
        private readonly IMapper _mapper = mapper;

        public string UserName { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "You must choose a role.")]
        public string[] Role { get; set; }

        public void OnGet(string userId)
        {
            var user = _userService.GetUserVM(userId).First();
            UserName = user.UserName;
            UserId = userId;
        }

        public IActionResult OnPost(string userId)
        {
            if (ModelState.IsValid)
            {

                var roles = Role;
                _userService.UpdateUser(userId, UserName, roles);

                ViewData["Message"] = "User updated successfully!";
                TempData["Message"] = ViewData["Message"];
                return RedirectToPage("Index"); ;
            }
            return Page();
        }

        public IActionResult OnPostDelete(string userId)
        {
            _userService.DeleteUser(userId);
            ViewData["Message"] = "User deleted successfully!";
            TempData["Message"] = ViewData["Message"];
            return RedirectToPage("Index");
        }

    }
}




