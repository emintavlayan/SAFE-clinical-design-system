module Pages.ValidationPage

open Feliz
open Design.Components
open Design.SafetyPatterns

type Model = {
    PatientId: string
    Course: string
    Reviewer: string
}

type Msg =
    | SetPatientId of string
    | SetCourse of string
    | SetReviewer of string

let initialModel = {
    PatientId = ""
    Course = ""
    Reviewer = ""
}

let private findings model = [
    if model.PatientId.Trim() = "" || model.Course.Trim() = "" then
        {
            Kind = BlockingError
            Title = "Blocking error"
            Message = "Patient and course identifiers are required before the workflow can continue."
        }
    {
        Kind = Warning
        Title = "Warning"
        Message = "A secondary reviewer should be assigned when workflow inputs come from an external planning system."
    }
    {
        Kind = Information
        Title = "Information"
        Message = "Validation messages should explain the next action and not rely on external documentation."
    }
    if
        model.PatientId.Trim() <> ""
        && model.Course.Trim() <> ""
        && model.Reviewer.Trim() <> ""
    then
        {
            Kind = Success
            Title = "Successful validation"
            Message = "The required identifiers and reviewer fields are complete."
        }
]

let update msg model =
    match msg with
    | SetPatientId patientId -> { model with PatientId = patientId }
    | SetCourse course -> { model with Course = course }
    | SetReviewer reviewer -> { model with Reviewer = reviewer }

let view model dispatch =
    let patientHasError = model.PatientId.Trim() = ""
    let courseHasError = model.Course.Trim() = ""

    Html.div [
        prop.className "space-y-6"
        prop.children [
            Html.div [
                prop.className "grid gap-6 xl:grid-cols-2"
                prop.children [
                    textInput
                        "Patient identifier"
                        "Field validation should be visible before submission."
                        model.PatientId
                        "Enter patient identifier"
                        patientHasError
                        (SetPatientId >> dispatch)
                    textInput
                        "Course"
                        "Use the clinical course identifier."
                        model.Course
                        "Enter course identifier"
                        courseHasError
                        (SetCourse >> dispatch)
                    textInput
                        "Reviewer"
                        "Assign a responsible reviewer for the run."
                        model.Reviewer
                        "Enter reviewer name"
                        false
                        (SetReviewer >> dispatch)
                ]
            ]

            if patientHasError || courseHasError then
                blockingErrorPanel "Field validation" "These missing values block the workflow immediately." [
                    if patientHasError then
                        "Provide the patient identifier."

                        if courseHasError then
                            "Provide the course identifier."
                ]

            validationPanel
                "Validation examples"
                "This page demonstrates blocking, warning, informational, successful, and field-level validation states."
                (findings model)
        ]
    ]