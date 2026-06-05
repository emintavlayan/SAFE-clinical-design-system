module Pages.WizardUiPage

open Feliz
open Design.Components

/// Renders the Wizard UI placeholder page.
let view () =
    Html.div [
        prop.className "space-y-6"
        prop.children [
            Html.p [
                prop.className "max-w-3xl text-sm opacity-80"
                prop.text
                    "This page reserves space for future wizard layouts that combine staged data entry and safety review."
            ]
            emptyState "Wizard UI" "Wizard UI examples will be added here." None
        ]
    ]