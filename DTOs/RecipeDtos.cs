using RecipeCostAPI.Models;

namespace RecipeCostAPI.DTOs; 

public record IngredientDto(
    int Id, 
    string Name,
    UnitType BaseUnit, 
    decimal CostPerBaseUnit);

public record RecipeIngredientDto(
    int IngredientId,
    string IngredientName,
    double Amount,
    UnitType Unit,
    decimal LineItemCost);

public record RecipeDto(
    int Id, 
    string Name,
    int Servings,
    string? Description,
    decimal TotalCost,
    decimal CostPerServing,
    List<RecipeIngredientDto> Ingredients);

public record CreateRecipeDto(
    string Name,
    int Servings,
    string? Description,
    List<CreateRecipeIngredientDto> Ingredients);

public record CreateRecipeIngredientDto(
    int IngredientId,
    double Amount,
    UnitType Unit);
