using Smartwyre.DeveloperTest.Interfaces;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Services
{
    public class FixedRateRebateCalculator : IIncentiveCalculator
    {
        public IncentiveType IncentiveType => IncentiveType.FixedRateRebate;
        public bool IsApplicable(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return rebate != null &&
                   product != null &&
                   product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate) &&
                   rebate.Percentage > 0 &&
                   product.Price > 0 &&
                   request.Volume > 0;
        }

        public decimal CalculateAmount(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return product.Price * rebate.Percentage * request.Volume;
        }
    }
}
