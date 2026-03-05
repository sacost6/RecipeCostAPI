# RecipeCostAPI 🍳

A .NET 8 Web API designed to calculate and manage the costs of culinary recipes. This project uses **PostgreSQL** for data storage and **Entity Framework Core** for ORM, fully containerized with **Docker**.



---

## 🚀 Features
* **Ingredient Management:** Track costs based on specific units (Gram, Kilogram, etc.).
* **Recipe Costing:** Automatic calculation of total recipe costs and cost-per-serving.
* **DTO Mapping:** Clean separation between Database Models and API Response shapes.
* **Containerized Infrastructure:** Ready-to-go database and cache layers using Docker Compose.

---

## 🛠️ Tech Stack
* **Framework:** .NET 8.0
* **Database:** PostgreSQL 16 (via Docker)
* **ORM:** Entity Framework Core (Npgsql)
* **Containerization:** Docker & Docker Compose

---

## 🏁 Getting Started

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)
* [EF Core Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) (`dotnet tool install --global dotnet-ef`)
