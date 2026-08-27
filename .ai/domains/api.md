# Api Layer

Rules for changes inside `src/ApiDemo.Api`.

Read together with `.ai/core/*.md`.

## Boundaries

- The Api is the HTTP host: controllers, DTO binding (commands double as
  request bodies), exception handling, OpenAPI/Swagger, JWT auth.
- Allowed dependencies: `ApiDemo.Application`, `ApiDemo.Infrastructure`,
  `ApiDemo.Domain`, `Microsoft.AspNetCore.Authentication.JwtBearer`,
  `Swashbuckle.AspNetCore(.Annotations)`.
- Api is a leaf project. **Nothing depends on it.**

## Controllers

- Traditional ASP.NET Core MVC controllers (not Minimal APIs), one file
  per resource under `Controllers/`, inheriting `ApiDemoControllerBase`
  (`Controllers/Base/ApiDemoControllerBase.cs`).
- `[Route("api/[controller]")]` + `[ApiController]`; add `[Authorize]` at
  the controller level unless the resource is intentionally anonymous
  (see `AccountsController` vs `EventsController`).
- Actions inject `IMediator` via a classic constructor and do only:
  build the command/query, `await _mediator.Send(...)`, return
  `Ok(response)` (or the base class's `BadRequest(List<Error> errors)`
  helper for the `Result<T>` pattern). No business logic, no repository
  access, in a controller action.
- Action names keep the `Async` suffix
  (`SuppressAsyncSuffixInActionNames = false` in `Program.cs`) — it is
  intentionally visible in the OpenAPI operation id.
- Every action documents its response with XML doc comments
  (`<summary>`, `<param>`, `<returns>`), a `[SwaggerResponse(...)]`, and a
  matching `[ProducesResponseType(...)]` + `[Produces(MediaTypeNames.Application.Json)]` —
  keep all three in sync when adding or changing an action (see
  `EventsController`).

## Exception Handling

- Exception-to-HTTP mapping is done exclusively via ASP.NET Core
  `IExceptionHandler` implementations in `Handlers/`, registered in
  `Program.cs` with `services.AddExceptionHandler<T>()` in this order:
  `ValidationExceptionHandler` → `NotFoundExceptionHandler` →
  `GlobalExceptionHandler`.
- `ValidationExceptionHandler` catches `FluentValidation.ValidationException`
  → 400 with an errors array in `ProblemDetails`.
- `NotFoundExceptionHandler` catches `NotFoundException` (and subclasses
  from Domain) → 404.
- `GlobalExceptionHandler` is the catch-all → 500, logs the exception.
- Adding a new Domain exception type that should map to something other
  than 404/500 means adding a new `IExceptionHandler` and registering it
  in this same ordered list, in the same commit.
- Do not add `try`/`catch` in a controller action to work around this
  pipeline.

## Settings Validation

- `Extensions/OptionsBuilderExtensions.ValidateFluently<TOptions>()` is the
  single helper that wires a settings POCO's `FluentValidation` validator
  into `IValidateOptions<TOptions>`. Use it for every new settings class:
  `.Bind(...).ValidateFluently().ValidateOnStart()`.

## Program.cs Composition

- `Program.cs` uses top-level statements.
- Kestrel: `AddServerHeader = false`.
- MVC/JSON: `SuppressModelStateInvalidFilter = true` (validation errors are
  handled by the MediatR `ValidationBehavior` + `ValidationExceptionHandler`,
  not ASP.NET's automatic 400), `DefaultIgnoreCondition =
  JsonIgnoreCondition.WhenWritingNull`, `LowercaseUrls = true`.
- Composition calls, in order: `services.RegisterApplication()` then
  `services.RegisterInfrastructure()`. New cross-project composition calls
  go in `Program.cs` at this level, not scattered across controllers.
- Validators are discovered from the Infrastructure assembly:
  `AddValidatorsFromAssemblyContaining<ApiDemo.Infrastructure.IAssemblyReference>(ServiceLifetime.Singleton)`.
  Do not duplicate scanning from a different marker.

## Prohibited

- No business/domain logic in controllers.
- No direct repository or `DbContext`-equivalent access in controllers.
- No ad-hoc exception handling in controllers — extend the
  `IExceptionHandler` pipeline instead.
- No `[assembly: InternalsVisibleTo]` — Api is a leaf executable and there
  are no test projects consuming internals today.

## Verification

- `dotnet build` at the solution root must be clean.
- There are no Api integration tests today. If you add the first
  `ApiDemo.Api.*Tests` project (for example a
  `WebApplicationFactory`-based suite), record the test command here.
- Any change to `Program.cs`, exception handlers, or controller routes
  should be manually smoke-tested against Swagger (`/swagger`) until
  automated tests exist.
