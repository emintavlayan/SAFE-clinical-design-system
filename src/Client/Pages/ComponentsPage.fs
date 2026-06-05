module Pages.ComponentsPage

open Feliz
open Design.Components

type Model = {
    SampleInput: string
    SampleSelection: string
}

type Msg =
    | SetSampleInput of string
    | SetSampleSelection of string

let initialModel = {
    SampleInput = "Clinical note"
    SampleSelection = "review"
}

let private workflowOptions: SelectOption list = [
    { Value = "review"; Label = "Review" }
    {
        Value = "validation"
        Label = "Validation"
    }
    {
        Value = "execution"
        Label = "Execution"
    }
]

type ExampleRow = { Name: string; State: string }

let private exampleRows = [
    {
        Name = "Summary table"
        State = "Starter"
    }
    {
        Name = "Validation surface"
        State = "Reusable"
    }
    {
        Name = "Review gate"
        State = "Shared"
    }
]

/// Updates the local sample state for the component gallery.
let update msg model =
    match msg with
    | SetSampleInput sampleInput -> { model with SampleInput = sampleInput }
    | SetSampleSelection sampleSelection -> {
        model with
            SampleSelection = sampleSelection
      }

/// Renders the compact component gallery page.
let view model dispatch =
    Html.div [
        prop.className "space-y-6"
        prop.children [
            Html.div [
                prop.className "grid gap-6 xl:grid-cols-2"
                prop.children [
                    card "Buttons" "Forward, neutral, and destructive actions stay visually distinct." [
                        Html.div [
                            prop.className "flex flex-wrap gap-3"
                            prop.children [
                                primaryAction "Primary" false (fun () -> ())
                                secondaryAction "Secondary" false (fun () -> ())
                                dangerAction "Danger" false (fun () -> ())
                            ]
                        ]
                    ]

                    card "Alerts" "Alert variants communicate severity with semantic daisyUI classes." [
                        Html.div [
                            prop.className "space-y-3"
                            prop.children [
                                infoAlert "Info" "Use for non-blocking operational context."
                                warningAlert "Warning" "Use when operator attention is required."
                                successAlert "Success" "Use when a workflow gate has cleared."
                                errorAlert "Error" "Use for blocking or failed conditions."
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

                    card "Inputs" "Reusable fields keep labels, hints, validation, and spacing consistent." [
                        Html.div [
                            prop.className "space-y-4"
                            prop.children [
                                textInput
                                    "Example input"
                                    "Use shared field structure for standard data entry."
                                    model.SampleInput
                                    "Enter sample text"
                                    false
                                    (SetSampleInput >> dispatch)
                                selectInput
                                    "Example select"
                                    "Use shared selection styling for simple choices."
                                    model.SampleSelection
                                    workflowOptions
                                    false
                                    (SetSampleSelection >> dispatch)
                            ]
                        ]
                    ]

                    card "Card" "Cards provide a standard surface for reusable content blocks." [
                        Html.p [
                            prop.className "text-sm opacity-80"
                            prop.text "Use cards for component examples, summaries, and grouped workflow content."
                        ]
                    ]
                ]
            ]

            Html.section [
                prop.className "space-y-4"
                prop.children [
                    sectionTitle "Table" "Tables are appropriate when rows must be scanned and compared repeatedly."
                    simpleTable
                        [
                            {
                                Header = "Component"
                                Cell = fun row -> Html.span row.Name
                            }
                            {
                                Header = "State"
                                Cell = fun row -> Html.span row.State
                            }
                        ]
                        exampleRows
                ]
            ]
        ]
    ]