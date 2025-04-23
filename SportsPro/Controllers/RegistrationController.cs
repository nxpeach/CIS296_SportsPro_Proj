using Microsoft.AspNetCore.Mvc;
using SportsPro.Data.UnitOfWork;
using SportsPro.Models;
using SportsPro.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Linq;
using SportsPro.Data.Repositories;

namespace SportsPro.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var options = new QueryOptions<Customer>
            {
                OrderBy = c => c.LastName
            };
            ViewBag.Customers = _unitOfWork.Customers.List(options).ToList();
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

            var customer = _unitOfWork.Customers.Get(customerID.Value);
            if (customer == null) return RedirectToAction("Get");

            var viewModel = new RegistrationsViewModel
            {
                Customer = customer,
                Products = _unitOfWork.Products
                    .List(new QueryOptions<Product> { OrderBy = p => p.Name })
                    .ToList(),
                CustomerProducts = _unitOfWork.Registrations
                    .List(new QueryOptions<Registration>
                    {
                        WhereClauses = { r => r.CustomerID == customerID.Value },
                        Includes = { r => r.Product }
                    })
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

            var registrationOptions = new QueryOptions<Registration>();
            registrationOptions.AddWhere(r => r.CustomerID == customerID.Value && r.ProductID == productID);

            bool alreadyRegistered = _unitOfWork.Registrations.List(registrationOptions).Any();

            if (!alreadyRegistered)
            {
                _unitOfWork.Registrations.Insert(new Registration
                {
                    CustomerID = customerID.Value,
                    ProductID = productID
                });
                _unitOfWork.Save();
            }

            return RedirectToAction("Registrations");
        }


        [HttpPost]
        public IActionResult Delete(int productID)
        {
            var customerID = HttpContext.Session.GetInt32("CustomerID");
            if (customerID == null) return RedirectToAction("Get");

            var registration = _unitOfWork.Registrations
                .List(new QueryOptions<Registration>
                {
                    WhereClauses = { r => r.CustomerID == customerID.Value && r.ProductID == productID }
                })
                .FirstOrDefault();

            if (registration != null)
            {
                _unitOfWork.Registrations.Delete(registration);
                _unitOfWork.Save();
            }

            return RedirectToAction("Registrations");
        }
    }
}
