using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Interfaces;
using ServiceLibrary.ViewModels;

namespace BankApp.Pages.User
{
    public class IndexModel(IUserService service) : PageModel
    {
        private readonly IUserService _userService = service;

        public List<UserViewModel> Users { get; set; }

        public void OnGet()
        {
            Users = _userService.GetUsers();
        }
    }
}
