# Components

This document covers the reusable primitives defined in `src/Client/Design/Components.fs`.

## card

Why:
Cards provide a consistent bordered surface for repeated content blocks.

Use when:
Use a card for a single repeated unit such as a principle, component example, or summary block.

Do not use when:
Do not wrap full page sections or nest cards inside other cards.

## sectionTitle

Why:
Section titles create predictable hierarchy inside content-heavy pages.

Use when:
Use a section title before a group of related controls or examples.

Do not use when:
Do not use it as a replacement for the page title provided by the shell.

## primaryAction, secondaryAction, dangerAction

Why:
Action variants keep forward, neutral, and destructive behavior distinct.

Use when:
Use `primaryAction` for the main next step, `secondaryAction` for neutral actions, and `dangerAction` for irreversible or destructive actions.

Do not use when:
Do not choose variants based on preference or decoration.

## warningAlert, infoAlert, successAlert, errorAlert

Why:
Alerts provide a uniform way to communicate severity and outcome.

Use when:
Use alerts for workflow state, validation feedback, or execution feedback.

Do not use when:
Do not use alerts for ordinary static copy that carries no workflow significance.

## badge

Why:
Badges compress short, repeated state labels into a scannable format.

Use when:
Use badges for statuses, roles, categories, or risk cues that fit within a few words.

Do not use when:
Do not place long narrative text inside a badge.

## fieldLabel, textInput, selectInput, checkbox

Why:
Form primitives should behave consistently across every workflow.

Use when:
Use these primitives for standard labeled fields with optional hints and validation state.

Do not use when:
Do not build one-off field layouts unless the interaction differs materially from a normal input or selection control.

## emptyState

Why:
Empty states help the user understand what is missing and what to do next.

Use when:
Use an empty state when a dataset or result list is absent.

Do not use when:
Do not use empty states for validation errors or transient loading messages.

## simpleTable

Why:
Tables remain the clearest surface for repeated row comparison.

Use when:
Use a simple table when the user needs to compare multiple rows across shared columns.

Do not use when:
Do not force narrative or single-record content into table form.
