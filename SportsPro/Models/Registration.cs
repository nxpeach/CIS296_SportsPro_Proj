namespace SportsPro.Models
{
    public class Registration
    {
        public int CustomerID { get; set; }
        public Customer Customer { get; set; } = null!;

        public int ProductID { get; set; }
        public Product Product { get; set; } = null!;
    }
}
