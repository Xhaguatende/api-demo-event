# AI Instruction System

This folder is the **single source of truth** for AI-agent instructions in
this repository.

## Goals

- Keep guidance in one place to avoid drift and duplication.
- Share the same rules across every AI agent used against this codebase.
- Keep agent-specific bootstrap files (in `.github/`, `CLAUDE.md`, etc.)
  minimal, delegating to this folder.

## Structure

```
.ai/
  README.md
  core/          Rules that always apply, regardless of task or layer.
    engineering.md
    workflow.md
    dotnet.md
  domains/       Rules that apply when touching a specific layer or concern.
                 One file per project. Rarely changes.
  skills/        Task playbooks and checklists. One file per task type.
                 Added on demand.
```

## Content Rules

- **`core/`** — universal rules. Language, engineering principles, workflow.
- **`domains/`** — per-layer rules. One file per project
  (`ApiDemo.Api`, `ApiDemo.Application`, `ApiDemo.Domain`,
  `ApiDemo.Infrastructure`).
- **`skills/`** — per-task rules. Added when the task type recurs and
  benefits from a checklist. Do not create empty skill files.

## Editing Policy

- Update `.ai/*` files first. Do not maintain parallel instruction sets
  elsewhere in the repository.
- Prefer a small number of stable files over many volatile ones.
- If a rule appears in more than one file, decide which file owns it and
  reference it from the other.
- Skills reference domains; domains reference core. Never the reverse.

## Precedence

When guidance conflicts, apply this order:

1. The most specific applicable `skills/*.md` file.
2. The applicable `domains/*.md` file.
3. `core/*.md`.

Within the same tier, the more specific rule wins.
