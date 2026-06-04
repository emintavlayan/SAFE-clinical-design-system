module Pages.ComponentsPage

open Feliz
open Design.Components
open Design.SafetyPatterns

type Model = {
    PatientId: string
    WorkflowType: string
    RequiresPeerReview: bool
}

type Msg =
    | SetPatientId of string
    | SetWorkflowType of string
    | SetRequiresPeerReview of bool

type ExampleRow = {
    Name: string
    Status: Status
    Risk: RiskLevel
}

let initialModel = {
    PatientId = "HOSP-20481"
    WorkflowType = "plan-review"
    RequiresPeerReview = true
}

let private workflowOptions: SelectOption list = [
    {
        Value = "plan-review"
        Label = "Plan review"
    }
    {
        Value = "dose-recalculation"
        Label = "Dose recalculation"
    }
    {
        Value = "export"
        Label = "Export bundle"
    }
]

let private exampleRows = [
    {
        Name = "Plan import"
        Status = Ready
        Risk = Moderate
    }
    {
        Name = "Constraint validation"
        Status = Blocked
        Risk = High
    }
    {
        Name = "Clinical sign-off"
        Status = Complete
        Risk = Low
    }
]

let update msg model =
    match msg with
    | SetPatientId patientId -> { model with PatientId = patientId }
    | SetWorkflowType workflowType -> {
        model with
            WorkflowType = workflowType
      }
    | SetRequiresPeerReview requiresPeerReview -> {
        model with
            RequiresPeerReview = requiresPeerReview
      }

let view model dispatch =
    Html.div [
        prop.className "space-y-8"
        prop.children [
            infoAlert
                "Component catalog"
                "These primitives are intended to be copied into production SAFE applications and composed into workflow-specific screens."

            Html.div [
                prop.className "grid gap-6 xl:grid-cols-2"
                prop.children [
                    card "Actions" "Forward, neutral, and destructive actions should remain visually distinct." [
                        Html.div [
                            prop.className "flex flex-wrap gap-3"
                            prop.children [
                                primaryAction "Primary action" false (fun () -> ())
                                secondaryAction "Secondary action" false (fun () -> ())
                                dangerAction "Danger action" false (fun () -> ())
                            ]
                        ]
                    ]

                    card "Alerts" "Alert variants communicate severity without using arbitrary utility colors." [
                        Html.div [
                            prop.className "space-y-3"
                            prop.children [
                                warningAlert
                                    "Warning"
                                    "Warnings highlight conditions that require attention before execution."
                                infoAlert
                                    "Information"
                                    "Informational messages explain context without blocking the workflow."
                                successAlert
                                    "Success"
                                    "Success messages confirm that a review or validation gate has cleared."
                                errorAlert "Error" "Error messages identify blocking conditions that must be corrected."
                            ]
                        ]
                    ]

                    card "Badges" "Badges compress short status labels into a scannable semantic form." [
                        Html.div [
                            prop.className "flex flex-wrap gap-2"
                            prop.children [
                                badge BadgeTone.Primary "Primary"
                                badge BadgeTone.Secondary "Secondary"
                                badge BadgeTone.Info "Info"
                                badge BadgeTone.Success "Success"
                                badge BadgeTone.Warning "Warning"
                                badge BadgeTone.Error "Error"
                            ]
                        ]
                    ]

                    card "Form inputs" "Reusable fields should keep labels, hints, validation, and spacing consistent." [
                        Html.div [
                            prop.className "space-y-4"
                            prop.children [
                                textInput
                                    "Patient identifier"
                                    "Use a stable clinical identifier."
                                    model.PatientId
                                    "Enter patient identifier"
                                    (model.PatientId.Trim() = "")
                                    (SetPatientId >> dispatch)
                                selectInput
                                    "Workflow type"
                                    "Choose the workflow family."
                                    model.WorkflowType
                                    workflowOptions
                                    false
                                    (SetWorkflowType >> dispatch)
                                checkbox
                                    "Require peer review before execution"
                                    model.RequiresPeerReview
                                    (SetRequiresPeerReview >> dispatch)
                            ]
                        ]
                    ]

                    emptyState
                        "Empty state"
                        "Use empty states for missing data sets, not for transient validation messages."
                        (Some(secondaryAction "Review source system" false (fun () -> ())))

                    card "Field label" "Labels and hints should explain the data expectation before the user types." [
                        fieldLabel "Plan scope" "Describe the specific structure set or course selection."
                    ]
                ]
            ]

            Html.section [
                prop.className "space-y-4"
                prop.children [
                    sectionTitle
                        "Simple table"
                        "Tables are appropriate when rows must be scanned and compared repeatedly."
                    simpleTable
                        [
                            {
                                Header = "Workflow"
                                Cell = fun row -> Html.span row.Name
                            }
                            {
                                Header = "Status"
                                Cell = fun row -> statusBadge row.Status
                            }
                            {
                                Header = "Risk"
                                Cell = fun row -> riskBadge row.Risk
                            }
                        ]
                        exampleRows
                ]
            ]
        ]
    ]