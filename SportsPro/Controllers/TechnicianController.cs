using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using SportsPro.ViewModels;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        private readonly SportsProContext _context;

        public TechnicianController(SportsProContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetTechnician()
        {
            var technicians = _context.Technicians.ToList();
            var viewModel = new GetTechnicianViewModel
            {
                Technicians = technicians,
                SelectedTechnicianID = 0
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ListIncidentsByTechnician(GetTechnicianViewModel viewModel)
        {
            if (viewModel.SelectedTechnicianID == 0)
            {
                ModelState.AddModelError("", "Please select a technician."); //Getting double error messages??
                viewModel.Technicians = _context.Technicians.ToList(); //Repopulate the technicians list
                return View("GetTechnician", viewModel);
            }
            //Fetch the selected technician name
            var technician = _context.Technicians
                .FirstOrDefault(t => t.TechnicianID == viewModel.SelectedTechnicianID);
            if (technician == null)
            {
                ModelState.AddModelError("SelectedTechnicianId", "Technician not found.");
                viewModel.Technicians = _context.Technicians.ToList();
                return View("GetTechnician", viewModel);
            }
            //Fetch open incidents
            var incidents = _context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Where(i => i.TechnicianID == viewModel.SelectedTechnicianID && i.DateClosed == null) //Filter for open incidents
                .ToList();
            return View("IncidentsByTechnician", new IncidentsByTechnicianViewModel
            {
                TechnicianName = technician.Name,
                Incidents = incidents
            });
        }

        public IActionResult SwitchTechnician()
        {
            return RedirectToAction("GetTechnician");
        }
    }
}