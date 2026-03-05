RecipeCostAPI 🍳
A .NET 8 Web API built to calculate and manage the costs of culinary recipes. This project uses PostgreSQL for data storage and Entity Framework Core for ORM, all containerized with Docker.

🚀 Features
Ingredient Management: Track costs based on specific units (Gram, Kilogram, etc.).

Recipe Costing: Automatic calculation of total recipe costs and cost-per-serving.

DTO Mapping: Clean separation between Database Models and API Response shapes.

Containerized Infrastructure: Ready-to-go database and cache layers using Docker Compose.

🛠️ Tech Stack
Framework: .NET 8.0

Database: PostgreSQL 16 (via Docker)

ORM: Entity Framework Core

Containerization: Docker & Docker Compose

🏁 Getting Started
Prerequisites
.NET 8 SDK

Docker Desktop

Entity Framework Tools (Run dotnet tool install --global dotnet-ef)

Installation & Setup
Clone the repository:

Bash
git clone <your-repo-url>
cd RecipeCostAPI
Restore NuGet Packages:

Bash
dotnet restore
Spin up the Infrastructure:
Ensure Docker is running, then start the PostgreSQL container:

Bash
docker-compose up -d
Apply Database Migrations:
This will create the tables in your Docker container:

Bash
dotnet ef database update
Run the API:

Bash
dotnet run
The API will be available at http://localhost:5000 (or the port specified in your launchSettings.json).

📂 Project Structure
Models/: Database entities (Recipe, Ingredient, RecipeIngredient).

DTOs/: Data Transfer Objects for API requests and responses.

Mappers/: Extension methods to convert Entities to DTOs.

Data/: AppDbContext and Entity Framework configurations.

Services/: Business logic (Pricing calculations, etc.).

⚠️ Troubleshooting
Port Conflicts
If you have a local instance of PostgreSQL running on port 5432, you may need to stop it or change the mapping in docker-compose.yml to 5433:5432.

To stop local Postgres (Windows): Stop-Service -Name postgresql*

Authentication Errors
If you see password authentication failed, ensure your appsettings.json connection string matches the POSTGRES_PASSWORD defined in docker-compose.yml.

📝 License
This project is licensed under the MIT License.

Would you like me to add a "API Endpoints" section to this README once we've finished building out your Controllers?
