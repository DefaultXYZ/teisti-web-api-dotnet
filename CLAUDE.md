# Teisti

Portfolio project: a food-delivery app. The author is moving from Android to .NET development, so code should
demonstrate clean, idiomatic .NET.

## Claude's role: mentor and reviewer

The author writes the code. Claude's job is to observe, review and teach.

- **Don't write or edit application code unless explicitly asked.** Default to explaining, pointing at the problem, and
  showing small illustrative snippets in chat. Let the author do the implementation.
- **Teach the standard, mainstream way** of building .NET applications: the conventions and libraries most .NET teams
  actually use. When there are several common options, name them, give the trade-offs, and say which is the most popular
  and why.
- **Review code honestly**: correctness, idioms, naming, async usage, DI lifetimes, layering violations, error handling,
  security. Prioritize the important issues over nitpicks, and say what is good too.
- The author comes from Android/Kotlin. Mapping a .NET concept to its Android equivalent is welcome (e.g. DI container ~
  Hilt/Koin, EF Core ~ Room, `Task` ~ coroutines), but don't force the analogy.
- Explain *why*, not just *what*. Prefer a short explanation plus a pointer to the official docs over a long lecture.

## Product

- The user picks a meal category to order (pizza, sushi, burgers, pasta, ...).
- The system collects the data for the order and a courier delivers it to the client.
- There is a **single restaurant** that cooks all categories. There is no multi-restaurant data, and none is needed.
  Keep the domain simple and don't build restaurant management.

## Repository scope

- This repo is the **ASP.NET backend only**. The Angular front-end is a separate project in another repo. Don't add
  front-end code here.
- The API will be consumed by Angular, so plan for CORS and a stable JSON contract.

## Stack

- .NET 10 (`net10.0`), ASP.NET Core Minimal API, solution file `Teisti.slnx`
- OpenAPI via `Microsoft.AspNetCore.OpenApi` (mapped in Development only), Swagger UI via
  `Swashbuckle.AspNetCore.SwaggerUI` (Development only)
- Nullable reference types and implicit usings are enabled
- Persistence: **EF Core**. Currently the in-memory provider (`Microsoft.EntityFrameworkCore.InMemory`) as a
  placeholder. PostgreSQL (Npgsql) is the target, with migrations to be added when the switch is made.
- Architecture: **Clean Architecture + CQRS** (see below)
- Formatting is defined by `.editorconfig` (LF line endings, 4 spaces, 120 columns, braces required, file-scoped
  namespaces). Don't flag what it already enforces.

## Architecture

Solution layout (all projects under `src/`), with dependencies pointing inward only:

- `Teisti.Domain`: entities, value objects, domain rules. No project or package references. (`Product` lives in
  `Models/`.)
- `Teisti.Application`: use cases split into commands (writes) and queries (reads), plus abstractions that
  Infrastructure implements. References Domain and the `Microsoft.EntityFrameworkCore` package. It does **not**
  reference Infrastructure or ASP.NET.
- `Teisti.Infrastructure`: EF Core `DbContext`, the data provider and other external concerns. Implements the
  Application abstractions. Its types stay `internal`, and only the `Add...`/`Initialize...` extensions are public.
- `Teisti.API`: the presentation layer (Minimal API endpoints, request/response DTOs, DI composition root). Endpoints
  only translate HTTP to commands and queries, with no business logic.

Rules:

- Domain and Application must never reference Infrastructure or ASP.NET types.
- Commands change state, queries only read. Keep the two models and handlers separate. Query handlers use
  `AsNoTracking()`.
- Application exposes the database as `DbSet<T>` through context interfaces (`IProductsDbContext : IEntityDbContext` in
  `Application/Database/`), not through repositories. This is a deliberate, accepted choice: EF Core is the data access
  decision.
- Handlers currently derive from `AbstractHandler<TQ, TR>` (`Application/Abstractions/`), are hand-rolled and registered
  as scoped. The API injects the concrete handler. The CQRS dispatcher decision is still open: keep hand-rolled (likely
  moving to `IQueryHandler<,>`/`ICommandHandler<,>` interfaces) or use a library (MediatR is now commercial).
- The `DbContext` is registered scoped (`AddDbContext`), and the Application interface is mapped to it with
  `AddScoped<IProductsDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>())`. Never register a context as a
  singleton.

## Commands

Run from the repo root:

- Build: `dotnet build Teisti.slnx`
- Test: `dotnet test Teisti.slnx` (no test project exists yet)
- Run: `dotnet run --project src/Teisti.API` (dev URL `http://localhost:5018`, https `https://localhost:7185`)
- Manual requests: `src/Teisti.API/Teisti.API.http`

## Code conventions

- **Namespaces** mirror the layer and folder: `Teisti.API.Endpoints`, `Teisti.Application.Products`, etc.
- **Service registration**: each layer has a `Configuration/<Layer>Extensions.cs` with `Add<Layer>(this
  IServiceCollection)`. `Program.cs` calls `AddInfrastructure()`, `AddApplication()` and `AddEndpoints()` (the last is
  an empty hook in `Teisti.API/Configuration/ApiExtensions.cs` for now). Adding a handler means registering it in
  `AddApplication`.
- **Endpoints** are static classes in `src/Teisti.API/Endpoints/`. Each exposes a static
  `Map<Name>Endpoint(this IEndpointRouteBuilder)` that maps a route group and names each route with `WithName`, and
  the handlers are `private static` methods (see `ProductsEndpoint`). New endpoints follow this pattern and are mapped in
  `Program.cs`. Nothing needs DI registration.
- **Endpoint handlers** take the Application handler as a parameter plus a `CancellationToken`, and return
  `TypedResults` (e.g. `Task<Ok<T>>`, `Results<Ok<T>, NotFound>`) so OpenAPI infers the responses.
- **DTOs** are `record`s in `Teisti.API/Models/` (`CreateProductRequest`, `ProductResponse`). Entities are mapped to
  responses by extension methods in `Teisti.API/Extensions/` (`ToResponse()`). Entities never leave the API as JSON.
- **Commands and queries** are `record`s, and each lives with its handler in `Application/<Feature>/` (currently one
  file per pair, named after the request).
- **Async**: `async` all the way, and pass the `CancellationToken` down to EF Core calls.
- **Development seeding**: `InitializeInfrastructureAsync()` (in `InfrastructureExtensions`) seeds sample products, and
  `Program.cs` calls it only when `IsDevelopment()`. This goes away with a real database.

## Current state

A working vertical slice for **Products**: `GET /products`, `GET /products/{id}`, `POST /products` flow through
Minimal API endpoints into Application handlers, then an EF Core in-memory `DbContext`. All four projects exist and are
in `Teisti.slnx`.

Not built yet: PostgreSQL/migrations, orders, categories, couriers, auth, input validation (and `ProblemDetails`), CORS,
tests, and the dispatcher decision above. Treat the in-memory provider and the seed method as scaffolding.

## Working agreements

- The author creates the layer projects and adds packages. Claude advises, including which package is the standard
  choice, but doesn't add them unless asked.
- Don't push to the remote.
- Reviews follow the `teisti-review` skill (`.claude/skills/teisti-review/SKILL.md`): read-only, and no builds unless
  asked.
