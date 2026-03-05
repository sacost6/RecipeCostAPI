using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeCostAPI.Data; 
using RecipeCostAPI.Mappers;
using RecipeCostAPI.Models;
using RecipeCost.Shared;

namespace RecipeCostAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController : ControllerBase
{
    private readonly AppDbContext _context;

    public IngredientsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/ingredients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredients()
    {
        var ingredients = await _context.Ingredients.ToListAsync();

        // Use your Mapper to convert Entities to DTOs
        return Ok(ingredients.Select(i => i.ToDto()));
    }

    // GET: api/ingredients/1
    [HttpGet("{id}")]
    public async Task<ActionResult<IngredientDto>> GetIngredient(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);

        if (ingredient == null) return NotFound();

        return Ok(ingredient.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<IngredientDto>> CreateIngredient(IngredientDto ingredientDto)
    {
        // 1. Convert the "Translator" (DTO) into a "Database Object" (Entity)
        var ingredient = new Ingredient
        {
            Name = ingredientDto.Name,
            CostPerUnit = ingredientDto.CostPerBaseUnit,
            BaseUnit = RecipeCostAPI.Models.UnitType.Gram, // Defaulting for now
            CostPerBaseUnit = ingredientDto.CostPerBaseUnit
        };

        // 2. Tell the database to track this new item
        _context.Ingredients.Add(ingredient);

        // 3. Save the changes to PostgreSQL
        await _context.SaveChangesAsync();

        // 4. Send back a new DTO with the ID the database just created
        var resultDto = new IngredientDto
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            BaseUnit = ingredient.BaseUnit.ToString(),
            CostPerBaseUnit = ingredient.CostPerBaseUnit
        };

        return CreatedAtAction(nameof(GetIngredient), new { id = ingredient.Id }, resultDto);
    }
}