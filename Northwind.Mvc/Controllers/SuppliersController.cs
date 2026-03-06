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
            supplierInDb is null ? 0 : 1, supplierInDb);
        return View(model);
    }
    // POST : /suppliers/edit
    // Body : Supplier
    // Updates an existing supplier

    [HttpPost]
    public IActionResult Edit(Supplier supplier)
    {
        int affected = 0;

        if (ModelState.IsValid)
        {
            Supplier? supplierInDb = _db.Suppliers.Find(supplier.SupplierId);

            if (supplierInDb is not null)
            {
                supplierInDb.CompanyName = supplier.CompanyName;
                supplierInDb.Country = supplier.Country;
                supplierInDb.Phone = supplier.Phone;

                affected = _db.SaveChanges();

            }
        }

        SupplierViewModel mode = new(affected, supplier);

        if (affected == 0)
        {
            return View(mode);
        }
        else
        {
            return RedirectToAction("Index");



        }

    }

    public IActionResult Delete(int? id)
    {
        Supplier? supplierInDb = _db.Suppliers.Find(id);
        SupplierViewModel model = new(
            supplierInDb is null ? 0 : 1, supplierInDb);
        return View(model);
    }
    [HttpPost("/suppliers/delete/{id:int?}")]
    [ValidateAntiForgeryToken]
    public IActionResult DoTheDelete(int? id)
    {
        int affected = 0;
        Supplier? supplierInDb = _db.Suppliers.Find(id);


        if (supplierInDb is not null)
        {
            _db.Suppliers.Remove(supplierInDb);
            affected = _db.SaveChanges();
        }

        SupplierViewModel mode = new(affected, supplierInDb);
        if (affected == 0)
        {
            return View(mode);
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
    public IActionResult Add()
    {
        SupplierViewModel model = new(0, new Supplier());
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(Supplier supplier)
    {
        int affected = 0;

        if (ModelState.IsValid)
        {
            _db.Suppliers.Add(supplier);
            affected = _db.SaveChanges();
        }

        SupplierViewModel model = new(affected, supplier);
        if(affected == 0)
        {
            return View(model);
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

}