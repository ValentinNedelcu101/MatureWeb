using Microsoft.AspNetCore.Mvc;
using Northwind.Mvc.Models;
using System.Diagnostics;
using Northwind.EntityModels;
using Microsoft.EntityFrameworkCore;
namespace Northwind.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NorthwindContext _db;

        public HomeController(ILogger<HomeController> logger, NorthwindContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult Index()
        {
            //_logger.LogError("This is an error message from HomeController Index action.");
            //_logger.LogWarning("This is a warning message from HomeController Index action.");
            //_logger.LogWarning("Second warning message from HomeController Index action.");
            //_logger.LogInformation("This is an informational message from HomeController Index action.");

            HomeIndexViewModel model = new
                (
                    VisitorCount: Random.Shared.Next(1, 1001),
                    Categories: _db.Categories.ToList(),
                    Products: _db.Products.ToList()
                );

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ProductDetail(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Product ID is required. For example; /Home/ProductDetail/21");
            }
            Product? model = _db.Products.Include(p => p.Category).SingleOrDefault(p => p.ProductId == id);
            if ( model is null)
            {
                return NotFound($"No product found with ID {id}");
            }
            return View(model);
        }
        public IActionResult ModelBinding()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ModelBinding(Thing thing)
        {
            HomeModelBindingViewModel model = new(
                Thing: thing,
                HasErrors: !ModelState.IsValid,
                ValidationErrors: ModelState.Values
                    .SelectMany(state => state.Errors)
                    .Select(error => error.ErrorMessage)
                );
            return View(model);
        }

        public IActionResult ProductsThatCostMoreThan(decimal? price)
        {
                if (!price.HasValue) // if price is null
                {
                    return BadRequest("Price is required. For example; /Home/ProductsThatCostMoreThan?price=50");
                }
                IEnumerable<Product> model = _db.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .Where(p => p.UnitPrice > price);
                if (!model.Any()) // if no products found
                {
                    return NotFound($"No products found that cost more than {price:C}");
                }

            ViewData["MaxPrice"] = price.Value.ToString("C");

            return View("Views/Home/CostlyProducts.cshtml", model);
        }
    }
}
