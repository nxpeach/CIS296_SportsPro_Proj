using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SportsPro.Models;
namespace SportsPro.ViewModels
{
    public class IncidentListViewModel
    {
        public List<Incident> Incidents { get; set; } //List of incidents
        public string Filter { get; set; } //Filter by all, unassigned, or open
    }
}