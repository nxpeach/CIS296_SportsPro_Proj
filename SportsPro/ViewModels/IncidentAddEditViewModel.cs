using SportsPro.Models;
using System.Collections.Generic;
namespace SportsPro.ViewModels
{
    public class IncidentAddEditViewModel
    {
        public Incident Incident { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Product> Products { get; set; }
        public List<Technician> Technicians { get; set; }
        public string Action { get; set; }  //"Add" or "Edit"
    }
}