module Design.SafetyPatterns

open Feliz
open Design.Components

type SummaryItem = { Label: string; Value: string }

type ProvenanceItem = { Label: string; Value: string }

type ValidationKind =
    | BlockingError
    | Warning
    | Information
    | Success

type ValidationFinding = {
    Kind: ValidationKind
    Title: string
    Message: string
}

type Status =
    | Draft
    | Ready
    | InProgress
    | Blocked
    | Complete

type RiskLevel =
    | Low
    | Moderate
    | High
    | Critical

type ReviewChecklistItem = {
    Key: string
    Label: string
    IsChecked: bool
}

/// Returns the badge tone and label for the supplied workflow status.
let private statusPresentation (status: Status) =
    match status with
    | Draft -> BadgeTone.Neutral, "Draft"
    | Ready -> BadgeTone.Primary, "Ready"
    | InProgress -> BadgeTone.Info, "In Progress"
    | Blocked -> BadgeTone.Error, "Blocked"
    | Complete -> BadgeTone.Success, "Complete"

/// Returns the badge tone and label for the supplied clinical risk level.
let private riskPresentation (risk: RiskLevel) =
    match risk with
    | Low -> BadgeTone.Success, "Low Risk"
    | Moderate -> BadgeTone.Warning, "Moderate Risk"
    | High -> BadgeTone.Error, "High Risk"
    | Critical -> BadgeTone.Error, "Critical Risk"

/// Renders the step indicator for wizard-style workflows.
let wizardSteps (labels: string list) (activeIndex: int) =
    Html.ul [
        prop.className
            "steps steps-vertical w-full rounded-box border border-base-300 bg-base-100 p-4 lg:steps-horizontal"
        prop.children [
            for index, label in labels |> List.indexed do
                let stepClass =
                    if index < activeIndex then "step step-primary"
                    elif index = activeIndex then "step step-secondary"
                    else "step"

                Html.li [ prop.className stepClass; prop.text label ]
        ]
    ]

/// Renders a summary card for key workflow decision data.
let decisionSummaryCard (title: string) (description: string) (items: SummaryItem list) =
    card title description [
        Html.dl [
            prop.className "grid gap-4 md:grid-cols-2"
            prop.children [
                for (item: SummaryItem) in items do
                    Html.div [
                        prop.className "space-y-1 rounded-box border border-base-300 bg-base-200 p-4"
                        prop.children [
                            Html.dt [ prop.className "text-sm opacity-70"; prop.text item.Label ]
                            Html.dd [ prop.className "font-medium"; prop.text item.Value ]
                        ]
                    ]
            ]
        ]
    ]

/// Renders a grouped validation panel for mixed severities.
let validationPanel (title: string) (description: string) (findings: ValidationFinding list) =
    Html.section [
        prop.className "space-y-4"
        prop.children [
            sectionTitle title description

            if findings |> List.isEmpty then
                infoAlert "No findings" "This validation panel has no active messages."
            else
                for finding in findings do
                    match finding.Kind with
                    | BlockingError -> errorAlert finding.Title finding.Message
                    | Warning -> warningAlert finding.Title finding.Message
                    | Information -> infoAlert finding.Title finding.Message
                    | Success -> successAlert finding.Title finding.Message
        ]
    ]

/// Renders a blocking error panel for workflow-stopping conditions.
let blockingErrorPanel (title: string) (message: string) (nextSteps: string list) =
    Html.div [
        prop.className "space-y-4 rounded-box border border-error bg-base-100 p-4"
        prop.children [
            errorAlert title message

            if nextSteps |> List.isEmpty |> not then
                Html.div [
                    prop.className "space-y-2"
                    prop.children [
                        Html.h3 [ prop.className "text-sm font-semibold"; prop.text "Required next steps" ]
                        Html.ul [
                            prop.className "list-disc space-y-1 pl-6 text-sm"
                            prop.children [
                                for nextStep in nextSteps do
                                    Html.li nextStep
                            ]
                        ]
                    ]
                ]
        ]
    ]

/// Renders a provenance panel for source and ownership visibility.
let provenancePanel (title: string) (items: ProvenanceItem list) =
    card title "Every safety-critical workflow should expose the source, version, and ownership of its inputs." [
        Html.dl [
            prop.className "space-y-3"
            prop.children [
                for (item: ProvenanceItem) in items do
                    Html.div [
                        prop.className "grid gap-1 md:grid-cols-[minmax(0,14rem)_minmax(0,1fr)]"
                        prop.children [
                            Html.dt [ prop.className "text-sm opacity-70"; prop.text item.Label ]
                            Html.dd [ prop.className "font-medium"; prop.text item.Value ]
                        ]
                    ]
            ]
        ]
    ]

