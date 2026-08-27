# Core Engineering Rules

Universal engineering rules for this repository. Language-agnostic.

## Architecture

- Use **constructor injection** only. No service locator, no property
  injection, no ambient container lookups.
- Respect layer boundaries. Dependencies flow inward:
  `Api -> Application -> Domain`. `Infrastructure` implements ports declared
  by `Application` (repositories, services) and depends on `Application` +
  `Domain`. Never invert this without an explicit design decision.
- Prefer **explicit contracts** (interfaces, DTOs, typed boundaries) at
  layer seams.
- Prefer **pure/stateless helpers** unless instance state is required.

## Change Discipline

- Keep changes scoped to the requested task. Do not perform unrelated
  refactors in the same change.
- If you discover a defect directly coupled to the code you are changing,
  fix it. If it is unrelated, flag it and move on.
- **Reuse existing abstractions and patterns** before introducing new ones.
  This repo already has patterns for commands, repositories, validators,
  and exception handling — follow them rather than inventing alternatives.

## Errors and Failures

- Domain/business rule violations throw a `DomainException` subtype (for
  example `NotFoundException`, `EventNotFoundException`,
  `MaxNumberEventsException`). They are caught by the `IExceptionHandler`
  pipeline in `ApiDemo.Api/Handlers`, not by ad-hoc try/catch in handlers or
  controllers.
- Input validation failures are FluentValidation `ValidationException`,
  raised by `ValidationBehavior` in the MediatR pipeline — do not
  hand-validate inside a command handler when a validator can express the
  rule instead.
- The `Result<T>` / `Error` pattern (`ApiDemo.Domain/Shared`) is reserved for
  outcomes that are an expected business "no" the caller must branch on
  (currently `SignIn`). Do not throw for that kind of outcome; return
  `Result<T>` instead.
- No empty `catch` blocks. Do not swallow exceptions to keep flow going.

## Naming and Structure

- Follow existing naming and folder conventions of the project being
  touched. Do not import conventions from other projects unless the
  target project already uses them.
- **File path is authoritative.** Do not infer target location from the
  namespace. Place new files where the existing folder convention says
  they belong, then match the namespace to the folder.

## Validation

- Validate inputs at the boundary where they first enter a layer (command
  validators for Application input, model binding/data annotations for
  Blazor forms in the sibling `blazor-demo-event` repo).
- Do not re-validate the same input at every internal call site.
- Prefer failing early over defensive checks scattered through call chains.

## Dependencies

- Do not add packages "just in case". Every new package reference must be
  justified by the task at hand.
- Do not introduce a new tool, framework, or pattern to solve a problem
  the existing stack already solves (MediatR for commands/queries,
  FluentValidation for validation, `IExceptionHandler` for error mapping).
- Package versions follow the latest stable release. If a document pins
  an older version, update the document rather than downgrading.

## Verification

- Run the smallest targeted build and test command that covers the change.
- This repository currently has **no automated test projects**. Until one
  exists, verification is `dotnet build` at the solution root with zero
  warnings/errors. If you add the first test project for a layer, wire it
  into this file and the relevant `domains/*.md` verification section.
- Do not introduce new lint/build/test tooling unless the task requires it.
