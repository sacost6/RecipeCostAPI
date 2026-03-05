using Microsoft.EntityFrameworkCore;
using RecipeCostAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Get connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add DbContext using Npgql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Services Area 
// Define a string variable for the name of our policy
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add CORS services to the cotainer and define a policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
                      
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==========================================
// 2. THE MIDDLEWARE PIPELINE (Add this after builder.Build)
// ==========================================
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANT: app.UseCors must go here!
// Must be placed AFTER app.UseHttpsRedirection() and BEFORE app.MapGet() (or any other MapXXX) and BEFORE auth
app.UseCors(MyAllowSpecificOrigins);
 

app.Run();