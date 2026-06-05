module Pages.StepsPage

open Feliz
open Design.Components
open Design.SafetyPatterns

/// Renders the Steps reference page.
let view () =
    Html.div [
        prop.className "space-y-6"
        prop.children [
            Html.p [
                prop.className "max-w-3xl text-sm opacity-80"
                prop.text "This page shows the staged workflow indicator style used for clinical task progression."
            ]
            wizardSteps [ "Select Context"; "Configure"; "Validate"; "Review" ] 1
            emptyState "Steps" "Step indicators and staged workflow examples will be added here." None
        ]
    ]