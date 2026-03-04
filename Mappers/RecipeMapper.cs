using RecipeCostAPI.Models;
using RecipeCostAPI.DTOs;
using RecipeCostAPI.Services; // Ensure this is here for IPricingService


namespace RecipeCostAPI.Mappers;
public static class RecipeMapper
{
    // 1. Map Ingredient -> IngredientDTO
    public static IngredientDto ToDto(this Ingredient ingredient) =>
        new(
            ingredient.Id,
            ingredient.Name,
            ingredient.BaseUnit,
            ingredient.CostPerBaseUnit
            );

    // 2. Map RecipeIngredient -> RecipeIngredientDTO
    public static RecipeIngredientDto ToDto(this RecipeIngredient ri, IPricingService pricingService) =>
    new(
        ri.IngredientId,
        ri.Ingredient?.Name ?? "Unknown",
        ri.Amount,
        ri.Unit,
        pricingService.CalculateLineItemCost(ri.Amount, ri.Unit, ri.Ingredient!)
    );

    // 3. Map Recipe -> RecipeDto (The "Big" One)
    public static RecipeDto ToDto(this Recipe recipe, IPricingService pricingService)
    {
        var ingredientDtos = recipe.RecipeIngredients
            .Select(ri => ri.ToDto(pricingService))
            .ToList();

        var totalCost = ingredientDtos.Sum(i => i.LineItemCost);

        return new RecipeDto(
            recipe.Id,
            recipe.Name,
            recipe.Servings,
            recipe.Description,
            totalCost,
            recipe.Servings > 0 ? totalCost / recipe.Servings : 0,
            ingredientDtos
        );
    }
}


public static class RecipeMappingExtensions
{
    /// <summary>
    /// Converts a CreateRecipeDto into a Recipe Entity.
    /// Note: This does NOT fetch from the DB, it just prepares the object graph.
    /// </summary>
    public static Recipe ToEntity(this CreateRecipeDto dto)
    {
        return new Recipe
        {
            Name = dto.Name,
            Servings = dto.Servings,
            Description = dto.Description,
            // Map the child collection
            RecipeIngredients = dto.Ingredients.Select(i => new RecipeIngredient
            {
                IngredientId = i.IngredientId,
                Amount = i.Amount,
                Unit = i.Unit // Our Enum handles the type safety here
            }).ToList()
        };
    }
}