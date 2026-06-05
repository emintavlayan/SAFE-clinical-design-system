module Pages.AccordionPage

open Feliz
open Design.Components

/// Renders one starter accordion example and its placeholder card.
let view () =
    Html.div [
        prop.className "space-y-6"
        prop.children [
            Html.p [
                prop.className "max-w-3xl text-sm opacity-80"
                prop.text "Accordions help compress supporting details without hiding primary workflow controls."
            ]
            Html.div [
                prop.className "collapse collapse-arrow border border-base-300 bg-base-100"
                prop.children [
                    Html.input [ prop.type'.checkbox; prop.defaultChecked true ]
                    Html.div [
                        prop.className "collapse-title text-base font-semibold"
                        prop.text "Imported plan provenance"
                    ]
                    Html.div [
                        prop.className "collapse-content text-sm opacity-80"
                        prop.text
                            "Show source system, source version, import time, and reviewer ownership when collapsed detail is appropriate."
                    ]
                ]
            ]
            emptyState "Accordion" "Clinical accordion examples will be added here." None
        ]
    ]