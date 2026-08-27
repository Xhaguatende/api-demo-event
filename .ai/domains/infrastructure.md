# Infrastructure Layer

Rules for changes inside `src/ApiDemo.Infrastructure`.

Read together with `.ai/core/*.md`.

## Boundaries

- Allowed dependencies: `ApiDemo.Application`, `ApiDemo.Domain`,
  `Microsoft.Extensions.DependencyInjection(.Abstractions)`,
  `Microsoft.Extensions.Options`, `Microsoft.IdentityModel.JsonWebTokens`,
  `FrameworkReference Microsoft.AspNetCore.App`.
- **Never** reference `ApiDemo.Api`. Doing so inverts the layer graph.
- `MongoDB.Driver` is currently referenced but **not used** anywhere in
  code. Do not start wiring real MongoDB persistence as a side effect of
  an unrelated task — if the task is specifically "replace the mock
  repositories with MongoDB", treat that as its own scoped change (new
  skill/ADR-worthy decision), not an incidental addition.

## Repositories

- One repository per aggregate/entity, implementing the Application-layer
  interface (`IEventRepository`, `IAccountRepository`).
- Current implementations are **in-memory mocks** backed by `static
  List<T>` fields (`EventRepositoryMock`, `AccountRepositoryMock`). Keep
  new repositories consistent with this shape until real persistence is
  introduced as a deliberate change.
- `EventRepositoryMock` enforces a hard cap of 15 events and throws
  `MaxNumberEventsException` when exceeded — preserve this invariant if
  you touch `UpsertAsync`.
- Repositories stamp `CreatedAt`/`CreatedBy`/`UpdatedAt`/`UpdatedBy` using
  `DateTime.UtcNow` and the injected `IApplicationContextService` for the
  current user — do not bypass this by setting audit fields elsewhere.

## Pipeline Behaviours

- `PipelineBehaviors/ValidationBehavior<TRequest, TResponse>` is the only
  MediatR behaviour today. It runs every registered
  `IValidator<TRequest>` and throws `FluentValidation.ValidationException`
  on failure — handled by `ValidationExceptionHandler` in the Api layer.
- New cross-cutting behaviours (logging, timing, etc.) are decorators
  registered the same way, via
  `services.AddTransient(typeof(IPipelineBehavior<,>), typeof(YourBehavior<,>))`
  in `Extensions/ServiceCollectionExtensions.RegisterInfrastructure`. Keep
  registration in that one file.

## Services

- `TokenService` (`ITokenService`) issues JWTs with
  `JsonWebTokenHandler`, reading `AuthSettings` via `IOptions<AuthSettings>`.
- `HttpApplicationContextService` (`IApplicationContextService`) reads the
  current user's email from `IHttpContextAccessor` claims. Both use
  `DateTime.UtcNow` directly — follow the existing convention (see
  `.ai/core/dotnet.md`).

## Settings

- A settings area is a POCO (`Settings/AuthSettings.cs`) with `init`- or
  `set`-able properties and no validation attributes, plus a sibling
  `FluentValidation.AbstractValidator<T>` in `Validators/`
  (`AuthSettingsValidator.cs`).
- Bind and validate at the Api composition root with
  `.Bind(...).ValidateFluently().ValidateOnStart()` — see
  `ApiDemo.Api/Extensions/OptionsBuilderExtensions.cs`. Infrastructure does
  not perform the binding itself.

## Composition

- `Extensions/ServiceCollectionExtensions.RegisterInfrastructure` is the
  single composition entry point: registers repositories, services, and
  the validation pipeline behaviour.
- `IAssemblyReference` is the marker used for assembly-scoped operations
  (e.g. `AddValidatorsFromAssemblyContaining<ApiDemo.Infrastructure.IAssemblyReference>`
  in `Program.cs`).

## Prohibited

- No HTTP-shaped types (`IActionResult`, `ProblemDetails`, status codes) —
  those belong in `ApiDemo.Api`.
- No project reference to `ApiDemo.Api`.
- No new persistence technology introduced silently — swapping the mock
  repositories for a real database is a deliberate, scoped change, not an
  incidental one.

## Verification

- `dotnet build` at the solution root must be clean.
- There are no Infrastructure unit/integration tests today. If you add the
  first `ApiDemo.Infrastructure.*Tests` project, record the test command
  here.
