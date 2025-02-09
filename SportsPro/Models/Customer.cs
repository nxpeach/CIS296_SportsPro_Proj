using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Customer
    {
		public int CustomerID { get; set; }

		[Required(ErrorMessage = "A first name is required.")]
		public string FirstName { get; set; } = string.Empty;

		[Required(ErrorMessage = "A last name is required.")]
		public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "An address is required.")]
		public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "A city is required.")]
		public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "A state is required.")]
		public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "A postal code is required.")]
		public string PostalCode { get; set; } = string.Empty;
		public string? Phone { get; set; } 
        public string? Email { get; set; }

        [Required(ErrorMessage = "A country is required.")]
        public string CountryID { get; set; } = string.Empty;
        public Country? Country { get; set; }

        public string FullName => FirstName + " " + LastName;   // read-only property
	}
}
