using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Interfaces
{
    internal interface IIncentiveCalculator
    {
        bool IsApplicable(Rebate rebate, Product product, CalculateRebateRequest request);
        decimal CalculateAmount(Rebate rebate, Product product, CalculateRebateRequest request);
    }
}
