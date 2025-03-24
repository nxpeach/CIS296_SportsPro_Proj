using SportsPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using SportsPro.ViewModels;


namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        private SportsProContext context { get; set; }

        public IncidentController(SportsProContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IActionResult Add()  //Now using IncidentAddEditViewModel
        {
            var viewModel = new IncidentAddEditViewModel
            {
                Action = "Add",
                Customers = context.Customers
                    .OrderBy(c => c.LastName)
                    .ThenBy(c => c.FirstName)
                    .ToList(),
                Products = context.Products
                    .OrderBy(p => p.Name)
                    .ToList(),
                Technicians = context.Technicians
                    .OrderBy(t => t.Name)
                    .ToList(),
                Incident = new Incident { DateOpened = null }
            };

            return View("Edit", viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)  //Now using IncidentAddEditViewModel
        {
            var incident = context.Incidents.Find(id);
            if (incident == null)
            {
                return NotFound(); //Return 404 if incident isn't found
            }

            var viewModel = new IncidentAddEditViewModel
            {
                Action = "Edit",
                Customers = context.Customers
                    .OrderBy(c => c.LastName)
                    .ThenBy(c => c.FirstName)
                    .ToList(),
                Products = context.Products
                    .OrderBy(p => p.Name)
                    .ToList(),
                Technicians = context.Technicians
                    .OrderBy(t => t.Name)
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
                    context.Incidents.Add(incident);
                }
                else
                {
                    context.Incidents.Update(incident);
                }
                context.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {  
                //Create view model for invalid state
                var viewModel = new IncidentAddEditViewModel
                {
                    Action = (incident.IncidentID == 0) ? "Add" : "Edit",
                    Customers = context.Customers
                        .OrderBy(c => c.LastName)
                        .ThenBy(c => c.FirstName)
                        .ToList(),
                    Products = context.Products
                        .OrderBy(p => p.Name)
                        .ToList(),
                    Technicians = context.Technicians
                        .OrderBy(t => t.Name)
                        .ToList(),
                    Incident = incident
                };
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var incident = context.Incidents
                           .Include(i => i.Customer)  // Ensure that you are including the Customer data
                           .FirstOrDefault(i => i.IncidentID == id);

           

            return View(incident);
        }

        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            context.Incidents.Remove(incident);
            context.SaveChanges();
            return RedirectToAction("List");
        }

        [Route("/incidents/{filterString?}")]
        public IActionResult List(string filterString = "all")
        {
            //Create object
            var filters = new Utilities.IncidentFilters(filterString);

            //Start with all
            IQueryable<Incident> query = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician);

            //Apply filtering
            query = Utilities.IncidentFilters.ApplyFilter(query, filters);

            //Get final list
            var incidents = query.ToList();

            var viewModel = new IncidentListViewModel
            {
                Incidents = incidents,
                Filter = filterString
            };

            ViewBag.Filters = filters;

            return View(viewModel);
        }
    }
}
