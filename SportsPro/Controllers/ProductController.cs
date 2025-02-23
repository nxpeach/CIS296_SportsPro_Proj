using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace SportsPro.Controllers
{
	public class ProductController : Controller
	{
		private SportsProContext context { get; set; }
		public ProductController(SportsProContext ctx)
		{
			context = ctx;
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
			var product = context.Products.Find(id);
			return View("AddEdit", product);
		}

		[HttpPost]
		public RedirectToActionResult Edit(Product product)
		{
			if (ModelState.IsValid)
			{
				if (product.ProductID == 0)
				{
					context.Products.Add(product);
					TempData["Message"] = $"{product.Name} was successfully added.";
				}
				else
				{
					context.Products.Update(product);
					TempData["Message"] = $"{product.Name} was successfully edited.";
				}
				context.SaveChanges();
				return RedirectToAction("List");
			}
			else
			{
				ViewBag.Action = (product.ProductID == 0) ? "Add" : "Edit";
				return RedirectToAction("AddEdit", product);
			}
		}

		[HttpGet]
		public ViewResult Delete(int id)
		{
			var product = context.Products.Find(id);
			if (product == null)
			{
				return View("List");  // or return NotFound();
			}
			return View(product);
		}

		[HttpPost]
		public RedirectToActionResult Delete(Product product)
		{
            context.Products.Remove(product);
			context.SaveChanges();

			TempData["Message"] = "Product was successfully deleted.";
			return RedirectToAction("List");
		}

        [Route("products")]
        public IActionResult List()
		{
			var products = context.Products.ToList();
			return View(products);
		}
    }
}
