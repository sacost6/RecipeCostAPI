using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeCostAPI.Data;
using RecipeCostAPI.Mappers;
using RecipeCostAPI.Services;

namespace RecipeCostAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPricingService _pricingService;

    public RecipesController(AppDbContext context, IPricingService pricingService)
    {
        _context = context;
        _pricingService = pricingService;
    }

    // GET: api/recipes/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecipe(int id)
    {
        var recipe = await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null) return NotFound();

        // Pass the pricing service to the mapper as we discussed earlier
        return Ok(recipe.ToDto(_pricingService));
    }
}