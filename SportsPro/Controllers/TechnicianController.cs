using Microsoft.AspNetCore.Mvc;
using SportsPro.Data.Repositories;
using SportsPro.Models;
using SportsPro.ViewModels;
using SportsPro.Data;
using SportsPro.Data.UnitOfWork;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TechnicianController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetTechnician()
        {
            var viewModel = new GetTechnicianViewModel
            {
                Technicians = _unitOfWork.Technicians.List(new QueryOptions<Technician>()).ToList(),
                SelectedTechnicianID = 0
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ListIncidentsByTechnician(GetTechnicianViewModel viewModel)
        {
            if (viewModel.SelectedTechnicianID == 0)
            {
                ModelState.AddModelError("", "Please select a technician.");
                viewModel.Technicians = _unitOfWork.Technicians.List(new QueryOptions<Technician>()).ToList();
                return View("GetTechnician", viewModel);
            }

            var technician = _unitOfWork.Technicians.Get(viewModel.SelectedTechnicianID);
            if (technician == null)
            {
                ModelState.AddModelError("SelectedTechnicianId", "Technician not found.");
                viewModel.Technicians = _unitOfWork.Technicians.List(new QueryOptions<Technician>()).ToList();
                return View("GetTechnician", viewModel);
            }

            var options = new QueryOptions<Incident>
            {
                WhereClauses = {
                    i => i.TechnicianID == viewModel.SelectedTechnicianID,
                    i => i.DateClosed == null
                },
                Includes = {
                    i => i.Customer,
                    i => i.Product
                }
            };

            var incidents = _unitOfWork.Incidents.List(options).ToList();

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
