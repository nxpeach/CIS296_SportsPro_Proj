using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Data.Repositories;
using SportsPro.Data;
using System.Linq;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _productRepo;

        public ProductController(IRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        public ViewResult Add()
        {
            ViewBag.Action = "Add";
            return View("AddEdit", new Product());
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var product = _productRepo.Get(id);
            return View("AddEdit", product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductID == 0)
                {
                    _productRepo.Insert(product);
                    TempData["Message"] = $"{product.Name} was successfully added.";
                }
                else
                {
                    _productRepo.Update(product);
                    TempData["Message"] = $"{product.Name} was successfully edited.";
                }

                _productRepo.Save();
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Action = (product.ProductID == 0) ? "Add" : "Edit";
                return View("AddEdit", product);
            }
        }

        [HttpGet]
        public ViewResult Delete(int id)
        {
            var product = _productRepo.Get(id);
            if (product == null)
            {
                return View("List");  // or return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            _productRepo.Delete(product);
            _productRepo.Save();

            TempData["Message"] = "Product was successfully deleted.";
            return RedirectToAction("List");
        }

        [Route("products")]
        public IActionResult List()
        {
            var options = new QueryOptions<Product>
            {
                OrderBy = p => p.Name
            };

            var products = _productRepo.List(options).ToList();
            return View(products);
        }
    }
}
