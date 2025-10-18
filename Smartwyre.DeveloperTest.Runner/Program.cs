using System;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        // Initialize dependencies for RebateService
        var rebateDataStore = new RebateDataStore();
        var productDataStore = new ProductDataStore();
        var rebateService = new RebateService(rebateDataStore, productDataStore);

        // Prompt user for Rebate Identifier
        Console.WriteLine("Enter Rebate Identifier:");
        string rebateIdentifier = Console.ReadLine();

        // Prompt user for Product Identifier
        Console.WriteLine("Enter Product Identifier:");
        string productIdentifier = Console.ReadLine();

        // Prompt user for Volume with input validation
        Console.WriteLine("Enter Volume:");
        if (!decimal.TryParse(Console.ReadLine(), out decimal volume))
        {
            // Provide default value and inform user if input is invalid
            Console.WriteLine("Invalid volume input. Using default volume of 0.");
            volume = 0;
        }

        // Create request object with user inputs
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        };

        // Execute rebate calculation
        var result = rebateService.Calculate(request);

        // Display result to user
        Console.WriteLine(result.Success
            ? $"Rebate calculation successful! The calculated amount for {result.Rebate.Identifier} is {result.CalculatedRebateAmount}"
            : "Rebate calculation failed.");
    }
}
