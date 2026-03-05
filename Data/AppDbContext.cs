using Microsoft.EntityFrameworkCore;
using RecipeCostAPI.Models;

namespace RecipeCostAPI.Data;

public class AppDbContext : DbContext {

    // The constructor (AppDbContext) is 
    // what the program.cs uses to pass 
    // the connection string to the DbContextOptions,
    // which is then used by EF Core to connect to the database.
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // These 'DbSets' represent the tables in our database.
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Configure the join table (many-to-many)
        // This tells EF that a RecipeIngredient is unique by its RecipeId + Ingredient
        modelBuilder.Entity<RecipeIngredient>()
            .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

        // 2. Optional: Configure Precision for Currency
        modelBuilder.Entity<Ingredient>()
            .Property(i => i.CostPerBaseUnit)
            .HasPrecision(18, 4); // Adjust as needed for your currency
    }
    }