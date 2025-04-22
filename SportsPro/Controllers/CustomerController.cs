using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using System.Linq;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {
        private readonly SportsProContext _context;

        public CustomerController(SportsProContext context)
        {
            _context = context;
        }

        //GET Customer/List
        [Route("/customers")]
        public IActionResult List()
        {
            var customers = _context.Customers.ToList();
            return View(customers);
        }

        //GET Customer/Add
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            ViewBag.Countries = _context.Countries.ToList(); //Get countries for dropdown
            return View("AddEdit", new Customer());
        }

        //POST Customer/Add
        [HttpPost]
        public IActionResult Add(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("List");
            }
            ViewBag.Countries = _context.Countries.ToList(); //Get countries again if validation fails
            return View(customer);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            ViewBag.Countries = _context.Countries.ToList();
            var customer = _context.Customers.Find(id);
            if (customer == null)   //If there is no customer, return the not found response
            {
                return NotFound();
            }
            return View("AddEdit", customer);
        }

        //POST Customer/Edit/
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Update(customer);
                _context.SaveChanges();
                return RedirectToAction("List");
            }
            ViewBag.Countries = _context.Countries.ToList();
            return View(customer);
        }

        //GET Customer/Delete/
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        //POST Customer/Delete/
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }

        //SAVE the new customer and add
        [HttpPost]
        public IActionResult Save(Customer customer)
        {
            // Check for duplicate email ONLY when adding a new customer
            if (customer.CustomerID == 0 && _context.Customers.Any(c => c.Email == customer.Email))
            {
                ModelState.AddModelError(nameof(customer.Email), "This email is already in use.");
            }

            if (ModelState.IsValid)
            {
                if (customer.CustomerID == 0) // New customer
                {
                    _context.Customers.Add(customer);
                }
                else // Existing customer
                {
                    _context.Customers.Update(customer);
                }
                _context.SaveChanges();
                return RedirectToAction("List");
            }

            // End here if there's a validation error
            ViewBag.Action = customer.CustomerID == 0 ? "Add" : "Edit";
            ViewBag.Countries = _context.Countries.ToList();
            return View("AddEdit", customer);
        }

        //GET Customer/Select
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


    }
}
