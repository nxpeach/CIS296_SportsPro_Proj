using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using SportsPro.ViewModels;

namespace SportsPro.Controllers
{
    public class RegistrationController : Controller
    {
        //DbContext needed to be injected into the controller
        private readonly SportsProContext _context;

        public RegistrationController(SportsProContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Get()
        {
            ViewBag.Customers = _context.Customers.OrderBy(c => c.LastName).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Get(int customerID)
        {
            if (customerID == 0)
            {
                TempData["ErrorMessage"] = "Please select a customer.";
                return RedirectToAction("Get");
            }

            HttpContext.Session.SetInt32("CustomerID", customerID);
            return RedirectToAction("Registrations");
        }

        [HttpGet]
        public IActionResult Registrations()
        {
            var customerID = HttpContext.Session.GetInt32("CustomerID");
            if (customerID == null) return RedirectToAction("Get");

            var customer = _context.Customers.Find(customerID);
            if (customer == null) return RedirectToAction("Get");

            var viewModel = new RegistrationsViewModel
            {
                Customer = customer,
                Products = _context.Products.OrderBy(p => p.Name).ToList(),
                CustomerProducts = _context.Registrations
                    .Where(r => r.CustomerID == customerID)
                    .Select(r => r.Product)
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Register(int productID)
        {
            var customerID = HttpContext.Session.GetInt32("CustomerID");
            if (customerID == null) return RedirectToAction("Get");

            if (!_context.Registrations.Any(r => r.CustomerID == customerID && r.ProductID == productID))
            {
                _context.Registrations.Add(new Registration
                {
                    CustomerID = customerID.Value,
                    ProductID = productID
                });
                _context.SaveChanges();
            }

            return RedirectToAction("Registrations");
        }

        [HttpPost]
        public IActionResult Delete(int productID)
        {
            var customerID = HttpContext.Session.GetInt32("CustomerID");
            if (customerID == null) return RedirectToAction("Get");

            var registration = _context.Registrations
                .FirstOrDefault(r => r.CustomerID == customerID && r.ProductID == productID);

            if (registration != null)
            {
                _context.Registrations.Remove(registration);
                _context.SaveChanges();
            }

            return RedirectToAction("Registrations");
        }
    }
}
