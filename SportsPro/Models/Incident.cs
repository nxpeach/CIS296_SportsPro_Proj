using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Incident
    {
		public int IncidentID { get; set; }

        [Required(ErrorMessage = "Please enter a title.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the date opened.")]
        public DateTime? DateOpened { get; set; } = DateTime.Now;
        public DateTime? DateClosed { get; set; } = null;

        [Required(ErrorMessage = "Please select a customer.")]
        public int? CustomerID { get; set; }                   // foreign key property
		public Customer? Customer { get; set; } = null!;       // navigation property

        [Required(ErrorMessage = "Please select a product.")]
        public int? ProductID { get; set; }                    // foreign key property
		public Product? Product { get; set; } = null!;         // navigation property

        [Required(ErrorMessage = "Please select a technician.")]
        public int? TechnicianID { get; set; }                 // foreign key property 
		public Technician? Technician { get; set; } = null!;   // navigation property

		
	}
}