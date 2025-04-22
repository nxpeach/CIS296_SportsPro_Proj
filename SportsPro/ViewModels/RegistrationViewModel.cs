using SportsPro.Models;

namespace SportsPro.ViewModels
{
    public class RegistrationsViewModel
    {
        public Customer Customer { get; set; } = null!;
        public List<Product> Products { get; set; } = new();
        public List<Product> CustomerProducts { get; set; } = new();
    }
}
