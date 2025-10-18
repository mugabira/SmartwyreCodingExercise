using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Interfaces;

public interface IRebateService
{
    CalculateRebateResult Calculate(CalculateRebateRequest request);
}
