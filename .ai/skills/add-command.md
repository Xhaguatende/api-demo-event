# Skill: Add a new command or query

Use when adding a new MediatR command/query to `ApiDemo.Application`, end to
end through `ApiDemo.Domain`, `ApiDemo.Infrastructure`, and
`ApiDemo.Api`.

Read first: `.ai/core/engineering.md`, `.ai/core/dotnet.md`,
`.ai/domains/domain.md`, `.ai/domains/application.md`,
`.ai/domains/infrastructure.md`, `.ai/domains/api.md`.

## Scope

This skill covers a typical vertical slice: one new command or query that
reads/writes one entity. It does not cover adding a brand-new aggregate
(see `add-entity.md`) or a new cross-cutting pipeline behaviour.

## Checklist

1. **Decide the slice name and shape.**
   - Folder: `ApiDemo.Application/Commands/<VerbNoun>/` (for example
     `Commands/CancelEvent/`), mirroring existing slices
     (`DeleteEvent`, `UpsertEvent`, `GetEvents`).
   - Decide command vs. query: both are `IRequest<TResponse>` records in
     the same `Commands/` folder — there is no separate `Queries/` folder
     in this repo.

2. **Domain change, if any.**
   - If the operation requires new entity behaviour, add an
     intention-revealing method to the entity (private-set properties,
     public method) per `.ai/domains/domain.md`. Do not add public
     setters.
   - If the operation can be refused for a business reason the entity must
     validate, throw the appropriate `DomainException` subtype (new one
     under the entity's own `Exceptions/` folder if none fits) — do not
     invent a new pattern.

3. **Repository/service change, if any.**
   - If a new repository method is needed, add it to the interface in
     `Application/Repositories/` first, then implement it in the matching
     `Infrastructure/Repositories/*Mock.cs` class.

4. **Create the command/query record.**
   - `<Name>Command.cs`: positional record for simple shapes, property-bag
     record/class for optional/many properties.
   - `<Name>CommandResponse.cs`: the shape returned to the caller. Keep it
     separate from the Domain entity — do not return the entity directly.

5. **Create the validator, if the input needs validation.**
   - `<Name>CommandValidator.cs` : `AbstractValidator<TCommand>` in the
     same folder. Skip it only when every field is fully constrained by
     the route (mirrors `DeleteEventCommand`, `GetEventByIdCommand`).

6. **Create the handler.**
   - `<Name>CommandHandler.cs`: `IRequestHandler<TCommand, TResponse>`,
     classic constructor injection, `readonly` fields.
   - Body: load via repository, call the domain method (if any), map to
     `<Name>CommandResponse`, persist via the repository. Nothing else.

7. **Wire the Api controller action.**
   - Add (or extend) a controller action in the matching
     `ApiDemo.Api/Controllers/<Resource>Controller.cs`.
   - Include XML doc comments, `[SwaggerResponse(...)]`,
     `[ProducesResponseType(...)]`, `[Produces(MediaTypeNames.Application.Json)]`,
     and the correct `[Http*]` attribute/route template, per
     `.ai/domains/api.md`.
   - The action body only builds the command/query and calls
     `_mediator.Send(...)`.

8. **Check the exception-handler mapping.**
   - If step 2 introduced a new exception type that isn't already caught
     by an existing `IExceptionHandler`, add a new handler and register it
     in `Program.cs` in the same commit — see `.ai/domains/api.md`.

9. **Cross-repo check.**
   - If the response shape or route changes, check whether
     `blazor-demo-event`'s service layer and models need a matching
     update (see `.ai/core/workflow.md` — Cross-Repository Context). Note
     this explicitly in your handoff even if you cannot edit that repo
     from here.

10. **Verify.**
    - `dotnet build` at the solution root — zero warnings, zero errors.
    - Manually exercise the new endpoint via Swagger
      (`/swagger`) since there are no automated tests yet.

## Do Not

- Do not add a `Queries/` folder — queries live in `Commands/<Name>/` like
  every other slice in this repo.
- Do not put validation logic inside the handler when a
  `FluentValidation` validator can express it.
- Do not return the Domain entity directly from a handler or controller —
  always map to a `<Name>CommandResponse`.
- Do not add ad-hoc `try`/`catch` in the controller — extend the
  `IExceptionHandler` pipeline instead.
