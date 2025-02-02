using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        //Create list of products for testing
        private static List<Product> products = new List<Product>
        {
            new Product { 
                ProductID = 1, 
                ProductCode = "TRNY10", 
                Name = "Tournament Master 1.0", 
                YearlyPrice = 4.99M, 
                ReleaseDate = DateTime.Now
            },

            new Product { 
                ProductID = 2, 
                ProductCode = "LEAG10", 
                Name = "League Scheduler 1.0", 
                YearlyPrice = 4.99M, 
                ReleaseDate = DateTime.Now 
            },

            new Product { 
                ProductID = 3, 
                ProductCode = "LEAGD10", 
                Name = "League Scheduler Deluxe 1.0", 
                YearlyPrice = 7.99M, 
                ReleaseDate = DateTime.Now
            }
        };

        [HttpGet]   //When user clicks the edit button
        public IActionResult Edit(int id)   //product ID in the URL
        {
            //Find product in test list
            var product = products.Find(p => p.ProductID == id);    //Finds the product to show in the edit form
            if (product == null)
            {
                return NotFound();
            }

            return View("AddEdit", product);
        }

        [HttpPost]  //Handles when form is submitted
        public IActionResult Edit(Product product)
        {
			if (ModelState.IsValid) //Validation check
            {
                var existingProduct = products.Find(p => p.ProductID == product.ProductID);

                if (existingProduct != null)
                {				
                existingProduct.ProductCode = product.ProductCode;
				existingProduct.Name = product.Name;
				existingProduct.YearlyPrice = product.YearlyPrice;
				existingProduct.ReleaseDate = product.ReleaseDate;
                }
				return RedirectToAction("List");
			}
			return View("AddEdit", product);
        }

		[HttpGet]
		public IActionResult Add()
		{
			var product = new Product();  // Creates blank product
			return View("AddEdit", product);  // Uses same view as Edit
		}

		[HttpPost]
		public IActionResult Add(Product product)
		{
			if (ModelState.IsValid)
			{
				// Add the new product to the list
				products.Add(product);
				return RedirectToAction("List");
			}
			return View("AddEdit", product);
		}

		[HttpGet]
		public IActionResult Delete(int id)
		{
			var product = products.Find(p => p.ProductID == id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}

		[HttpPost]
		public IActionResult Delete(Product product)
		{
			var productToDelete = products.Find(p => p.ProductID == product.ProductID);
			if (productToDelete != null)
			{
				products.Remove(productToDelete);
			}
			return RedirectToAction("List");
		}

		public IActionResult List()
        {
            //Pass list of products to view
            return View(products);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
