# SAFE Clinical Design System Adoption Guide

This guide describes how to apply SAFE Clinical Design System to an existing SAFE application.

## What To Copy

Copy these client modules first:

- `src/Client/Design/Theme.fs`
- `src/Client/Design/Shell.fs`
- `src/Client/Design/Components.fs`
- `src/Client/Design/SafetyPatterns.fs`

Copy only the reference pages you need for implementation examples.

## Recommended Adoption Order

1. Replace application-wide hardcoded color utilities with semantic daisyUI classes.
2. Introduce the shell and theme selector so the application has a stable frame.
3. Move repeated buttons, alerts, inputs, and tables onto the shared component primitives.
4. Introduce workflow patterns such as validation panels and review-before-run gates where risk justifies them.
5. Add provenance visibility anywhere imported or generated data affects execution.

## When To Adopt Shared Components

Adopt a shared component when:

- the same interaction appears on multiple pages
- the application needs the same semantics as other clinical workflows
- using the shared component removes local styling drift

Do not adopt a shared component if:

- the interaction is genuinely unique to one application
- the domain rule changes the behavior enough that the shared primitive becomes misleading

## When To Adopt Shared Patterns

Adopt a shared pattern when:

- a workflow can block, warn, execute, or require traceability
- reviewers need a predictable structure before confirming an action

Do not adopt a shared pattern if:

- the workflow is informational only
- the pattern adds formality without reducing actual risk or ambiguity

## Expected Local Customization

Local applications should customize:

- page-level domain content
- API integration
- workflow-specific validation rules
- provenance labels that reflect the source system

Local applications should not customize:

- semantic theme strategy without a documented reason
- basic action severity meanings
- the presence of a final review gate for high-consequence execution flows
