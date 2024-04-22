

using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter a name between 2-50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter a last name between 2-50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter an address between 2-50 characters")]
        public string Streetaddress { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter a city between 2-50 characters")]
        public string City { get; set; }

        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Enter a Zip Code between 2-10 characters")]
        public string Zipcode { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Country { get; set; }

        public string CountryCode { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? Birthday { get; set; }

        public string? NationalId { get; set; }

        public string? Telephonecountrycode { get; set; }

        [DataType(DataType.PhoneNumber)]
        [MinLength(5, ErrorMessage = "Minimum 5 numbers required.")]
        public string? Telephonenumber { get; set; }

        [EmailAddress]
        public string? Emailaddress { get; set; }
    }
}
