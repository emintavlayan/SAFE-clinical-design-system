module Pages.ValidationPage

open Feliz
open Design.Components

/// Renders the starter validation page.
let view () =
    Html.div [
        prop.className "space-y-6"
        prop.children [
            Html.p [
                prop.className "max-w-3xl text-sm opacity-80"
                prop.text
                    "Starter validation surfaces should stay concise and clearly distinguish blocking, warning, and successful states."
            ]
            errorAlert
                "Blocking error"
                "Patient, course, and plan identifiers must be present before execution can continue."
            warningAlert
                "Warning"
                "A peer review is recommended when imported data has changed since the last approved run."
            successAlert "Success" "The validation surface can confirm when required prerequisites are complete."
        ]
    ]