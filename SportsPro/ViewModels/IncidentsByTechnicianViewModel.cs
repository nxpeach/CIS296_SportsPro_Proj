using SportsPro.Models;

namespace SportsPro.ViewModels
{
    public class IncidentsByTechnicianViewModel
    {
        public string TechnicianName { get; set; }
        public List<Incident> Incidents { get; set; } = new List<Incident>();
    }
}