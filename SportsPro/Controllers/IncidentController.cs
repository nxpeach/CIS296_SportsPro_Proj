using SportsPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        private SportsProContext context { get; set; }

        public IncidentController(SportsProContext ctx)
        {
            context = ctx;
        }

        public IActionResult List()
        {
            var incidents = context.Incidents.Include(i => i.Customer).
                                              Include(i => i.Product)
                                              .ToList();

            return View(incidents);
        }
    }
}
