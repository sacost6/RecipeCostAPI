using RecipeCostAPI.Models; 
using RecipeCostAPI.Services.Interfaces;
using RecipeCost.Shared;
namespace RecipeCostAPI.Services; 

public class PricingService : IPricingService
{
	public decimal CalculateLineItemCost(decimal amount, UnitType usedUnit, Ingredient ingredient)
	{
		if (ingredient == null) return 0;

		// Simple logic for now, but easily expandable for unit conversions!
		// Example: if (usedUnit == UnitType.Kilogram && ingredient.BaseUnit == UnitType.Gram) factor = 1000;
		if(usedUnit != ingredient.BaseUnit)
		{
			// Implement unit conversion logic here if needed
			// For now, we assume the used unit is the same as the ingredient's base unit
			throw new NotImplementedException("Unit conversion not implemented yet.");
        }	

        return (decimal)amount * ingredient.CostPerBaseUnit;
	}

    // Calculate the total cost of a recipe by summing the costs of its line items
	public decimal CalculateRecipeCost(Recipe recipe)
	{
		if(recipe == null || recipe.RecipeIngredients == null) return 0;
		decimal totalCost = 0;
		foreach (var lineItem in recipe.RecipeIngredients)
		{
			totalCost += CalculateLineItemCost(lineItem.Quantity, lineItem.Unit, lineItem.Ingredient);
		}
		return totalCost;
    }
}