using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeCostAPI.Models;
// Data model for Ingredient
public class Ingredient
{
    public int Id { get; init; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal CostPerUnit { get; set; }

    [Required]
    public UnitType BaseUnit { get; set; }

    [Range(0, 999999)]
    public decimal CostPerBaseUnit { get; set; }

    // Links back to any recipes using this ingredient
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}