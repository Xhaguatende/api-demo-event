# Domain Layer

Rules for changes inside `src/ApiDemo.Domain`.

Read together with `.ai/core/*.md`.

## Boundaries

- Domain has **no dependency on any other project in the solution**.
- The only package reference is
  `Microsoft.AspNetCore.Cryptography.KeyDerivation`. Do not add MongoDB,
  ASP.NET hosting, MediatR, FluentValidation, or DI-container packages
  here — those belong to `Application` or `Infrastructure`.
- If a change needs a framework package, the change belongs in another
  layer.

## Entities

- Every entity inherits `Entity<TId>` (`Primitives/Entity.cs`), which
  supplies `Id`, `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy`,
  `Version`, and equality-by-id semantics.
- Constructors are **public**, taking the required state and forwarding the
  id to `base(id)` — see `Events/Event.cs`. This repo does not use private
  factory methods; do not introduce them without discussing the change,
  since Infrastructure mock repositories construct entities directly.
- State-changing behaviour is exposed as intention-revealing instance
  methods with private setters on the properties they touch (`Update(...)`
  on `Event`). Do not add public setters to entity properties.

## Value Objects

- Value objects inherit `ValueObject` (`Primitives/ValueObject.cs`) and
  implement `GetEqualityComponents()` for structural equality — see
  `Accounts/ValueObjects/AccountId.cs`.
- Validate inputs in the value object's constructor; throw
  `ArgumentException`/`ArgumentNullException` (or a `DomainException`
  subtype) on invalid input.

## Exceptions

- Business/domain-rule violations derive from `DomainException`
  (`Exceptions/DomainException.cs`). `NotFoundException` is the shared
  "not found" base; feature-specific exceptions
  (`EventNotFoundException`, `MaxNumberEventsException`) live under the
  owning feature folder (`Events/Exceptions/`), not in the shared
  `Exceptions/` folder.
- `ApiDemo.Api/Handlers` maps these to HTTP responses. When you add a new
  exception type that should produce a distinct HTTP status, add or update
  the matching `IExceptionHandler` in the Api layer in the same change —
  see `.ai/domains/api.md`.

## Result / Error Pattern

- `Shared/Result<T>` and `Shared/Error` express an **expected business
  outcome the caller must branch on** (currently used by `SignIn`). Use
  this pattern, not an exception, when a "no" is a normal, anticipated
  outcome rather than a broken invariant.
- Do not mix the two: a method either throws for exceptional/invariant
  failures or returns `Result<T>` for expected refusals — not both for the
  same failure mode.

## Prohibited

- No project references out of `ApiDemo.Domain`.
- No public setters on entity state — mutate through named methods.
- No `catch (Exception)` blocks in domain code.

## Verification

- `dotnet build` at the solution root must be clean.
- There are no Domain unit tests today. If you add the first
  `ApiDemo.Domain.UnitTests` project, record the test command here and in
  `.ai/core/engineering.md`.
