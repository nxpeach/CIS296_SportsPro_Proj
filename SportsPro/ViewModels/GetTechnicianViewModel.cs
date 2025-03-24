using SportsPro.Models;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.ViewModels
{
    public class GetTechnicianViewModel
    {
        public List<Technician> Technicians { get; set; } = new List<Technician>();

        [Required(ErrorMessage = "Please select a technician.")]
        public int SelectedTechnicianID { get; set; }
    }
}