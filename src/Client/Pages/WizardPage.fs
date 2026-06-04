module Pages.WizardPage

open Feliz
open Design.Components
open Design.SafetyPatterns

type Step =
    | SelectContext
    | ConfigureOperation
    | Validate
    | ReviewAndRun

type Model = {
    ActiveStep: Step
    Context: string
    Operation: string
    IncludePeerReview: bool
    Checklist: Set<string>
    RunRequested: bool
}

type Msg =
    | SetContext of string
    | SetOperation of string
    | SetIncludePeerReview of bool
    | ToggleChecklist of string * bool
    | NextStep
    | PreviousStep
    | Execute
    | ResetReview

let initialModel = {
    ActiveStep = SelectContext
    Context = ""
    Operation = ""
    IncludePeerReview = true
    Checklist = Set.empty
    RunRequested = false
}

let private contexts: SelectOption list = [
    { Value = ""; Label = "Select context" }
    {
        Value = "head-and-neck"
        Label = "Head and neck course"
    }
    {
        Value = "thoracic"
        Label = "Thoracic adaptive plan"
    }
    {
        Value = "pelvic"
        Label = "Pelvic re-plan"
    }
]

let private operations: SelectOption list = [
    {
        Value = ""
        Label = "Select operation"
    }
    {
        Value = "dose-recalculation"
        Label = "Dose recalculation"
    }
    {
        Value = "structure-review"
        Label = "Structure review"
    }
    {
        Value = "export"
        Label = "Export treatment package"
    }
]

let private reviewChecklistKeys = [ "verify-summary"; "verify-warnings"; "verify-provenance" ]

let private activeIndex step =
    match step with
    | SelectContext -> 0
    | ConfigureOperation -> 1
    | Validate -> 2
    | ReviewAndRun -> 3

let private canAdvanceFromContext model = model.Context <> ""

let private canAdvanceFromConfiguration model = model.Operation <> ""

let private validationFindings model = [
    if model.Context = "" then
        {
            Kind = BlockingError
            Title = "Context is missing"
            Message = "The clinical context must be selected before the workflow can be validated."
        }
    if model.Operation = "" then
        {
            Kind = BlockingError
            Title = "Operation is missing"
            Message = "The operation must be configured before the workflow can proceed."
        }
    if model.Operation = "dose-recalculation" then
        {
            Kind = Warning
            Title = "Secondary check required"
            Message = "Dose recalculation should be reviewed against the latest approved prescription."
        }
    {
        Kind = Information
        Title = "Traceability expectation"
        Message = "All generated outputs should capture input plan version, operator, and generation time."
    }
    if model.Context <> "" && model.Operation <> "" then
        {
            Kind = Success
            Title = "Configuration complete"
            Message = "The workflow has enough information to reach the review step."
        }
]

let private hasBlockingValidation model =
    validationFindings model
    |> List.exists (fun finding -> finding.Kind = BlockingError)

let private canExecute model =
    reviewChecklistKeys |> List.forall model.Checklist.Contains

let update msg model =
    match msg with
    | SetContext context -> { model with Context = context }
    | SetOperation operation -> { model with Operation = operation }
    | SetIncludePeerReview includePeerReview -> {
        model with
            IncludePeerReview = includePeerReview
      }
    | ToggleChecklist(key, isChecked) ->
        let updatedChecklist =
            if isChecked then
                model.Checklist |> Set.add key
            else
                model.Checklist |> Set.remove key

        {
            model with
                Checklist = updatedChecklist
        }
    | NextStep ->
        match model.ActiveStep with
        | SelectContext when canAdvanceFromContext model -> {
            model with
                ActiveStep = ConfigureOperation
          }
        | ConfigureOperation when canAdvanceFromConfiguration model -> { model with ActiveStep = Validate }
        | Validate when not (hasBlockingValidation model) -> { model with ActiveStep = ReviewAndRun }
        | _ -> model
    | PreviousStep ->
        match model.ActiveStep with
        | SelectContext -> model
        | ConfigureOperation -> {
            model with
                ActiveStep = SelectContext
          }
        | Validate -> {
            model with
                ActiveStep = ConfigureOperation
          }
        | ReviewAndRun -> { model with ActiveStep = Validate }
    | Execute when canExecute model -> { model with RunRequested = true }
    | Execute -> model
    | ResetReview -> {
        model with
            Checklist = Set.empty
            RunRequested = false
      }

