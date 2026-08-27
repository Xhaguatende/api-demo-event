# Application Layer

Rules for changes inside `src/ApiDemo.Application`.

Read together with `.ai/core/*.md`.

## Boundaries

- Allowed dependencies: `ApiDemo.Domain`, `MediatR`, `FluentValidation`
  (+ `FluentValidation.DependencyInjectionExtensions`).
- **Never** reference `ApiDemo.Infrastructure` or `ApiDemo.Api`. Doing so
  inverts the layer graph.
- No infrastructure packages (MongoDB driver, ASP.NET hosting, HTTP
  clients). If a change wants one, it belongs in `ApiDemo.Infrastructure`.

## Vertical Slices

- Every command/query lives under `Commands/<Name>/` and contains:
  `<Name>Command.cs`, `<Name>CommandHandler.cs`, `<Name>CommandResponse.cs`,
  and — when the command carries input worth validating —
  `<Name>CommandValidator.cs` (see `UpsertEvent/`).
- Do not group by artefact kind (`Handlers/`, `Validators/`,
  `Responses/`); cohesion lives inside the slice folder.
- Queries are also modelled as `IRequest<TResponse>` records under
  `Commands/<Name>/` (there is no separate `Queries/` folder) — follow
  `GetEvents/` and `GetEventById/` as the reference shape for read
  operations.

## Command/Query Contract

- Commands and queries are records implementing `IRequest<TResponse>`.
  Use positional record syntax for simple shapes
  (`public record DeleteEventCommand(Guid Id) : IRequest<...>;`); use a
  property-bag record/class when the shape has optional or many
  properties (`UpsertEventCommand`).
- Handlers implement `IRequestHandler<TCommand, TResponse>`, are `public`
  (not `sealed` currently, follow existing files), and use classic
  constructor injection with `readonly` fields.
- A handler's body: load via repository/service, apply domain behaviour,
  translate to the `<Name>CommandResponse`, persist via the repository.
  Nothing else.
- Business refusals that must be branched on by the caller return
  `Result<T>` (see `SignIn`). Everything else that fails is an exception
  from the Domain layer, allowed to propagate to the Api's
  `IExceptionHandler` pipeline.

## Validators

- One `AbstractValidator<TCommand>` per command that needs validation,
  in the same slice folder.
- Registered by FluentValidation assembly scanning
  (`AddValidatorsFromAssembly(typeof(IAssemblyReference).Assembly)`) in
  `Extensions/ServiceCollectionExtensions.cs`. `ApiDemo.Infrastructure`'s
  `ValidationBehavior` runs them for every request in the MediatR
  pipeline — do not hand-call a validator from inside a handler.
- Not every command needs a validator (`DeleteEventCommand`,
  `GetEventByIdCommand`, `GetEventsCommand` have none because their inputs
  are fully constrained by the route). Add one whenever a command accepts
  free-form or optional input.

## Repositories and Services

- Interfaces for persistence live in `Repositories/` (`IEventRepository`,
  `IAccountRepository`); interfaces for cross-cutting services live in
  `Services/` (`ITokenService`, `IApplicationContextService`).
- Implementations live in `ApiDemo.Infrastructure`. Application never
  implements these interfaces itself.

## Composition

- `Extensions/ServiceCollectionExtensions.RegisterApplication` is the
  single composition entry point: registers MediatR from the Application
  assembly and scans validators from the same assembly.
- `IAssemblyReference` is the marker type for assembly scanning. Do not
  duplicate this pattern with a different marker.

## Prohibited

- No infrastructure packages or `using` directives.
- No HTTP-shaped types (`IActionResult`, `HttpContext`, status codes) —
  those belong in `ApiDemo.Api`.
- No direct persistence access (no MongoDB driver, no `List<T>` static
  stores) — that lives behind the repository interfaces in
  `ApiDemo.Infrastructure`.

## Verification

- `dotnet build` at the solution root must be clean.
- There are no Application unit tests today. If you add the first
  `ApiDemo.Application.UnitTests` project, record the test command here.