/// Renders a status badge for workflow communication.
let statusBadge (status: Status) =
    let tone, label = statusPresentation status
    badge tone label

/// Renders a risk badge for safety communication.
let riskBadge (risk: RiskLevel) =
    let tone, label = riskPresentation risk
    badge tone label

/// Renders a confirmation panel for high-consequence actions.
let confirmationPanel
    (title: string)
    (message: string)
    (confirmLabel: string)
    (isConfirmed: bool)
    (onToggle: bool -> unit)
    (onConfirm: unit -> unit)
    (onCancel: unit -> unit)
    =
    Html.section [
        prop.className "space-y-4 rounded-box border border-base-300 bg-base-100 p-4"
        prop.children [
            Html.div [
                prop.className "space-y-2"
                prop.children [
                    Html.h3 [ prop.className "text-lg font-semibold"; prop.text title ]
                    Html.p [ prop.className "text-sm opacity-80"; prop.text message ]
                ]
            ]
            checkbox confirmLabel isConfirmed onToggle
            Html.div [
                prop.className "flex flex-wrap gap-3"
                prop.children [
                    secondaryAction "Cancel" false onCancel
                    primaryAction "Confirm" (not isConfirmed) onConfirm
                ]
            ]
        ]
    ]

/// Renders a review-before-run panel with summary, warnings, provenance, and checklist controls.
let reviewBeforeRunPanel
    (title: string)
    (description: string)
    (summaryItems: SummaryItem list)
    (warnings: string list)
    (provenanceItems: ProvenanceItem list)
    (checklistItems: ReviewChecklistItem list)
    (onToggle: string -> bool -> unit)
    (onExecute: unit -> unit)
    (onCancel: unit -> unit)
    (canExecute: bool)
    (executionRequested: bool)
    =
    Html.section [
        prop.className "space-y-6 rounded-box border border-base-300 bg-base-100 p-6"
        prop.children [
            Html.div [
                prop.className "space-y-2"
                prop.children [
                    Html.h3 [ prop.className "text-lg font-semibold"; prop.text title ]
                    Html.p [ prop.className "text-sm opacity-80"; prop.text description ]
                ]
            ]

            Html.dl [
                prop.className "grid gap-4 md:grid-cols-2 xl:grid-cols-4"
                prop.children [
                    for (item: SummaryItem) in summaryItems do
                        Html.div [
                            prop.className "space-y-1 rounded-box bg-base-200 p-4"
                            prop.children [
                                Html.dt [ prop.className "text-sm opacity-70"; prop.text item.Label ]
                                Html.dd [ prop.className "font-medium"; prop.text item.Value ]
                            ]
                        ]
                ]
            ]

            Html.div [
                prop.className "space-y-3"
                prop.children [
                    Html.h4 [ prop.className "font-semibold"; prop.text "Warnings" ]

                    if warnings |> List.isEmpty then
                        successAlert "No active warnings" "The current review scope does not contain advisory warnings."
                    else
                        for (warning: string) in warnings do
                            warningAlert "Review required" warning
                ]
            ]

            Html.div [
                prop.className "space-y-3"
                prop.children [
                    Html.h4 [ prop.className "font-semibold"; prop.text "Provenance" ]
                    Html.dl [
                        prop.className "grid gap-3 md:grid-cols-2"
                        prop.children [
                            for (provenanceItem: ProvenanceItem) in provenanceItems do
                                Html.div [
                                    prop.className "space-y-1 rounded-box bg-base-200 p-4"
                                    prop.children [
                                        Html.dt [ prop.className "text-sm opacity-70"; prop.text provenanceItem.Label ]
                                        Html.dd [ prop.className "font-medium"; prop.text provenanceItem.Value ]
                                    ]
                                ]
                        ]
                    ]
                ]
            ]

            Html.div [
                prop.className "space-y-3"
                prop.children [
                    Html.h4 [ prop.className "font-semibold"; prop.text "Review checklist" ]
                    Html.div [
                        prop.className "space-y-2"
                        prop.children [
                            for (checklistItem: ReviewChecklistItem) in checklistItems do
                                checkbox checklistItem.Label checklistItem.IsChecked (fun isChecked ->
                                    onToggle checklistItem.Key isChecked)
                        ]
                    ]
                ]
            ]

            if executionRequested then
                successAlert
                    "Execution requested"
                    "The workflow has cleared the review gate and can be handed to the production application."

            Html.div [
                prop.className "flex flex-wrap gap-3"
                prop.children [
                    secondaryAction "Cancel" false onCancel
                    primaryAction "Execute" (not canExecute) onExecute
                ]
            ]
        ]
    ]