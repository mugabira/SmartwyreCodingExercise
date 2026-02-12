using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Interfaces;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IProductDataStore
{
    // In-memory storage for products, using a dictionary with Identifier as the key
    private readonly Dictionary<string, Product> _products;

    public ProductDataStore()
    {
        // Initialize the in-memory database
        _products = new Dictionary<string, Product>();

        // Seed the database with sample product data
        SeedData();
    }

    // Seed the in-memory database with sample products for testing
    private void SeedData()
    {
        _products.Add("PROD1", new Product
        {
            Id = 1,
            Identifier = "PROD1",
            Price = 100m,
            Uom = "Each",
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        });

        _products.Add("PROD2", new Product
        {
            Id = 2,
            Identifier = "PROD2",
            Price = 200m,
            Uom = "Each",
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        });

        _products.Add("PROD3", new Product
        {
            Id = 3,
            Identifier = "PROD3",
            Price = 50m,
            Uom = "Unit",
            SupportedIncentives = SupportedIncentiveType.AmountPerUom
        });

        _products.Add("PROD4", new Product
        {
            Id = 4,
            Identifier = "PROD4",
            Price = 150m,
            Uom = "Each",
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount | SupportedIncentiveType.FixedRateRebate
        });
    }

    public Product GetProduct(string productIdentifier)
    {
        // Retrieve product from in-memory store, return null if not found
        return _products.TryGetValue(productIdentifier, out var product) ? product : null;
    }
}
