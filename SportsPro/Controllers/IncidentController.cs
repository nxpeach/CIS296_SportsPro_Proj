using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Data.UnitOfWork;
using SportsPro.Models;
using SportsPro.ViewModels;
using SportsPro.Utilities;
using System.Linq;
using SportsPro.Data.Repositories;

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public IncidentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Add()
        {
            var viewModel = new IncidentAddEditViewModel
            {
                Action = "Add",
                Customers = _unitOfWork.Customers
                    .List(new QueryOptions<Customer> { OrderBy = c => c.LastName, ThenOrderBy = c => c.FirstName })
                    .ToList(),
                Products = _unitOfWork.Products
                    .List(new QueryOptions<Product> { OrderBy = p => p.Name })
                    .ToList(),
                Technicians = _unitOfWork.Technicians
                    .List(new QueryOptions<Technician> { OrderBy = t => t.Name })
                    .ToList(),
                Incident = new Incident { DateOpened = null }
            };

            return View("Edit", viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var incident = _unitOfWork.Incidents.Get(id);
            if (incident == null)
                return NotFound();

            var viewModel = new IncidentAddEditViewModel
            {
                Action = "Edit",
                Customers = _unitOfWork.Customers
                    .List(new QueryOptions<Customer> { OrderBy = c => c.LastName, ThenOrderBy = c => c.FirstName })
                    .ToList(),
                Products = _unitOfWork.Products
                    .List(new QueryOptions<Product> { OrderBy = p => p.Name })
                    .ToList(),
                Technicians = _unitOfWork.Technicians
                    .List(new QueryOptions<Technician> { OrderBy = t => t.Name })
                    .ToList(),
                Incident = incident
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(Incident incident)
        {
            if (ModelState.IsValid)
            {
                if (incident.IncidentID == 0)
                {
                    _unitOfWork.Incidents.Insert(incident);
                }
                else
                {
                    _unitOfWork.Incidents.Update(incident);
                }
                _unitOfWork.Save();
                return RedirectToAction("List");
            }

            var viewModel = new IncidentAddEditViewModel
            {
                Action = (incident.IncidentID == 0) ? "Add" : "Edit",
                Customers = _unitOfWork.Customers
                    .List(new QueryOptions<Customer> { OrderBy = c => c.LastName, ThenOrderBy = c => c.FirstName })
                    .ToList(),
                Products = _unitOfWork.Products
                    .List(new QueryOptions<Product> { OrderBy = p => p.Name })
                    .ToList(),
                Technicians = _unitOfWork.Technicians
                    .List(new QueryOptions<Technician> { OrderBy = t => t.Name })
                    .ToList(),
                Incident = incident
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var options = new QueryOptions<Incident>();
            options.AddWhere(i => i.IncidentID == id);
            options.AddInclude(i => i.Customer);

            var incident = _unitOfWork.Incidents.List(options).FirstOrDefault();
            return View(incident);
        }

        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            _unitOfWork.Incidents.Delete(incident);
            _unitOfWork.Save();
            return RedirectToAction("List");
        }

        [Route("/incidents/{filterString?}")]
        public IActionResult List(string filterString = "all")
        {
            var filters = new IncidentFilters(filterString);

            var options = new QueryOptions<Incident>();
            options.AddInclude(i => i.Customer);
            options.AddInclude(i => i.Product);
            options.AddInclude(i => i.Technician);

            var query = _unitOfWork.Incidents.List(options);
            query = IncidentFilters.ApplyFilter(query, filters);

            var viewModel = new IncidentListViewModel
            {
                Incidents = query.ToList(),
                Filter = filterString
            };

            ViewBag.Filters = filters;
            return View(viewModel);
        }
    }
}
