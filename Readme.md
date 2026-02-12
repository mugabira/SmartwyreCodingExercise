# Smartwyre Developer Test

## Overview
This is a coding exercise for the Smartwyre Developer Test. The original task was to refactor the `RebateService.cs` to adhere to SOLID principles, improve testability, readability, and extensibility for future incentive types. The refactored solution implements the Strategy Pattern for incentive calculations and includes comprehensive unit tests and an updated console runner.

## Key Changes Made

### 1. **Strategy Pattern Implementation**
- **IIncentiveCalculator Interface**: Defines the contract for all rebate calculation strategies with `IsApplicable` and `CalculateAmount` methods.
- **Concrete Strategy Classes**: 
  - `FixedCashAmountCalculator`: Handles fixed monetary rebates
  - `FixedRateRebateCalculator`: Calculates percentage-based rebates
  - `AmountPerUomCalculator`: Computes amount per unit rebates
- **Dictionary-based Strategy Selection**: `RebateService` uses a `Dictionary<IncentiveType, IIncentiveCalculator>` to dynamically select the appropriate calculator at runtime based on the rebate's `IncentiveType`.

### 2. **SOLID Principles Applied**
- **Single Responsibility**: Each calculator handles one specific incentive type
- **Open/Closed**: New incentive types can be added by implementing `IIncentiveCalculator` and registering in the dictionary
- **Dependency Inversion**: `RebateService` depends on abstractions (`IRebateDataStore`, `IProductDataStore`)
- **Interface Segregation**: Clean interfaces for data access and calculation strategies

### 3. **Dependency Injection**
- `RebateService` constructor accepts `IRebateDataStore` and `IProductDataStore` interfaces
- Eliminates tight coupling to concrete implementations
- Enables easy mocking for unit testing

### 4. **In-Memory Database Implementation**
- **RebateDataStore**: Uses `Dictionary<string, Rebate>` for in-memory storage with seeded sample data
- **ProductDataStore**: Uses `Dictionary<string, Product>` for in-memory storage with seeded sample data
- Both implement their respective interfaces (`IRebateDataStore`, `IProductDataStore`)

### 5. **Unit Tests**
- Comprehensive tests in `RebateServiceTests.cs` using Moq and xUnit
- Tests cover all three incentive types with valid data
- Tests edge cases like null rebates and unknown incentive types
- Verify proper storage of calculation results

### 6. **Console Runner Updates**
- Interactive input prompts for `RebateIdentifier`, `ProductIdentifier`, and `Volume`
- Input validation for volume with default fallback
- Clear success/failure output

## Dependencies

### New Dependencies Added
- **Moq**: For mocking interfaces in unit tests
  - Package: `Moq` (version 4.18.4 or later)
  - Used in: `Smartwyre.DeveloperTest.Tests` project
- **xUnit**: For unit testing framework
  - Package: `xunit` and `xunit.runner.visualstudio`
  - Used in: `Smartwyre.DeveloperTest.Tests` project
- **Microsoft.NET.Test.Sdk**: Required for xUnit test execution
  - Package: `Microsoft.NET.Test.Sdk`

### Project Structure Dependencies
Smartwyre.DeveloperTest/
├── Smartwyre.DeveloperTest.Services/    # Contains RebateService and strategy classes
├── Smartwyre.DeveloperTest.Data/        # Contains data store implementations
├── Smartwyre.DeveloperTest.Types/       # Contains domain types and enums
├── Smartwyre.DeveloperTest.Runner/      # Console application for testing
└── Smartwyre.DeveloperTest.Tests/       # Unit tests


## Testing the Solution

### Running Unit Tests
1. Open the solution in Visual Studio or VS Code
2. Build the solution (`dotnet build` or Build → Build Solution)
3. Run tests:
   - **Visual Studio**: Test → Run All Tests
   - **Command Line**: `dotnet test Smartwyre.DeveloperTest.Tests.csproj`
4. All tests should pass, verifying:
   - Each incentive type calculation works correctly
   - Error handling for null rebates and unknown incentive types
   - Proper storage of calculation results

### Testing via Console Application (Program.cs)

The console application (`Smartwyre.DeveloperTest.Runner`) uses seeded in-memory data for testing. Follow these steps:

```bash
#### 1. **Build and Run**

cd Smartwyre.DeveloperTest.Runner
dotnet build
dotnet run

2. Sample Test Scenarios
The system includes seeded data for testing all incentive types. Use these combinations:
FixedCashAmount Test:

Rebate Identifier: REBATE1
Product Identifier: PROD1 or PROD4
Volume: 0 (not used for FixedCashAmount)
Expected Result: "Rebate calculation successful!"
Calculation: Fixed $50 rebate amount

FixedRateRebate Test:

Rebate Identifier: REBATE2
Product Identifier: PROD2 or PROD4
Volume: 2
Expected Result: "Rebate calculation successful!"
Calculation: 200 * 0.1 * 2 = $40 rebate amount

AmountPerUom Test:

Rebate Identifier: REBATE3
Product Identifier: PROD3
Volume: 3
Expected Result: "Rebate calculation successful!"
Calculation: 5 * 3 = $15 rebate amount

Compatibility Test (Multiple Incentives):

Rebate Identifier: REBATE1 (FixedCashAmount)
Product Identifier: PROD4 (supports FixedCashAmount + FixedRateRebate)
Volume: 0
Expected Result: "Rebate calculation successful!"

Failure Test (Incompatible Product):

Rebate Identifier: REBATE1 (FixedCashAmount)
Product Identifier: PROD2 (only supports FixedRateRebate)
Volume: 0
Expected Result: "Rebate calculation failed!"
Reason: Product doesn't support FixedCashAmount incentive

Invalid Identifier Test:

Rebate Identifier: INVALID
Product Identifier: PROD1
Volume: 1
Expected Result: "Rebate calculation failed!"
Reason: Rebate not found in data store

3. Console Interaction Flow
Enter Rebate Identifier: REBATE2
Enter Product Identifier: PROD2
Enter Volume: 2
Rebate calculation successful!
````

### 4. Seeded Data Overview
```
Rebates:

REBATE1: FixedCashAmount, $50 fixed rebate
REBATE2: FixedRateRebate, 10% percentage rebate
REBATE3: AmountPerUom, $5 per unit rebate

Products:

PROD1: $100, supports FixedCashAmount only
PROD2: $200, supports FixedRateRebate only
PROD3: $50, supports AmountPerUom only
PROD4: $150, supports FixedCashAmount + FixedRateRebate (demonstrates flags enum)
````
Verification Steps

Build Verification: Ensure solution builds without errors
Test Coverage: Run all unit tests to verify core functionality
Console Testing: Use the sample scenarios above to test:

Each incentive type calculation
Product compatibility validation
Error handling for invalid inputs
Strategy pattern selection based on IncentiveType