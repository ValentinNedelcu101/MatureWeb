using Microsoft.AspNetCore.Mvc;
using Northwind.EntityModels;
using Northwind.Mvc.Models;
namespace Northwind.Mvc.Controllers;

    public class SuppliersController : Controller
    {
    private readonly NorthwindContext _db;

    public SuppliersController(NorthwindContext db)
    {
        _db = db;
    }

    public IActionResult Index()
        {
        SuppliersIndexViewModel model = new(_db.Suppliers
            .OrderBy(c => c.Country)
            .ThenBy(c => c.CompanyName));
            return View(model);
        }

    public IActionResult Edit(int? id)
    {
        Supplier? supplierInDb = _db.Suppliers.Find(id);
        SupplierViewModel model = new(
            supplierInDb is null ? 0 : 1, supplierInDb ); 
        return View(model);
    }

    }

