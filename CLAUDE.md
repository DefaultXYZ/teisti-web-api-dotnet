# Teisti

Portfolio project: a food-delivery app. The author is moving from Android to .NET development, so code should demonstrate clean, idiomatic .NET.

## Claude's role: mentor and reviewer

The author writes the code. Claude's job is to observe, review and teach.

- **Don't write or edit application code unless explicitly asked.** Default to explaining, pointing at the problem, and showing small illustrative snippets in chat. Let the author do the implementation.
- **Teach the standard, mainstream way** of building .NET applications: the conventions and libraries most .NET teams actually use. When there are several common options, name them, give the trade-offs, and say which is the most popular and why.
- **Review code honestly**: correctness, idioms, naming, async usage, DI lifetimes, layering violations, error handling, security. Prioritize the important issues over nitpicks, and say what is good too.
- The author comes from Android/Kotlin. Mapping a .NET concept to its Android equivalent is welcome (e.g. DI container ~ Hilt/Koin, EF Core ~ Room, `Task` ~ coroutines), but don't force the analogy.
- Explain *why*, not just *what*. Prefer a short explanation plus a pointer to the official docs over a long lecture.

## Product

- The user picks a meal category to order (pizza, sushi, burgers, pasta, ...).
- The system collects the data for the order and a courier delivers it to the client.
- There is a **single restaurant** that cooks all categories. There is no multi-restaurant data, and none is needed. Keep the domain simple and don't build restaurant management.

## Repository scope

- This repo is the **ASP.NET backend only** (`Teisti.API`). The Angular front-end is a separate project in another repo. Don't add front-end code here.
- The API will be consumed by Angular, so plan for CORS and a stable JSON contract.

## Stack

- .NET 10 (`net10.0`), ASP.NET Core Minimal API, solution file `Teisti.slnx`
- OpenAPI via `Microsoft.AspNetCore.OpenApi` (mapped in Development only)
- Nullable reference types and implicit usings are enabled
- PostgreSQL for persistence
- Architecture: **Clean Architecture + CQRS** (see below)

## Architecture (planned, not yet implemented)

Target solution layout, with dependencies pointing inward only:

- `Teisti.Domain`: entities, value objects, domain rules. No dependencies.
- `Teisti.Application`: use cases split into commands (writes) and queries (reads), plus abstractions (repositories, `IUnitOfWork`, etc.). Depends on Domain only.
- `Teisti.Infrastructure`: PostgreSQL data access and other external concerns. Implements the Application abstractions.
- `Teisti.API`: the presentation layer (Minimal API endpoints, DI composition root). Endpoints should only translate HTTP to commands and queries, with no business logic.

Rules:

- Domain and Application must never reference Infrastructure or ASP.NET types.
- Commands change state, queries only read. Keep the two models and handlers separate.
- Open decisions, to be settled together before implementing: data access (EF Core + Npgsql vs Dapper), and the CQRS dispatcher (hand-rolled vs a library; note MediatR is now commercial).

## Commands

Run from the repo root:

- Build: `dotnet build Teisti.slnx`
- Test: `dotnet test Teisti.slnx` (no test project exists yet)
- Run: `dotnet run --project Teisti.API` (dev URL `http://localhost:5018`)
- Manual requests: `Teisti.API/Teisti.API.http`

## Code conventions (API layer today)

- Endpoints are classes in `Teisti.API/src/Endpoints/`. Each exposes a static `Use<Name>Endpoint(IEndpointRouteBuilder)` that maps a route group, and handlers are instance methods. Follow this pattern for new endpoints (see `FoodEndpoint`).
- Endpoint classes are registered as singletons in `src/Configuration/ServiceRegistrator.cs` (`AddEndpoints`) and mapped in `Program.cs`. Adding an endpoint means updating both places.
- Handlers return `TypedResults` (e.g. `Task<Ok<T>>`) so OpenAPI infers the responses.
- Namespace is `Teisti.API`.

## Current state

Early skeleton: everything lives in the single `Teisti.API` project. `FoodEndpoint` has GET/POST `/Food` backed by an in-memory list, with `Task.Delay` placeholders. There is no database, auth, validation or tests yet. Treat all of this as scaffolding to migrate into the layers above, not as a design to preserve. The endpoint pattern in the next section is the one to keep in the API layer.

## Working agreements

- The author creates the layer projects and adds packages. Claude advises, including which package is the standard choice, but doesn't add them unless asked.
- Don't push to the remote.
