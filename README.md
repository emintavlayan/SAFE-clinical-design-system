# SAFE Clinical Design System

SAFE Clinical Design System is a SAFE Stack repository for reusable UI patterns used in safety-critical clinical applications. It is the canonical source for shell layout, semantic theming, shared Feliz components, workflow patterns, and reference documentation that can be copied into production SAFE applications.

## Purpose

This repository exists to standardize:

- semantic theme usage across SAFE applications
- shared action, alert, field, table, and empty-state components
- review-before-run, validation-first, and provenance-visible workflow patterns
- page composition guidance for clinical operator workflows

It is not an application showcase and it is not a laboratory for unrelated UI experiments.

## Repository Structure

```text
src/
  Client/
    Design/
      Theme.fs
      Shell.fs
      Components.fs
      SafetyPatterns.fs
    Pages/
      HomePage.fs
      ComponentsPage.fs
      WizardUiPage.fs
      StepsPage.fs
      AccordionPage.fs
      ValidationPage.fs
      ReviewPage.fs
      TodoPage.fs
  Server/
  Shared/
docs/
  design-principles.md
  components.md
  safety-patterns.md
  theme-policy.md
  adoption-guide.md
```

## Run

Prerequisites:

- .NET SDK 8 or newer
- Node 18, 20, or 22
- npm 9 or 10

Commands:

```bash
cd src/Client
npm install
cd ../..
dotnet build Application.sln
dotnet run
```

The SAFE client runs at `http://localhost:8080`.

## Current Pages

The running client contains:

- `Home`
- `Wizard UI`
- `Steps`
- `Accordion`
- `Components`
- `Validation`
- `Review Before Run`
- `Todo`

These pages are lightweight reference surfaces and starter layouts, not production workflows.

## Theme Strategy

Configured daisyUI themes:

- `light` (default)
- `dark` (prefers dark)
- `corporate`
- `business`
- `emerald`
- `synthwave`

Theme selection is persisted in `localStorage`. Components should use semantic daisyUI classes such as `bg-base-100`, `bg-base-200`, `bg-base-300`, `text-base-content`, `btn-primary`, `alert-warning`, and `alert-error`. Avoid hardcoded visual colors unless a documented requirement makes them necessary.

See [docs/theme-policy.md](docs/theme-policy.md).

## Add New Components

1. Add the reusable primitive to [src/Client/Design/Components.fs](src/Client/Design/Components.fs).
2. Keep the API small and functional.
3. Use semantic daisyUI classes or `Feliz.DaisyUI` bindings where they reduce noise.
4. Add or update examples on [src/Client/Pages/ComponentsPage.fs](src/Client/Pages/ComponentsPage.fs).
5. Document why the component exists, when it should be used, and when it should not be used in [docs/components.md](docs/components.md).

Add a new component when it removes duplication or standardizes an interaction that appears across workflows. Do not add a component that only wraps a single application-specific layout.

## Add New Patterns

1. Add the pattern to [src/Client/Design/SafetyPatterns.fs](src/Client/Design/SafetyPatterns.fs).
2. Keep the pattern oriented around workflow safety, not generic ornament.
3. Add or update a reference page that demonstrates the pattern in context.
4. Document the rationale in [docs/safety-patterns.md](docs/safety-patterns.md).

Add a new pattern when multiple applications need the same safety behavior, review gate, or provenance treatment. Do not add a pattern for one-off domain rules that belong in an application layer.

## Adoption Strategy

For an existing SAFE application:

1. Copy the `Design` modules into the client project.
2. Copy only the page patterns needed for the target workflow.
3. Replace hardcoded color utilities with semantic daisyUI classes.
4. Move repeated local controls onto the shared component functions.
5. Add review-before-run and provenance surfaces anywhere a workflow can affect patient-facing or downstream clinical state.

See [docs/adoption-guide.md](docs/adoption-guide.md).

## Documentation

- [docs/design-principles.md](docs/design-principles.md)
- [docs/components.md](docs/components.md)
- [docs/safety-patterns.md](docs/safety-patterns.md)
- [docs/theme-policy.md](docs/theme-policy.md)
- [docs/adoption-guide.md](docs/adoption-guide.md)
