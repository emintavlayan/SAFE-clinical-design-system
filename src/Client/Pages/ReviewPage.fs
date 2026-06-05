module Pages.ReviewPage

open Feliz
open Design.Components
open Design.SafetyPatterns

type Model = {
    IsReviewed: bool
    ExecutionRequested: bool
}

type Msg =
    | ToggleReviewed of bool
    | Execute
    | Cancel

let initialModel = {
    IsReviewed = false
    ExecutionRequested = false
}

/// Returns whether the Execute action can be enabled.
let private canExecute model = model.IsReviewed

/// Updates the review-before-run starter page state.
let update msg model =
    match msg with
    | ToggleReviewed isReviewed -> { model with IsReviewed = isReviewed }
    | Execute when canExecute model -> { model with ExecutionRequested = true }
    | Execute -> model
    | Cancel -> {
        model with
            IsReviewed = false
            ExecutionRequested = false
      }

/// Renders the review-before-run starter layout.
let view model dispatch =
    Html.div [
        prop.className "space-y-6"
        prop.children [
            decisionSummaryCard
                "Summary"
                "Review the clinical context before exposing the execution action."
                ([
                    {
                        Label = "Patient"
                        Value = "HOSP-20481 / Jane Doe"
                    }
                    {
                        Label = "Course"
                        Value = "Thoracic adaptive course"
                    }
                    {
                        Label = "Plan"
                        Value = "PLAN-THX-07 / Revision 3"
                    }
                    {
                        Label = "Parameters"
                        Value = "Isocenter locked, density override review enabled"
                    }
                ]
                : SummaryItem list)

            card "Warnings" "Warnings should be visible before the operator chooses to execute." [
                Html.div [
                    prop.className "space-y-3"
                    prop.children [
                        warningAlert "Warning" "Imported structure data changed after the last approved plan review."
                        warningAlert "Warning" "A peer review is required before downstream execution."
                    ]
                ]
            ]

            provenancePanel
                "Provenance"
                ([
                    {
                        Label = "Source system"
                        Value = "Treatment planning system"
                    }
                    {
                        Label = "Source plan version"
                        Value = "14.2 / Approved 2026-06-05 09:15 CET"
                    }
                    {
                        Label = "Imported by"
                        Value = "Clinical automation gateway"
                    }
                    {
                        Label = "Template version"
                        Value = "SAFE Clinical Design System foundation"
                    }
                ]
                : ProvenanceItem list)

            card "Review checklist" "Execution stays disabled until the operator confirms review." [
                Html.div [
                    prop.className "space-y-4"
                    prop.children [
                        checkbox
                            "I reviewed the summary, warnings, and provenance."
                            model.IsReviewed
                            (ToggleReviewed >> dispatch)
                        if model.ExecutionRequested then
                            successAlert "Execute enabled" "The review gate has been cleared for this starter example."
                    ]
                ]
            ]

            Html.div [
                prop.className "flex flex-wrap gap-3"
                prop.children [
                    secondaryAction "Cancel" false (fun () -> dispatch Cancel)
                    primaryAction "Execute" (not (canExecute model)) (fun () -> dispatch Execute)
                ]
            ]
        ]
    ]