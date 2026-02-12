using Smartwyre.DeveloperTest.Interfaces;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Services
{
    // The strategy pattern for FixedCashAmount
    public class FixedCashAmountCalculator : IIncentiveCalculator
    {
        public bool IsApplicable(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return rebate != null &&
                   product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount) &&
                   rebate.Amount > 0;
        }

        public decimal CalculateAmount(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return rebate.Amount;
        }
    }
}
