using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Smartwyre.DeveloperTest.Interfaces;

namespace Smartwyre.DeveloperTest.Tests
{
    public class RebateServiceTests
    {
        private readonly Mock<IRebateDataStore> _rebateDataStoreMock;
        private readonly Mock<IProductDataStore> _productDataStoreMock;
        private readonly RebateService _rebateService;

        // Constructor initializes mocks and RebateService with dependencies
        public RebateServiceTests()
        {
            // Mock data stores to isolate RebateService for unit testing
            _rebateDataStoreMock = new Mock<IRebateDataStore>();
            _productDataStoreMock = new Mock<IProductDataStore>();
            _rebateService = new RebateService(_rebateDataStoreMock.Object, _productDataStoreMock.Object);
        }

        [Fact]
        public void Calculate_FixedCashAmount_ValidData_ReturnsSuccess()
        {
            // Arrange: Set up test data for FixedCashAmount scenario
            var request = new CalculateRebateRequest { RebateIdentifier = "R1", ProductIdentifier = "P1" };
            var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 10m };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

            // Mock data store responses
            _rebateDataStoreMock.Setup(x => x.GetRebate("R1")).Returns(rebate);
            _productDataStoreMock.Setup(x => x.GetProduct("P1")).Returns(product);

            // Act: Call the Calculate method
            var result = _rebateService.Calculate(request);

            // Assert: Verify success and that the result was stored
            Assert.True(result.Success);
            _rebateDataStoreMock.Verify(x => x.StoreCalculationResult(rebate, 10m), Times.Once());
        }

        [Fact]
        public void Calculate_FixedRateRebate_ValidData_ReturnsSuccess()
        {
            // Arrange: Set up test data for FixedRateRebate scenario
            var request = new CalculateRebateRequest { RebateIdentifier = "R1", ProductIdentifier = "P1", Volume = 2 };
            var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 0.1m };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 100m };

            // Mock data store responses
            _rebateDataStoreMock.Setup(x => x.GetRebate("R1")).Returns(rebate);
            _productDataStoreMock.Setup(x => x.GetProduct("P1")).Returns(product);

            // Act: Call the Calculate method
            var result = _rebateService.Calculate(request);

            // Assert: Verify success and correct calculation (100 * 0.1 * 2 = 20)
            Assert.True(result.Success);
            _rebateDataStoreMock.Verify(x => x.StoreCalculationResult(rebate, 20m), Times.Once());
        }

        [Fact]
        public void Calculate_AmountPerUom_ValidData_ReturnsSuccess()
        {
            // Arrange: Set up test data for AmountPerUom scenario
            var request = new CalculateRebateRequest { RebateIdentifier = "R1", ProductIdentifier = "P1", Volume = 3 };
            var rebate = new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = 5m };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };

            // Mock data store responses
            _rebateDataStoreMock.Setup(x => x.GetRebate("R1")).Returns(rebate);
            _productDataStoreMock.Setup(x => x.GetProduct("P1")).Returns(product);

            // Act: Call the Calculate method
            var result = _rebateService.Calculate(request);

            // Assert: Verify success and correct calculation (5 * 3 = 15)
            Assert.True(result.Success);
            _rebateDataStoreMock.Verify(x => x.StoreCalculationResult(rebate, 15m), Times.Once());
        }

        [Fact]
        public void Calculate_NullRebate_ReturnsFailure()
        {
            // Arrange: Simulate a null rebate scenario
            var request = new CalculateRebateRequest { RebateIdentifier = "R1", ProductIdentifier = "P1" };
            _rebateDataStoreMock.Setup(x => x.GetRebate("R1")).Returns((Rebate)null);

            // Act: Call the Calculate method
            var result = _rebateService.Calculate(request);

            // Assert: Verify failure and that no result was stored
            Assert.False(result.Success);
            _rebateDataStoreMock.Verify(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never());
        }

        [Fact]
        public void Calculate_UnknownIncentiveType_ReturnsFailure()
        {
            // Arrange: Simulate an unknown incentive type scenario
            var request = new CalculateRebateRequest { RebateIdentifier = "R1", ProductIdentifier = "P1" };
            var rebate = new Rebate { Incentive = (IncentiveType)999 };
            _rebateDataStoreMock.Setup(x => x.GetRebate("R1")).Returns(rebate);

            // Act: Call the Calculate method
            var result = _rebateService.Calculate(request);

            // Assert: Verify failure and that no result was stored
            Assert.False(result.Success);
            _rebateDataStoreMock.Verify(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never());
        }
    }
}
