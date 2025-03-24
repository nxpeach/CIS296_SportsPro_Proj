using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class TechIncidentController : Controller
    {
        private SportsProContext context { get; set; }

        public TechIncidentController(SportsProContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var incident = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefault(i => i.IncidentID == id);

            if (incident == null)
            {
                return RedirectToAction("GetTechnician", "Technician");
            }

            return View(incident);
        }

        [HttpPost]
        public IActionResult Edit(Incident incident)
        {
            // Get the existing incident to update
            var existingIncident = context.Incidents.Find(incident.IncidentID);

            if (existingIncident == null)
            {
                return RedirectToAction("GetTechnician", "Technician");
            }

            // Update only description and date closed
            existingIncident.Description = incident.Description;
            existingIncident.DateClosed = incident.DateClosed;

            // Save changes
            context.SaveChanges();

            // Go back to Technician selection
            return RedirectToAction("GetTechnician", "Technician");
        }
    }
}