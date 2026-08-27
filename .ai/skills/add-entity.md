# Skill: Add a new entity

Use when adding a brand-new entity/aggregate to `ApiDemo.Domain`, with its
repository, mock implementation, and initial commands.

Read first: `.ai/core/engineering.md`, `.ai/core/dotnet.md`,
`.ai/domains/domain.md`, `.ai/domains/application.md`,
`.ai/domains/infrastructure.md`.

## Scope

This skill covers the plumbing needed before any individual command/query
can be added (see `add-command.md` for the per-operation slice work):

- The entity class
- Its repository interface and mock implementation
- DI registration

## Checklist

1. **Decide the entity name, id type, and folder.**
   - Folder: `ApiDemo.Domain/<Feature>/` (mirrors `Events/`, `Accounts/`).
   - Id: reuse `Guid` via `Entity<Guid>` unless the id is naturally a
     value object (see `AccountId` under `Accounts/ValueObjects/`) — only
     introduce a value-object id when the id has structure or invariants
     beyond "is a Guid".

2. **Create the entity.**
   - Inherit `Entity<TId>` (`Primitives/Entity.cs`).
   - Public constructor taking the required state, forwarding the id to
     `base(id)`.
   - State-changing behaviour as public methods with private-set
     properties (`Update(...)` pattern from `Events/Event.cs`). No public
     setters.
   - If the entity needs its own refusal exceptions, put them under
     `<Feature>/Exceptions/`, inheriting `DomainException` (or the shared
     `NotFoundException` for the "not found" case).

3. **Create the repository interface.**
   - `ApiDemo.Application/Repositories/I<Entity>Repository.cs` — methods
     named `GetByIdAsync`, `GetAllAsync`, `UpsertAsync`, `DeleteAsync` as
     applicable, each with `CancellationToken cancellationToken = default`.

4. **Create the mock repository implementation.**
   - `ApiDemo.Infrastructure/Repositories/<Entity>RepositoryMock.cs`,
     backed by a `static List<T>` (or equivalent), implementing the
     interface from step 3.
   - Stamp audit fields (`CreatedAt`, `CreatedBy`, `UpdatedAt`,
     `UpdatedBy`) using `DateTime.UtcNow` and the injected
     `IApplicationContextService`, matching `EventRepositoryMock`.
   - Enforce any deliberate constraints (e.g. a max-count guard) by
     throwing a Domain exception, not by silently truncating data.

5. **Register the repository.**
   - Add to `ApiDemo.Infrastructure/Extensions/ServiceCollectionExtensions.AddRepositories`
     (`services.AddScoped<I<Entity>Repository, <Entity>RepositoryMock>();`).

6. **Add the first command/query slice.**
   - Follow `add-command.md` for the first operation (typically
     `Get<Entity>ById` and `Upsert<Entity>` or `Create<Entity>`).

7. **Verify.**
   - `dotnet build` at the solution root — zero warnings, zero errors.
   - Manually exercise the new endpoint(s) via Swagger since there are no
     automated tests yet.

## Do Not

- Do not add persistence technology beyond the existing in-memory mock
  pattern as an incidental part of this skill — that is a separate,
  deliberate architectural change (see `.ai/domains/infrastructure.md`).
- Do not give the entity public setters — use named behaviour methods.
- Do not skip the repository interface and reach for a concrete
  Infrastructure type from Application.
