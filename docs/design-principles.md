# Design Principles

This document explains why the design principles in `safe-clinical-design-system` exist, when they should be used, and when they should not be used.

## Semantic Themes

Why:
Semantic tokens let the same component operate across light, dark, and organizational themes without rewriting markup.

Use when:
Use semantic daisyUI classes for all reusable components and layout surfaces.

Do not use when:
Do not introduce hardcoded visual colors unless a documented clinical, regulatory, or branding rule requires a fixed presentation.

## Consistency

Why:
Clinical operators should not need to relearn basic controls or page structure between related workflows.

Use when:
Use the shared shell, action styles, field patterns, and summary surfaces across multiple pages or applications.

Do not use when:
Do not invent page-local variants for shared controls when an existing design-system primitive already communicates the same behavior.

## Visual Hierarchy

Why:
Critical state should be discoverable without scanning decorative or ambiguous UI.

Use when:
Use page titles, section titles, alerts, badges, and bordered surfaces to separate severity and workflow stages.

Do not use when:
Do not communicate severity through layout novelty, decorative graphics, or long explanatory paragraphs alone.

## Destructive Actions

Why:
Operators need immediate visual separation between safe progression and destructive execution.

Use when:
Use danger actions and confirmation panels for deletion, overwrite, or irreversible execution.

Do not use when:
Do not style neutral actions such as cancel, close, or back as destructive unless they actually discard committed work.

## Review-Before-Run Workflows

Why:
Workflows that can affect patient plans or downstream systems should expose a final human review gate.

Use when:
Use a review panel whenever a workflow summary, warnings, checklist, and provenance review must happen before execution.

Do not use when:
Do not skip the review gate if the operator still needs to inspect warnings or confirm source data.

## Validation-First Workflows

Why:
Blocking issues should be surfaced before execution rather than discovered after work has already started.

Use when:
Validate required context, identifiers, and operational selections as early as they can be checked.

Do not use when:
Do not delay obvious required-field or context validation until the final action button.

## Provenance Visibility

Why:
Safety-critical workflows require traceability for imported data, generated results, and ownership.

Use when:
Expose source system, source version, refresh time, and responsible owner for any input or generated artifact that influences execution.

Do not use when:
Do not hide provenance in logs or external tools that operators cannot access at the moment of review.

## Status Communication

Why:
Shared status language reduces ambiguity between draft, ready, blocked, and completed work.

Use when:
Use consistent badges and labels anywhere workflow state must be scanned quickly.

Do not use when:
Do not rely on free-form text where a standard status label communicates faster and with less interpretation.
