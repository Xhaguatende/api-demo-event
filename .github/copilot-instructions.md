# GitHub Copilot Instructions

All AI-agent guidance for this repository lives in [`.ai/`](../.ai/README.md).
Treat that folder as the single source of truth. Do not duplicate rules here.

## Load Order

Before making changes, read files in this order and apply the most specific rule
that matches the task:

1. **`.ai/skills/*.md`** — task playbook, if one matches (e.g. adding a
   command, adding an entity).
2. **`.ai/domains/*.md`** — rules for the layer(s) being touched
   (`api`, `application`, `domain`, `infrastructure`).
3. **`.ai/core/*.md`** — universal rules (`engineering.md`, `workflow.md`,
   `dotnet.md`).

Within the same tier, the more specific rule wins.

## Editing Policy

- Update `.ai/*` first. Do not add parallel instruction sets to `.github/`,
  the solution root, or individual projects.
- Keep this file minimal — it exists only to point Copilot at `.ai/`.
- Current architecture is a 4-layer solution (`Api -> Application -> Domain`,
  `Infrastructure` implementing Application ports) with in-memory mock
  repositories and MediatR commands/queries. Detailed rules still live in
  `.ai/`.
