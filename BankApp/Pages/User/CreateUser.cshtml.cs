using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.ViewModels;

namespace BankApp.Pages.User
{
    public class CreateUserModel : PageModel
    {

        public List<UserViewModel> Users { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool ConfirmPassword { get; set; } = true;

        public string[] Role { get; set; }

        public void OnGet()
        {
            
        }
    }
}
