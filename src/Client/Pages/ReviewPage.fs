module Pages.ReviewPage

open Feliz
open Design.SafetyPatterns

type Model = {
    Checklist: Set<string>
    ExecutionRequested: bool
}

type Msg =
    | ToggleChecklist of string * bool
    | Execute
    | Cancel

let initialModel = {
    Checklist = Set.empty
    ExecutionRequested = false
}

let private checklistKeys = [ "patient"; "parameters"; "warnings"; "provenance" ]

let private checklistItems model = [
    {
        Key = "patient"
        Label = "I verified the patient, course, and plan selection."
        IsChecked = model.Checklist.Contains "patient"
    }
    {
        Key = "parameters"
        Label = "I reviewed the operation parameters and generated values."
        IsChecked = model.Checklist.Contains "parameters"
    }
    {
        Key = "warnings"
        Label = "I assessed the warnings and determined they are acceptable for execution."
        IsChecked = model.Checklist.Contains "warnings"
    }
    {
        Key = "provenance"
        Label = "I confirmed the provenance and source system refresh timestamp."
        IsChecked = model.Checklist.Contains "provenance"
    }
]

let private canExecute model =
    checklistKeys |> List.forall model.Checklist.Contains

let update msg model =
    match msg with
    | ToggleChecklist(key, isChecked) ->
        let checklist =
            if isChecked then
                model.Checklist |> Set.add key
            else
                model.Checklist |> Set.remove key

        { model with Checklist = checklist }
    | Execute when canExecute model -> { model with ExecutionRequested = true }
    | Execute -> model
    | Cancel -> {
        model with
            Checklist = Set.empty
            ExecutionRequested = false
      }

let view model dispatch =
    Html.div [
        prop.className "space-y-6"
        prop.children [
            Html.div [
                prop.className "flex flex-wrap gap-2"
                prop.children [ statusBadge Ready; riskBadge Moderate ]
            ]

            decisionSummaryCard
                "Workflow summary"
                "Review summaries should let the operator inspect the full run context before execution."
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

            provenancePanel
                "Provenance"
                ([
                    {
                        Label = "Source system"
                        Value = "Treatment planning system"
                    }
                    {
                        Label = "Source plan version"
                        Value = "14.2 / Approved 2026-06-04 09:15 CET"
                    }
                    {
                        Label = "Imported by"
                        Value = "Clinical automation gateway"
                    }
                    {
                        Label = "Template version"
                        Value = "safe-clinical-design-system / foundation"
                    }
                ]
                : ProvenanceItem list)

            reviewBeforeRunPanel
                "Review before run"
                "This mock workflow summary demonstrates the last gate before an execution action is exposed."
                ([
                    {
                        Label = "Patient"
                        Value = "HOSP-20481"
                    }
                    {
                        Label = "Course"
                        Value = "Thoracic adaptive course"
                    }
                    {
                        Label = "Plan"
                        Value = "PLAN-THX-07"
                    }
                    {
                        Label = "Execution mode"
                        Value = "Dose recalculation"
                    }
                ]
                : SummaryItem list)
                [
                    "The imported structure set changed after the last approved review."
                    "A peer review is required before the recalculation result is copied downstream."
                ]
                ([
                    {
                        Label = "Parameters"
                        Value = "Density override review enabled"
                    }
                    {
                        Label = "Review owner"
                        Value = "Physics reviewer / M. Sorensen"
                    }
                    {
                        Label = "Last source refresh"
                        Value = "2026-06-04 13:30 CET"
                    }
                    {
                        Label = "Trace id"
                        Value = "TRACE-CLINICAL-20481"
                    }
                ]
                : ProvenanceItem list)
                (checklistItems model)
                (fun key isChecked -> dispatch (ToggleChecklist(key, isChecked)))
                (fun () -> dispatch Execute)
                (fun () -> dispatch Cancel)
                (canExecute model)
                model.ExecutionRequested
        ]
    ]