using RecipeCostAPI.Models; 
using RecipeCostAPI.Services.Interfaces;
using RecipeCost.Shared;
namespace RecipeCostAPI.Services; 

public class PricingService : IPricingService
{ 
    private decimal GetUnitValue(UnitType unit) => unit switch
    {
        UnitType.Gram or UnitType.Milliliter => 1m,
        UnitType.Kilogram or UnitType.Liter => 1000m,
        UnitType.Ounce => 28.3495m,
        UnitType.Pound => 453.592m,
        UnitType.Cup => 236.588m,
        UnitType.Quart => 946.353m,
        UnitType.Gallon => 3785.41m,
        _ => 1m // Piece, Each, etc.
    };

    public decimal CalculateLineItemCost(decimal amount, UnitType usedUnit, Ingredient ingredient)
    {
        if (ingredient == null) return 0;

        // 1. Validate: Prevent mixing mass and volume
        ValidateUnitCompatibility(usedUnit, ingredient.BaseUnit);

        // 2. Normalize to "Smallest Unit" (Grams/Milliliters)
        decimal totalAmountInSmallestUnit = amount * GetUnitValue(usedUnit);
        decimal baseUnitInSmallestUnit = GetUnitValue(ingredient.BaseUnit);

        // 3. The Ratio: How many "Base Units" are in the "Used Amount"?
        // Example: 1 Pound (453.59g) / 1 Ounce (28.35g) = 16 
        decimal quantityInBaseUnits = totalAmountInSmallestUnit / baseUnitInSmallestUnit;

        // 4. Final Math: (Quantity in Bases) * (Price per Base)
        return quantityInBaseUnits * ingredient.CostPerBaseUnit;
    }

    private void ValidateUnitCompatibility(UnitType used, UnitType baseUnit)
    {
        // Piece and Each are dimensionless - they work with anything
        if (used == UnitType.Piece || used == UnitType.Each || baseUnit == UnitType.Piece || baseUnit == UnitType.Each)
            return;

        bool isUsedWeight = IsWeight(used);
        bool isUsedVolume = IsVolume(used);
        bool isBaseWeight = IsWeight(baseUnit);
        bool isBaseVolume = IsVolume(baseUnit);

        // Both must be in the same category (weight OR volume)
        if ((isUsedWeight && !isBaseWeight) || (isUsedVolume && !isBaseVolume))
        {
            throw new InvalidOperationException($"Cannot convert {used} to {baseUnit}. Incompatible unit types.");
        }
    }

    private bool IsWeight(UnitType unit) =>
        unit is UnitType.Gram or UnitType.Kilogram or UnitType.Ounce or UnitType.Pound;

    private bool IsVolume(UnitType unit) =>
        unit is UnitType.Milliliter or UnitType.Liter or UnitType.Cup or UnitType.Quart or UnitType.Gallon;

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