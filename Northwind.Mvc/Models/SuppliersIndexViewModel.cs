using Northwind.EntityModels;
namespace Northwind.Mvc.Models;

public record SuppliersIndexViewModel(IEnumerable<Supplier>? Suppliers);
    

