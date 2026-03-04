using RecipeCostAPI.Models;

namespace RecipeCostAPI.Services;

public interface IPricingService
{
	decimal CalculateLineItemCost(double amount, UnitType usedUnit, Ingredient ingredient);
}

public class PricingService : IPricingService
{
	public decimal CalculateLineItemCost(double amount, UnitType usedUnit, Ingredient ingredient)
	{
		if (ingredient == null) return 0;

		// Simple logic for now, but easily expandable for unit conversions!
		// Example: if (usedUnit == UnitType.Kilogram && ingredient.BaseUnit == UnitType.Gram) factor = 1000;

		return (decimal)amount * ingredient.CostPerBaseUnit;
	}
}