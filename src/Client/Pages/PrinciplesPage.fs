module Pages.PrinciplesPage

open Feliz
open Design.Components

type Principle = {
    Title: string
    Why: string
    UseWhen: string
    AvoidWhen: string
}

let private principles = [
    {
        Title = "Semantic themes"
        Why =
            "Safety-critical applications need a consistent visual language that can be re-skinned without rewriting components."
        UseWhen =
            "Use semantic base, status, and action classes so the same component remains readable across light, dark, and organizational themes."
        AvoidWhen =
            "Do not hardcode visual colors unless a documented regulatory or brand rule requires a fixed presentation."
    }
    {
        Title = "Consistency"
        Why = "Clinicians and operators should not relearn common controls between workflows."
        UseWhen =
            "Use the shared shell, button styles, field patterns, and decision panels across related applications."
        AvoidWhen =
            "Do not introduce page-local conventions for common actions when an existing design-system component already fits."
    }
    {
        Title = "Visual hierarchy"
        Why = "Operators need the critical state to stand out without scanning decorative UI."
        UseWhen = "Use page titles, section titles, semantic alerts, and status badges to structure attention."
        AvoidWhen = "Do not rely on decorative layout or dense text blocks to communicate severity."
    }
    {
        Title = "Destructive actions"
        Why =
            "High-consequence workflows need explicit differentiation between forward progress and destructive operations."
        UseWhen =
            "Use danger actions and confirmation panels only for actions that can delete, overwrite, or irrevocably execute work."
        AvoidWhen = "Do not style cancel or back actions as destructive unless they discard committed data."
    }
    {
        Title = "Review-before-run workflows"
        Why =
            "Clinical execution should be gated by a final human review step with summary, warnings, and checklist visibility."
        UseWhen =
            "Use the review-before-run panel before any action that affects patient plans, parameters, exports, or downstream systems."
        AvoidWhen = "Do not skip the review surface when the operator still needs to verify warnings or provenance."
    }
    {
        Title = "Validation-first workflows"
        Why = "Blocking issues should surface before execution, not after a run starts."
        UseWhen =
            "Validate required context, configuration, and dependencies as early as possible and make blocking errors explicit."
        AvoidWhen = "Do not hide validation behind a final submission when intermediate steps can already be checked."
    }
    {
        Title = "Provenance visibility"
        Why = "Operators need to know where data came from, who produced it, and when it was last refreshed."
        UseWhen = "Expose provenance for imported plans, generated parameters, and externally sourced metadata."
        AvoidWhen = "Do not require users to inspect logs or external systems to verify data lineage."
    }
    {
        Title = "Status communication"
        Why = "Shared vocabulary reduces ambiguity between draft, ready, blocked, and completed work."
        UseWhen = "Use status and risk badges anywhere a workflow state must be scanned quickly."
        AvoidWhen = "Do not rely on free-form prose alone when a standard status label will communicate faster."
    }
]

let view () =
    Html.div [
        prop.className "space-y-6"
        prop.children [
            infoAlert
                "Purpose"
                "These principles define why the design-system patterns exist and how they should shape production SAFE applications."

            Html.div [
                prop.className "grid gap-6 xl:grid-cols-2"
                prop.children [
                    for principle in principles do
                        card principle.Title principle.Why [
                            Html.div [
                                prop.className "space-y-3 text-sm"
                                prop.children [
                                    Html.div [
                                        prop.className "space-y-1"
                                        prop.children [
                                            Html.h3 [ prop.className "font-semibold"; prop.text "Use when" ]
                                            Html.p principle.UseWhen
                                        ]
                                    ]
                                    Html.div [
                                        prop.className "space-y-1"
                                        prop.children [
                                            Html.h3 [ prop.className "font-semibold"; prop.text "Do not use when" ]
                                            Html.p principle.AvoidWhen
                                        ]
                                    ]
                                ]
                            ]
                        ]
                ]
            ]
        ]
    ]