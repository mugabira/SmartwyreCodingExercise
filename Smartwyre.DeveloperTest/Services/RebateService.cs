using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Interfaces;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    // Dictionary to map incentive types to their calculators for extensibility
    private readonly Dictionary<IncentiveType, IIncentiveCalculator> _calculators;

    // Constructor with dependency injection to improve testability and adhere to SOLID principles
    public RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore)
    {
        // Guard clauses to ensure dependencies are not null
        _rebateDataStore = rebateDataStore ?? throw new ArgumentNullException(nameof(rebateDataStore));
        _productDataStore = productDataStore ?? throw new ArgumentNullException(nameof(productDataStore));

        // Initialize calculators dictionary to support strategy pattern for different incentive types
        _calculators = new Dictionary<IncentiveType, IIncentiveCalculator>
        {
            { IncentiveType.FixedCashAmount, new FixedCashAmountCalculator() },
            { IncentiveType.FixedRateRebate, new FixedRateRebateCalculator() },
            { IncentiveType.AmountPerUom, new AmountPerUomCalculator() }
        };
    }
    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        // null check to prevent null reference exceptions
        if (request == null)
        {
            return new CalculateRebateResult { Success = false };
        }

        // Retrieve rebate data, avoiding repeated data store creation
        var rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        if (rebate == null)
        {
            return new CalculateRebateResult { Success = false };
        }

        // Retrieve product data, using injected dependency for testability
        var product = _productDataStore.GetProduct(request.ProductIdentifier);

        // Check if the incentive type has a registered calculator to handle unknown types
        if (!_calculators.TryGetValue(rebate.Incentive, out var calculator))
        {
            return new CalculateRebateResult { Success = false };
        }

        var result = new CalculateRebateResult();

        // Use strategy pattern to delegate validation and calculation to the appropriate calculator
        if (calculator.IsApplicable(rebate, product, request))
        {
            var rebateAmount = calculator.CalculateAmount(rebate, product, request);
            // Store result only if calculation is successful, using injected data store
            _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);

            result.Rebate = rebate;
            result.CalculatedRebateAmount = rebateAmount;
            result.Success = true;
        }
        else
        {
            result.Success = false;
        }

        return result;
    }
}
