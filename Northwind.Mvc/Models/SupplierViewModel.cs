using Northwind.EntityModels;
namespace Northwind.Mvc.Models;

public record SupplierViewModel(int EntitiesAffected, Supplier? Supplier);

