# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

FamUnion is a family reunion management platform. It is a monorepo containing a .NET 10 / ASP.NET Core REST API backend and a React Native mobile frontend, backed by PostgreSQL.

## Commands

### Backend (.NET)

Run all commands from the repo root unless otherwise noted.

```bash
# Build
dotnet build src/FamUnion.sln

# Build (Release)
dotnet build --configuration Release src/FamUnion.sln

# Run tests
dotnet test src/FamUnion.sln

# Run a specific test project
dotnet test src/FamUnion.Core.Tests/FamUnion.Core.Tests.csproj

# Run the API
dotnet run --project src/FamUnion.Api/FamUnion.Api.csproj
```

### Frontend (React Native)

Run all commands from the `fe/` directory.

```bash
# Install dependencies (use these flags to match CI)
npm ci --legacy-peer-deps --ignore-scripts

# Run tests (non-watch, matches CI)
npm test -- --watchAll=false --no-coverage

# Run a single test file
npm test -- --watchAll=false path/to/file.test.js

# Lint
npm run lint

# Start Metro bundler
npm start

# Run on Android / iOS
npm run android
npm run ios
```

### Database

Migrations are managed with Flyway. The migration files live in `src/FamUnion.Db/Postgres/`:
- `V1__tables.sql` — table definitions
- `V2__functions.sql` — stored procedures (all `sp_*` functions)
- `V3__seed.sql` — seed data (entity types)

## Architecture

### Backend project layout

```
src/
├── FamUnion.Api/          # ASP.NET Core entry point — controllers, Startup.cs, Dockerfile
├── FamUnion.Core/         # Domain layer — models, interfaces, DTOs, auth, validation
├── FamUnion.Infrastructure/ # Data/service layer — repositories, services, DbAccess<T>
├── FamUnion.Core.Tests/   # xUnit tests (covers Core only)
├── FamUnion.Db/           # SQL schema & Flyway Postgres migrations
├── FamUnion.WebAuth/      # Separate ASP.NET Core app for Auth0 OpenID Connect login
└── FamUnion/              # Legacy MVC project (not used by the mobile client)
```

### Dependency flow

```
FamUnion.Api  →  FamUnion.Core  ←  FamUnion.Infrastructure
                                        ↓
                                   PostgreSQL (via Dapper + Npgsql)
```

`FamUnion.Core` defines all interfaces (`IReunionRepository`, `IReunionService`, …). `FamUnion.Infrastructure` implements them. `FamUnion.Api` wires everything together in `Startup.cs` via constructor injection — repositories are registered with the connection string, services as `Transient`.

### Data access pattern

All database calls go through `DbAccess<T>` (`src/FamUnion.Infrastructure/DbAccess.cs`). It opens an `NpgsqlConnection` and calls Dapper. Every query targets a PostgreSQL function (stored proc convention):

```csharp
const string sql = "SELECT * FROM sp_get_reunion_by_id(@id)";
return await ExecuteStoredProc(sql, ParameterDictionary.Single("id", id));
```

`ParameterDictionary` is the custom parameter builder. `DefaultTypeMap.MatchNamesWithUnderscores = true` is set globally so Dapper maps `snake_case` columns to `PascalCase` C# properties automatically.

### Authentication & authorization

- **Provider**: Auth0 (JWT Bearer). Config keys live under `AppAuth` and `IdentityAuth` in `appsettings.json`.
- **Policies** (`src/FamUnion.Core/Auth/AppClaims.cs`): three tiers — `Access` (view), `Manage` (reunion organizer), `Admin` (system). Policies check the `permissions` claim in the JWT.
- `Access` is configured as the `DefaultPolicy`, meaning it applies when an endpoint uses `[Authorize]` without specifying a policy name. Endpoints are **anonymous by default** unless decorated with `[Authorize]` (or `FallbackPolicy` is set in Startup). Currently no controllers carry `[Authorize]`, so all endpoints are open. Use `[Authorize(Policy = AppClaimPolicy.Manage)]` for organizer-only endpoints.
- `FamUnion.WebAuth` is a separate web app that handles the Auth0 login/callback flow and is not part of the API.

### API surface

Controllers under `src/FamUnion.Api/Controllers/`:
- `ReunionsController` — CRUD + organizer management + cancel
- `EventsController` — events scoped to a reunion
- `AttendeesController` — invites and RSVP
- `UsersController` — user lookup/save
- `/health` — Npgsql health check endpoint

Swagger UI is served at `/swagger` in all environments.

### Frontend architecture

```
fe/
├── src/
│   ├── api/client.js      # Thin fetch wrapper (api.get / api.post / api.put)
│   ├── services/          # One file per resource (reunionService, eventService, …)
│   └── screens/           # One file per screen; uses services for data
├── __tests__/             # Jest tests
└── __mocks__/@env.js      # Mocks react-native-dotenv (@env) for tests
```

The frontend reads `APIURL` from a `.env` file (see `fe/__mocks__/@env.js` for the mock used in tests). All HTTP calls go through `api/client.js`, which normalises the base URL and throws on non-2xx responses.

### Database schema (key tables)

| Table | Purpose |
|---|---|
| `app_user` | Auth0-linked user accounts (`user_id` = Auth0 sub) |
| `reunion` | A family reunion event |
| `events` | Activities within a reunion |
| `address` | Polymorphic addresses linked via `entity_id` + `entity_type` |
| `reunion_organizer` | Many-to-many: users with organizer role on a reunion |
| `reunion_invite` | Invitations tracked by email + RSVP status |
| `entity_type` | Enum table (1 = Reunion, 2 = Event, etc.) |

All tables carry `created_by`, `created_date`, `modified_by`, `modified_date` audit columns and an `is_active` soft-delete flag.

## Configuration

### Backend

`src/FamUnion.Api/appsettings.json` is **tracked in git** — it contains only placeholder empty strings and must never have real secrets committed to it.

For local development, put secrets in one of these two git-safe locations:

**Option A — `appsettings.Development.json`** (git-ignored):
```json
{
  "ConnectionStrings": { "FamUnionDb": "Host=localhost;Port=5432;Database=famunion;Username=...;Password=..." },
  "AppAuth":      { "Domain": "...", "ClientId": "...", "ClientSecret": "...", "Audience": "..." },
  "IdentityAuth": { "Domain": "...", "ClientId": "...", "ClientSecret": "...", "Audience": "..." }
}
```

**Option B — .NET user-secrets** (preferred):
```bash
dotnet user-secrets --project src/FamUnion.Api set "AppAuth:Domain" "..."
dotnet user-secrets --project src/FamUnion.Api set "AppAuth:ClientSecret" "..."
dotnet user-secrets --project src/FamUnion.Api set "IdentityAuth:ClientSecret" "..."
# etc.
```

### Frontend

Create `fe/.env` with:
```
APIURL=http://localhost:5000
```

## CI / CD

GitHub Actions runs on PRs targeting `develop` or `main`:
- `.github/workflows/dotnetcore.yml` — `dotnet build --configuration Release`
- `.github/workflows/dotnetcoretest.yml` — `dotnet test`
- `.github/workflows/react-native-test.yml` — `npm ci --legacy-peer-deps --ignore-scripts && npm test`

PRs use the template at `.github/pull_request_template.md` (JIRA link + brief description).
