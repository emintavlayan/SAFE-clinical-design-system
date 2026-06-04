# Theme Policy

This repository uses daisyUI themes to standardize reusable SAFE UI across multiple clinical applications.

## Supported Themes

- `light`
- `dark`
- `corporate`
- `business`
- `emerald`
- `synthwave`

`light` is the default theme. `dark` follows the user’s preferred dark mode when no explicit selection has been saved.

## Persistence

The selected theme is stored in browser `localStorage` under the repository-specific theme key. The shell reapplies that theme on load.

## Required Semantic Usage

Reusable components should prefer semantic daisyUI classes such as:

- `bg-base-100`
- `bg-base-200`
- `bg-base-300`
- `text-base-content`
- `btn-primary`
- `alert-warning`
- `alert-error`

Use these semantic classes so theme switching changes the UI consistently without rewriting components.

## Hardcoded Color Policy

Hardcoded visual colors are not the default. They should only appear when:

- a clinical rule requires a fixed appearance
- an organizational brand rule has been documented
- a technical limitation makes semantic tokens insufficient and that decision has been recorded

If a hardcoded color is introduced, document the reason in the component or pattern documentation.

## When To Add A Theme

Add a theme only when:

- a production application needs a different semantic presentation
- the theme still preserves accessible contrast and predictable severity cues

Do not add themes for novelty or isolated demos.
