# Core Workflow

How an AI agent picks and applies guidance for a task in this repository.

## Mandatory Pre-Task Check

Before making any code change:

1. **Identify the task type** (adding a command/query, adding an entity,
   adding a page/service in the Blazor client, fixing a bug, writing
   documentation, etc.).
2. **Resolve applicable guidance** in this order:
   1. `.ai/core/engineering.md`
   2. `.ai/core/workflow.md`
   3. `.ai/core/dotnet.md` (for any .NET file change)
   4. Relevant `.ai/domains/*.md` for the project(s) being touched
   5. Relevant `.ai/skills/*.md` for the task type
3. **Confirm target file paths and projects** before editing. File path is
   authoritative; do not infer placement from namespace.
4. If no specialized guidance applies, proceed with `core/` rules only.

Do not skip this check.

## Task Routing

- Every .NET file change reads `.ai/core/dotnet.md`.
- Every change inside a project with a matching `domains/*.md` file reads
  that file.
- Every task with a matching `skills/*.md` file reads that file and follows
  its checklist.
- If no `skills/*.md` matches the task, work from `domains/` + `core/` and
  propose a new skill file if the task type is likely to recur.

## Conflict Resolution

When guidance conflicts, apply this precedence:

1. The most specific applicable `skills/*.md` file.
2. The applicable `domains/*.md` file.
3. `core/*.md`.

Within the same tier, the more specific rule wins.

## Source of Truth

- `.ai/` is the only canonical instruction source.
- Bootstrap files for individual agents (for example
  `.github/copilot-instructions.md`, `CLAUDE.md`) must delegate here rather
  than duplicate rules.

## Cross-Repository Context

- This API (`ApiDemo.*`) is consumed by the companion `blazor-demo-event`
  repository over HTTP. If a change alters a response shape, a route, or
  an auth requirement, check whether the Blazor client's service layer
  (`Services/Implementations/*`, `Models/*`) needs a matching update, and
  say so explicitly in the handoff even if you cannot make that change
  from this repository.

## Execution Expectations

- Prefer complete, working changes over partial output.
- Validate changes with the smallest targeted build (and test, once test
  projects exist) command that covers the change before handing off.
- Keep changes scoped. Do not perform unrelated refactors in the same
  change.
- When delegating to sub-agents, only the primary agent applies final file
  edits; sub-agents are analysis or drafting only.

## Handoff

When handing off a completed change, include:

- What was changed.
- What remains, if anything.
- Validation status (build result, test result).
- Known risks or assumptions, including any required change in the
  companion `blazor-demo-event` client.
