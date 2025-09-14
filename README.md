TaskTracker API

A task management REST API built with .NET 8, Entity Framework Core, SQL Server, and JWT authentication.
The service provides CRUD operations for tasks and users, background task status updates, and Swagger-based API documentation.

🚀 Setup Instructions
Prerequisites

Docker
 installed

Docker Compose
 installed

(Optional for local dev) .NET 8 SDK

1. Clone the repository
git clone https://github.com/nzuza-siyabonga-sanele/Assessment.git
cd Assessment

2. Build and run with Docker Compose
docker-compose up --build


This will start:

SQL Server 2022 container (listening on 1433)

TaskTracker API container (listening on http://localhost:5000)

3. Access the API

Swagger UI: http://localhost:5000/swagger

Health check: http://localhost:5000/swagger/v1/swagger.json

4. Development hot reload (optional)

With the docker-compose.override.yml:

docker-compose up --build


Source changes will auto-reload inside the container using dotnet watch run.

🏗 Core Design Decisions

Multi-layer architecture:

Core: domain interfaces & entities

Infrastructure: EF Core DbContext + repositories

Services: business logic + background scheduler

Api: entrypoint, controllers, configuration

EF Core Code-First:
Migrations are applied at startup (context.Database.Migrate()), ensuring the schema evolves with code.

JWT-based Authentication:
Secure access control using JWT Bearer tokens with IAuthService handling login/token generation.

Repository Pattern:
IRepository<T> provides generic CRUD, with specialized repositories for Task and User.

Background Scheduler:
TaskStatusUpdateService runs periodically to check and update overdue tasks, offloading time-based logic from controllers.

Swagger/OpenAPI:
Self-documenting API with JWT support in Swagger UI.

⚖️ Trade-offs Made

Database Seeding at Startup
Chose to seed automatically for developer convenience.
Trade-off: Adds complexity at startup and can fail if DB isn’t ready. Mitigated with retry logic.

Direct SQL Server in Docker Compose
Easy for local development.
Trade-off: In production, a managed DB service (Azure SQL, RDS, etc.) would be preferable.

Use of Repository Pattern
Adds abstraction and testability.
Trade-off: Slight boilerplate overhead versus directly using DbContext.

In-Container HTTPS Redirection
Currently uses UseHttpsRedirection() but runs HTTP in Docker.
Trade-off: Simplifies container setup. Production TLS termination would be handled by a reverse proxy (nginx/Traefik).

🔮 What We’d Improve with More Time

Resilient DB Connection Handling
Implement exponential backoff or Polly retries for DB migration/seeding.

Production-Grade TLS
Add nginx reverse proxy container to terminate TLS and forward requests securely.

Advanced Scheduling
Replace simple background loop with Quartz.NET or Hangfire for flexible cron-like scheduling.

Observability
Integrate structured logging (Serilog), metrics (Prometheus), and tracing (OpenTelemetry).

Unit & Integration Tests
Add xUnit test suite covering repositories, services, and controllers.

Role-based Authorization
Expand JWT claims to support Admin vs User permissions.

🧪 Testing & Observing the Scheduler

The TaskStatusUpdateService runs periodically to mark tasks as Overdue when their due dates pass.

How to Test:

Create a new task with a past due date:

POST /info/tasks
Content-Type: application/json
Authorization: Bearer <your-jwt>

{
  "title": "Past due task",
  "description": "This should become overdue",
  "dueDate": "2024-01-01T00:00:00Z",
  "status": "New"
}


Wait for the scheduler interval (configured in TaskStatusUpdateService).

Fetch tasks:

GET /info/tasks
Authorization: Bearer <your-jwt>


Observe that the task’s Status has been updated to Overdue.

How to Observe Internally:

Logs: The scheduler logs when it runs updates.

Database: Query the Tasks table directly (Status should change).

API response: Confirm via /info/tasks endpoint.

📂 File Locations

API Entrypoint: Senior_Developer_Assessment.Program.cs

DbContext: Senior_Developer_Assessment/Data/DataDbContext.cs

Repositories: Senior_Developer_Assessment/Models/Repositories/

Services: Senior_Developer_Assessment/Services/

Background Service: Senior_Developer_Assessment/Services/TaskStatusUpdateService.cs

Migrations: Senior_Developer_Assessment/Migrations/

Dockerfile: Senior_Developer_Assessment/Dockerfile

Compose: docker-compose.yml at repo root
