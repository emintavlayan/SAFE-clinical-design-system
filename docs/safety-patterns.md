# Safety Patterns

This document covers the safety-oriented patterns defined in `src/Client/Design/SafetyPatterns.fs`.

## wizardSteps

Why:
Multi-step workflows need clear stage visibility so operators know where they are and what is complete.

Use when:
Use step indicators for staged workflows with ordered progression.

Do not use when:
Do not use steps for single-screen forms without meaningful stage boundaries.

## decisionSummaryCard

Why:
Operators need a concise summary of the context that will govern execution.

Use when:
Use a decision summary card to present patient, course, plan, or parameter data before validation or execution.

Do not use when:
Do not hide required detail in dense prose when a structured summary will scan faster.

## validationPanel

Why:
Mixed validation severities should be grouped in one predictable place.

Use when:
Use the validation panel to present blocking errors, warnings, information, and successful checks together.

Do not use when:
Do not scatter validation feedback across multiple unrelated parts of the page when a unified panel is possible.

## blockingErrorPanel

Why:
Stopping conditions must be obvious and actionable.

Use when:
Use the blocking error panel when the workflow cannot continue and the user needs explicit next steps.

Do not use when:
Do not use a blocking panel for advisory warnings.

## reviewBeforeRunPanel

Why:
Execution should be gated by a final review of summary, warnings, provenance, and checklist confirmation.

Use when:
Use the review-before-run panel for any action that can trigger downstream clinical or operational change.

Do not use when:
Do not expose direct execution actions without review when the workflow still has nontrivial consequences.

## provenancePanel

Why:
Traceability needs a dedicated and repeatable surface.

Use when:
Use provenance panels for imported plans, generated artifacts, or external source data.

Do not use when:
Do not hide lineage in logs or secondary tabs when it affects the operator’s decision.

## statusBadge and riskBadge

Why:
Short state and risk labels should be consistent across applications.

Use when:
Use badges where quick state scanning matters.

Do not use when:
Do not invent free-form status wording when an existing status or risk label is adequate.

## confirmationPanel

Why:
High-consequence actions should require an explicit acknowledgment.

Use when:
Use a confirmation panel when a single checkbox or statement should gate a final action.

Do not use when:
Do not add confirmation panels to low-consequence actions that the user can easily reverse.
