# .NET Rules

Universal .NET/C# rules for this repository.

## File Header

- Every `.cs` file starts with the standard copyright header block used
  throughout the codebase, followed by a blank line before the namespace:

  ```csharp
  // -------------------------------------------------------------------------------------
  //  <copyright file="FileName.cs" company="{Company Name}">
  //    Copyright (c) {Company Name}. All rights reserved.
  //  </copyright>
  // -------------------------------------------------------------------------------------

  namespace ApiDemo.Layer.Feature;
  ```

- `file` in the `<copyright>` tag must match the actual file name.

## Style

- Use **file-scoped namespaces**.
- `using` directives go **inside** the file-scoped namespace, after a blank
  line following the namespace declaration.
- **Prefer the shortest `using` form the compiler resolves.** Sibling and
  parent namespaces under `ApiDemo.*` are reachable with short `using`
  directives (for example `using Domain.Events;` inside
  `ApiDemo.Application`, or `using Base;` inside
  `ApiDemo.Api.Controllers` for `ApiDemo.Api.Controllers.Base`). Do not
  rewrite these to fully qualified form as a "standards fix". Only fully
  qualify when the compiler cannot resolve the short form or two shortcuts
  collide.
- Group `using` directives with `System.*` first, then others; sort
  alphabetically within each group.

## Constructors

- Use **classic constructors** for any class with injected dependencies
  (handlers, controllers, services, pipeline behaviours). Assign to
  `_camelCase` `readonly` fields. Do not use primary constructors for
  dependency injection.
- `record` command/response types may use record positional syntax
  (`public record DeleteEventCommand(Guid Id) : IRequest<...>;`) — that is
  value/data shape, not injection, and both styles already coexist in
  `Commands/*` (positional records and property-bag records/classes).
  Prefer the positional form for simple commands with few properties, and
  the property-bag form when the command has optional/settable properties
  (mirrors existing `UpsertEventCommand`).

## Documentation Comments

- Public controller classes and public members carry XML doc comments
  (`<summary>`, `<param>`, `<returns>`) — see
  `ApiDemo.Api/Controllers/EventsController.cs`. Follow the same pattern
  for new public controller actions. Application/Domain/Infrastructure
  types do not require XML docs unless the project already has them.

## Naming

- Private fields: underscore prefix (`_myField`).
- Interfaces: `I` prefix (`IEventRepository`, `ITokenService`).
- Async methods: `Async` suffix, including MediatR-adjacent helpers.
- Follow Microsoft identifier-name conventions for everything else.

## Async and Cancellation

- Prefer async I/O.
- Include `CancellationToken cancellationToken = default` on repository and
  service interface methods, matching the existing signatures.
- Do not swallow `OperationCanceledException`.

## Nullability

- `<Nullable>enable</Nullable>` is the default in every project. Keep
  nullable intent explicit.
- Use `= default!` for properties that are always set by the framework or
  by the caller before use (existing convention in DTOs/commands), and `?`
  for genuinely optional values (`Guid? Id` on `UpsertEventCommand`).

## GUIDs and Time

- This codebase currently uses `Guid.NewGuid()` and `DateTime.UtcNow`
  directly (no injected clock abstraction). Follow the existing convention
  — do not introduce `IClock` or `Guid.CreateVersion7()` unless the task
  explicitly asks you to add that abstraction across the codebase.

## Settings

- Bind configuration to strongly-typed POCOs (`AuthSettings`, `ApiSettings`
  in the Blazor client). Validation for a settings POCO lives in a
  `FluentValidation.AbstractValidator<T>` next to it and is wired via
  `.ValidateFluently().ValidateOnStart()` — see
  `ApiDemo.Api/Extensions/OptionsBuilderExtensions.cs` and
  `Infrastructure/Settings/AuthSettings.cs` +
  `Infrastructure/Validators/AuthSettingsValidator.cs`.

## Interfaces

- Any new class intended for injection or mocking gets an interface in the
  layer that defines the contract (`Application/Repositories`,
  `Application/Services`), implemented in `Infrastructure`.
- Do not introduce interfaces speculatively for classes that are never
  substituted or injected.

## Dead Code

- Remove unused `using` directives before handoff.
- Remove unused `private` fields, properties, and methods before handoff.
- Do not leave commented-out code behind.
