using Smartwyre.DeveloperTest.Interfaces;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Services
{
    public class AmountPerUomCalculator : IIncentiveCalculator
    {
        public IncentiveType IncentiveType => IncentiveType.AmountPerUom;
        public bool IsApplicable(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return rebate != null &&
                   product != null &&
                   product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom) &&
                   rebate.Amount > 0 &&
                   request.Volume > 0;
        }

        public decimal CalculateAmount(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return rebate.Amount * request.Volume;
        }
    }
}
