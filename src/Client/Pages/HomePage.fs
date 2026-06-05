module Pages.HomePage

open Feliz
open Design.Components

type LibraryPage = {
    Title: string
    Description: string
    OnOpen: unit -> unit
}

/// Returns the page action callback at the requested index or a no-op fallback.
let private pageAction index (pages: LibraryPage list) =
    pages
    |> List.tryItem index
    |> Option.map (fun page -> page.OnOpen)
    |> Option.defaultValue ignore

/// Renders a compact landing card for a featured design-system capability.
let private featureCard (title: string) (description: string) =
    card title description [ Html.p [ prop.className "text-sm opacity-80"; prop.text description ] ]

/// Renders a navigable card for a page in the pattern library.
let private libraryCard (page: LibraryPage) =
    card page.Title page.Description [
        Html.div [
            prop.className "flex justify-start"
            prop.children [ secondaryAction "Open page" false page.OnOpen ]
        ]
    ]

/// Renders the design-system homepage.
let view (pages: LibraryPage list) =
    Html.div [
        prop.className "space-y-10"
        prop.children [
            Html.section [
                prop.className "hero rounded-box bg-base-100 shadow-sm"
                prop.children [
                    Html.div [
                        prop.className "hero-content w-full justify-start px-6 py-12 lg:px-10"
                        prop.children [
                            Html.div [
                                prop.className "max-w-3xl space-y-5"
                                prop.children [
                                    Html.h1 [
                                        prop.className "text-4xl font-semibold lg:text-5xl"
                                        prop.text "SAFE Clinical Design System"
                                    ]
                                    Html.p [
                                        prop.className "text-base opacity-80"
                                        prop.text
                                            "Reusable SAFE/Feliz/daisyUI patterns for clinical and safety-critical applications."
                                    ]
                                    Html.div [
                                        prop.className "flex flex-wrap gap-3"
                                        prop.children [
                                            primaryAction "Browse components" false (pageAction 5 pages)
                                            secondaryAction "Open Todo sample" false (pageAction 6 pages)
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
            ]
            Html.section [
                prop.className "grid gap-6 lg:grid-cols-3"
                prop.children [
                    featureCard
                        "Clinical workflow patterns"
                        "Starter surfaces for validation, staged review, and safety-sensitive workflow framing."
                    featureCard
                        "Semantic daisyUI theming"
                        "Themeable primitives built on semantic daisyUI classes instead of fixed visual colors."
                    featureCard
                        "Review-before-run safety surfaces"
                        "Reusable summary, warning, and provenance panels for high-consequence actions."
                ]
            ]
            Html.section [
                prop.className "space-y-4"
                prop.children [
                    sectionTitle "Pattern library" "Reference pages for reusable UI patterns and starter layouts."
                    Html.div [
                        prop.className "grid gap-6 md:grid-cols-2 xl:grid-cols-3"
                        prop.children [
                            for page in pages do
                                libraryCard page
                        ]
                    ]
                ]
            ]
        ]
    ]