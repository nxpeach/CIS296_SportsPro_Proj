using Microsoft.AspNetCore.Mvc;
using SportsPro.Data.Repositories;
using SportsPro.Data.UnitOfWork;
using SportsPro.Models;
using System.Linq;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IRepository<Customer> _customerRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IRepository<Customer> customerRepo, IUnitOfWork unitOfWork)
        {
            _customerRepo = customerRepo;
            _unitOfWork = unitOfWork;
        }

        // GET: /customers
        [Route("/customers")]
        public IActionResult List()
        {
            var options = new QueryOptions<Customer>();  // No filters = get all
            var customers = _customerRepo.List(options);
            return View(customers);
        }

        // GET: Customer/Add
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            ViewBag.Countries = _unitOfWork.Countries.List(new QueryOptions<Country>());
            return View("AddEdit", new Customer());
        }

        // POST: Customer/Add
        [HttpPost]
        public IActionResult Add(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _customerRepo.Insert(customer);
                _unitOfWork.Save();
                return RedirectToAction("List");
            }

            ViewBag.Countries = _unitOfWork.Countries.List(new QueryOptions<Country>());
            return View(customer);
        }

        // GET: Customer/Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            ViewBag.Countries = _unitOfWork.Countries.List(new QueryOptions<Country>());

            var customer = _customerRepo.Get(id);
            if (customer == null)
                return NotFound();

            return View("AddEdit", customer);
        }

        // POST: Customer/Edit
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _customerRepo.Update(customer);
                _unitOfWork.Save();
                return RedirectToAction("List");
            }

            ViewBag.Countries = _unitOfWork.Countries.List(new QueryOptions<Country>());
            return View(customer);
        }

        // GET: Customer/Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var customer = _customerRepo.Get(id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: Customer/DeleteConfirmed
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = _customerRepo.Get(id);
            if (customer != null)
            {
                _customerRepo.Delete(customer);
                _unitOfWork.Save();
            }

            return RedirectToAction("List");
        }

        // POST: Customer/Save (Unified Save for Add/Edit)
        [HttpPost]
        public IActionResult Save(Customer customer)
        {
            var options = new QueryOptions<Customer>();
            options.AddWhere(c => c.Email == customer.Email);
            var existingCustomer = _customerRepo.List(options).FirstOrDefault();


            if (customer.CustomerID == 0 && existingCustomer != null)
            {
                ModelState.AddModelError(nameof(customer.Email), "This email is already in use.");
            }

            if (ModelState.IsValid)
            {
                if (customer.CustomerID == 0)
                {
                    _customerRepo.Insert(customer);
                }
                else
                {
                    _customerRepo.Update(customer);
                }

                _unitOfWork.Save();
                return RedirectToAction("List");
            }

            ViewBag.Action = customer.CustomerID == 0 ? "Add" : "Edit";
            ViewBag.Countries = _unitOfWork.Countries.List(new QueryOptions<Country>());
            return View("AddEdit", customer);
        }

        // GET: Customer/Select
        [HttpGet]
        public IActionResult Get()
        {
            var options = new QueryOptions<Customer> { OrderBy = c => c.LastName };
            ViewBag.Customers = _customerRepo.List(options).ToList();
            return View();
        }

        // POST: Customer/Select
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
