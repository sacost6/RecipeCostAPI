using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeCostAPI.Data;
using RecipeCostAPI.Mappers;
using RecipeCostAPI.DTOs;

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
}