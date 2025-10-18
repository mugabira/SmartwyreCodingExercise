using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Interfaces;

namespace Smartwyre.DeveloperTest.Data
{
    public class RebateDataStore : IRebateDataStore
    {
        // In-memory storage for rebates, using a dictionary with Identifier as the key
        private readonly Dictionary<string, Rebate> _rebates;

        public RebateDataStore()
        {
            // Initialize the in-memory database
            _rebates = new Dictionary<string, Rebate>();

            // Seed the database with sample rebate data
            SeedData();
        }

        // Seed the in-memory database with sample rebates for testing
        private void SeedData()
        {
            _rebates.Add("REBATE1", new Rebate
            {
                Identifier = "REBATE1",
                Incentive = IncentiveType.FixedCashAmount,
                Amount = 50m,
                Percentage = 0m
            });

            _rebates.Add("REBATE2", new Rebate
            {
                Identifier = "REBATE2",
                Incentive = IncentiveType.FixedRateRebate,
                Amount = 0m,
                Percentage = 0.1m // 10% rebate
            });

            _rebates.Add("REBATE3", new Rebate
            {
                Identifier = "REBATE3",
                Incentive = IncentiveType.AmountPerUom,
                Amount = 5m, // $5 per unit
                Percentage = 0m
            });
        }

        public Rebate GetRebate(string rebateIdentifier)
        {
            // Retrieve rebate from in-memory store, return null if not found
            return _rebates.TryGetValue(rebateIdentifier, out var rebate) ? rebate : null;
        }

        public void StoreCalculationResult(Rebate rebate, decimal rebateAmount)
        {
            // Log or store the calculation result (in-memory update not required for this exercise)
            // For demonstration, we'll just update the rebate's Amount if applicable
            if (_rebates.ContainsKey(rebate.Identifier))
            {
                // Optionally update the rebate's Amount field (though not strictly necessary)
                _rebates[rebate.Identifier].Amount = rebateAmount;
            }
        }
    }
}
