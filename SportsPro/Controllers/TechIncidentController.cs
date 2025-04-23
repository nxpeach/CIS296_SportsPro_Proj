using Microsoft.AspNetCore.Mvc;
using SportsPro.Data.Repositories;
using SportsPro.Data.UnitOfWork;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class TechIncidentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TechIncidentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var options = new QueryOptions<Incident>
            {
                Includes = {
                    i => i.Customer,
                    i => i.Product,
                    i => i.Technician
                }
            };
            options.AddWhere(i => i.IncidentID == id);

            var incident = _unitOfWork.Incidents.List(options).FirstOrDefault();


            if (incident == null)
            {
                return RedirectToAction("GetTechnician", "Technician");
            }

            return View(incident);
        }

        [HttpPost]
        public IActionResult Edit(Incident incident)
        {
            var existingIncident = _unitOfWork.Incidents.Get(incident.IncidentID);

            if (existingIncident == null)
            {
                return RedirectToAction("GetTechnician", "Technician");
            }

            existingIncident.Description = incident.Description;
            existingIncident.DateClosed = incident.DateClosed;

            _unitOfWork.Save();

            return RedirectToAction("GetTechnician", "Technician");
        }
    }
}
