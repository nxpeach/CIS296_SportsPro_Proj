using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "A first name is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First name must be 1-50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "A last name is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Last name must be 1-50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "An address is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Address must be 1-50 characters.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "A city is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "City must be 1-50 characters.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "A state is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "State must be 1-50 characters.")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "A postal code is required.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Postal code must be 1-20 characters.")]
        public string PostalCode { get; set; } = string.Empty;

        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$",
            ErrorMessage = "Phone must be in (999) 999-9999 format.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "An email is required.")]
        [DataType(DataType.EmailAddress)]   //Especially for email validation
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Email must be 1-50 characters.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "A country is required.")]
        public string CountryID { get; set; } = string.Empty;
        public Country? Country { get; set; }

        public string FullName => FirstName + " " + LastName;
    }
}