let private summaryItems model : SummaryItem list = [
    {
        Label = "Context"
        Value = if model.Context = "" then "Not selected" else model.Context
    }
    {
        Label = "Operation"
        Value =
            if model.Operation = "" then
                "Not selected"
            else
                model.Operation
    }
    {
        Label = "Peer review"
        Value =
            if model.IncludePeerReview then
                "Required"
            else
                "Not required"
    }
    {
        Label = "Validation"
        Value =
            if hasBlockingValidation model then
                "Blocked"
            else
                "Ready for review"
    }
]

let private checklistItems model : ReviewChecklistItem list = [
    {
        Key = "verify-summary"
        Label = "I reviewed the context and operation summary."
        IsChecked = model.Checklist.Contains "verify-summary"
    }
    {
        Key = "verify-warnings"
        Label = "I reviewed the warnings and confirmed they are acceptable."
        IsChecked = model.Checklist.Contains "verify-warnings"
    }
    {
        Key = "verify-provenance"
        Label = "I verified the provenance and source plan version."
        IsChecked = model.Checklist.Contains "verify-provenance"
    }
]

let view model dispatch =
    Html.div [
        prop.className "space-y-6"
        prop.children [
            wizardSteps
                [ "Select Context"; "Configure Operation"; "Validate"; "Review And Run" ]
                (activeIndex model.ActiveStep)

            match model.ActiveStep with
            | SelectContext ->
                card "Select context" "The first step establishes the clinical scope for the workflow." [
                    selectInput
                        "Clinical context"
                        "Choose the course or plan family."
                        model.Context
                        contexts
                        (model.Context = "")
                        (SetContext >> dispatch)
                ]
            | ConfigureOperation ->
                card "Configure operation" "Operation parameters should be explicit before validation starts." [
                    Html.div [
                        prop.className "space-y-4"
                        prop.children [
                            selectInput
                                "Operation"
                                "Choose the action that will be executed later."
                                model.Operation
                                operations
                                (model.Operation = "")
                                (SetOperation >> dispatch)
                            checkbox
                                "Require peer review in the production application"
                                model.IncludePeerReview
                                (SetIncludePeerReview >> dispatch)
                        ]
                    ]
                ]
            | Validate ->
                validationPanel
                    "Validation"
                    "Only the active step shows detailed content, and validation must clear before review."
                    (validationFindings model)
            | ReviewAndRun ->
                reviewBeforeRunPanel
                    "Review before run"
                    "The final step requires a human review of summary data, warnings, provenance, and checklist confirmations."
                    (summaryItems model)
                    [
                        "Dose recalculation workflows should be cross-checked against the approved prescription when peer review is enabled."
                    ]
                    ([
                        {
                            Label = "Source plan"
                            Value = "TPS / Plan version 14.2"
                        }
                        {
                            Label = "Imported by"
                            Value = "Physics workflow service"
                        }
                        {
                            Label = "Last refreshed"
                            Value = "2026-06-04 13:30 CET"
                        }
                        {
                            Label = "Template version"
                            Value = "safe-clinical-design-system / foundation"
                        }
                    ]
                    : ProvenanceItem list)
                    (checklistItems model)
                    (fun key isChecked -> dispatch (ToggleChecklist(key, isChecked)))
                    (fun () -> dispatch Execute)
                    (fun () -> dispatch ResetReview)
                    (canExecute model)
                    model.RunRequested

            Html.div [
                prop.className "flex flex-wrap gap-3"
                prop.children [
                    secondaryAction "Back" (model.ActiveStep = SelectContext) (fun () -> dispatch PreviousStep)

                    match model.ActiveStep with
                    | ReviewAndRun -> secondaryAction "Reset review" false (fun () -> dispatch ResetReview)
                    | _ -> Html.none

                    match model.ActiveStep with
                    | SelectContext ->
                        primaryAction "Next" (not (canAdvanceFromContext model)) (fun () -> dispatch NextStep)
                    | ConfigureOperation ->
                        primaryAction "Next" (not (canAdvanceFromConfiguration model)) (fun () -> dispatch NextStep)
                    | Validate -> primaryAction "Next" (hasBlockingValidation model) (fun () -> dispatch NextStep)
                    | ReviewAndRun -> Html.none
                ]
            ]
        ]
    ]