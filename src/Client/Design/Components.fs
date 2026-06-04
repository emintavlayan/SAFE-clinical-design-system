module Design.Components

open Feliz
open Feliz.DaisyUI

type BadgeTone =
    | Neutral
    | Primary
    | Secondary
    | Info
    | Success
    | Warning
    | Error

type SelectOption = { Value: string; Label: string }

type TableColumn<'row> = {
    Header: string
    Cell: 'row -> ReactElement
}

/// Returns the daisyUI badge modifier for the supplied tone.
let private badgeTone tone =
    match tone with
    | Neutral -> badge.neutral
    | Primary -> badge.primary
    | Secondary -> badge.secondary
    | Info -> badge.info
    | Success -> badge.success
    | Warning -> badge.warning
    | Error -> badge.error

/// Returns the input class list for the supplied validation state.
let private textInputClass hasError =
    if hasError then
        "input input-bordered input-error w-full bg-base-100"
    else
        "input input-bordered w-full bg-base-100"

/// Returns the select class list for the supplied validation state.
let private selectInputClass hasError =
    if hasError then
        "select select-bordered select-error w-full bg-base-100"
    else
        "select select-bordered w-full bg-base-100"

/// Renders a titled card for reusable page content.
let card (title: string) (description: string) (children: ReactElement list) =
    Daisy.card [
        Feliz.DaisyUI.card.border
        prop.className "bg-base-100 shadow-sm"
        prop.children [
            Daisy.cardBody [
                prop.className "gap-4"
                prop.children [
                    Html.div [
                        prop.className "space-y-2"
                        prop.children [
                            Daisy.cardTitle [ prop.className "text-lg"; prop.text title ]
                            Html.p [ prop.className "text-sm opacity-80"; prop.text description ]
                        ]
                    ]
                    Html.div [ prop.className "space-y-4"; prop.children children ]
                ]
            ]
        ]
    ]

/// Renders a section heading for grouped content.
let sectionTitle (title: string) (description: string) =
    Html.div [
        prop.className "space-y-2"
        prop.children [
            Html.h2 [ prop.className "text-xl font-semibold"; prop.text title ]
            Html.p [ prop.className "max-w-3xl text-sm opacity-80"; prop.text description ]
        ]
    ]

/// Renders the primary button style for forward workflow actions.
let primaryAction (label: string) (isDisabled: bool) (onClick: unit -> unit) =
    Daisy.button.button [
        button.primary
        prop.disabled isDisabled
        prop.onClick (fun _ -> onClick ())
        prop.text label
    ]

/// Renders the secondary button style for neutral workflow actions.
let secondaryAction (label: string) (isDisabled: bool) (onClick: unit -> unit) =
    Daisy.button.button [
        button.secondary
        button.outline
        prop.disabled isDisabled
        prop.onClick (fun _ -> onClick ())
        prop.text label
    ]

/// Renders the danger button style for destructive workflow actions.
let dangerAction (label: string) (isDisabled: bool) (onClick: unit -> unit) =
    Daisy.button.button [
        button.error
        prop.disabled isDisabled
        prop.onClick (fun _ -> onClick ())
        prop.text label
    ]

/// Renders a warning alert with a title and message.
let warningAlert (title: string) (message: string) =
    Daisy.alert [
        alert.warning
        alert.vertical
        prop.className "items-start"
        prop.children [
            Html.span [ prop.className "font-semibold"; prop.text title ]
            Html.span message
        ]
    ]

/// Renders an informational alert with a title and message.
let infoAlert (title: string) (message: string) =
    Daisy.alert [
        alert.info
        alert.vertical
        prop.className "items-start"
        prop.children [
            Html.span [ prop.className "font-semibold"; prop.text title ]
            Html.span message
        ]
    ]

/// Renders a success alert with a title and message.
let successAlert (title: string) (message: string) =
    Daisy.alert [
        alert.success
        alert.vertical
        prop.className "items-start"
        prop.children [
            Html.span [ prop.className "font-semibold"; prop.text title ]
            Html.span message
        ]
    ]

/// Renders an error alert with a title and message.
let errorAlert (title: string) (message: string) =
    Daisy.alert [
        alert.error
        alert.vertical
        prop.className "items-start"
        prop.children [
            Html.span [ prop.className "font-semibold"; prop.text title ]
            Html.span message
        ]
    ]

/// Renders a semantic badge for short status labels.
let badge (tone: BadgeTone) (value: string) =
    Daisy.badge [ badgeTone tone; Feliz.DaisyUI.badge.soft; prop.text value ]

/// Renders a field label with an optional hint.
let fieldLabel (label: string) (hint: string) =
    Html.div [
        prop.className "flex flex-wrap items-center justify-between gap-2"
        prop.children [
            Daisy.fieldsetLegend label

            if hint <> "" then
                Html.span [ prop.className "text-sm opacity-70"; prop.text hint ]
        ]
    ]

/// Renders a reusable text input field.
let textInput
    (label: string)
    (hint: string)
    (value: string)
    (placeholder: string)
    (hasError: bool)
    (onChange: string -> unit)
    =
    Daisy.fieldset [
        prop.className "gap-2"
        prop.children [
            fieldLabel label hint
            Html.input [
                prop.className (textInputClass hasError)
                prop.value value
                prop.placeholder placeholder
                prop.onChange onChange
            ]
        ]
    ]

/// Renders a reusable select input field.
let selectInput
    (label: string)
    (hint: string)
    (value: string)
    (options: SelectOption list)
    (hasError: bool)
    (onChange: string -> unit)
    =
    Daisy.fieldset [
        prop.className "gap-2"
        prop.children [
            fieldLabel label hint
            Html.select [
                prop.className (selectInputClass hasError)
                prop.value value
                prop.onChange onChange
                prop.children [
                    for option in options do
                        Html.option [ prop.value option.Value; prop.text option.Label ]
                ]
            ]
        ]
    ]

/// Renders a reusable checkbox field.
let checkbox (label: string) (isChecked: bool) (onChange: bool -> unit) =
    Daisy.fieldset [
        prop.className "gap-2"
        prop.children [
            Html.label [
                prop.className "flex items-center gap-3"
                prop.children [
                    Html.input [
                        prop.className "checkbox checkbox-primary"
                        prop.type'.checkbox
                        prop.isChecked isChecked
                        prop.onChange onChange
                    ]
                    Html.span label
                ]
            ]
        ]
    ]

/// Renders an empty-state card with an optional action.
let emptyState (title: string) (message: string) (action: ReactElement option) =
    card title message [
        Html.div [
            prop.className "space-y-4"
            prop.children [
                Html.p [ prop.className "text-sm opacity-80"; prop.text message ]

                match action with
                | Some actionElement -> actionElement
                | None -> Html.none
            ]
        ]
    ]

/// Renders a simple semantic table for reusable list displays.
let simpleTable (columns: TableColumn<'row> list) (rows: 'row list) =
    Html.div [
        prop.className "overflow-x-auto rounded-box border border-base-300 bg-base-100"
        prop.children [
            Html.table [
                prop.className "table table-zebra"
                prop.children [
                    Html.thead [
                        Html.tr [
                            for column in columns do
                                Html.th column.Header
                        ]
                    ]
                    Html.tbody [
                        for row in rows do
                            Html.tr [
                                for column in columns do
                                    Html.td [ column.Cell row ]
                            ]
                    ]
                ]
            ]
        ]
    ]