module Design.Shell

open Feliz
open Design.Components

type NavigationItem = {
    Label: string
    IsActive: bool
    OnClick: unit -> unit
}

type ShellConfig = {
    Title: string
    Description: string
    ShowPageHeader: bool
    Navigation: NavigationItem list
    Theme: string
    ThemeOptions: string list
    OnThemeChange: string -> unit
    Content: ReactElement
}

/// Renders the theme selector for the application shell.
let themeSelector (currentTheme: string) (themeOptions: string list) (onThemeChange: string -> unit) =
    Html.div [
        prop.className "min-w-56"
        prop.children [
            selectInput
                "Theme"
                "Persisted in localStorage."
                currentTheme
                (themeOptions |> List.map (fun theme -> { Value = theme; Label = theme }))
                false
                onThemeChange
        ]
    ]

/// Renders the top navigation bar for the design-system shell.
let navbar
    (themeOptions: string list)
    (currentTheme: string)
    (navigation: NavigationItem list)
    (onThemeChange: string -> unit)
    =
    Html.header [
        prop.className "border-b border-base-300 bg-base-100"
        prop.children [
            Html.div [
                prop.className "mx-auto flex max-w-7xl flex-col gap-4 px-4 py-4 lg:px-6"
                prop.children [
                    Html.div [
                        prop.className "navbar min-h-0 flex-col items-start gap-4 rounded-box bg-base-100 px-0"
                        prop.children [
                            Html.div [
                                prop.className "space-y-1"
                                prop.children [
                                    Html.h1 [
                                        prop.className "text-xl font-semibold"
                                        prop.text "SAFE Clinical Design System"
                                    ]
                                    Html.p [
                                        prop.className "max-w-3xl text-sm opacity-80"
                                        prop.text
                                            "Reusable SAFE/Feliz/daisyUI patterns for clinical and safety-critical applications."
                                    ]
                                ]
                            ]
                            Html.div [
                                prop.className
                                    "flex w-full flex-col gap-4 lg:flex-row lg:items-center lg:justify-between"
                                prop.children [
                                    Html.nav [
                                        Html.ul [
                                            prop.className "flex flex-wrap gap-2"
                                            prop.children [
                                                for item in navigation do
                                                    Html.li [
                                                        if item.IsActive then
                                                            primaryAction item.Label false item.OnClick
                                                        else
                                                            secondaryAction item.Label false item.OnClick
                                                    ]
                                            ]
                                        ]
                                    ]
                                    Html.div [
                                        prop.className "w-full lg:w-72"
                                        prop.children [ themeSelector currentTheme themeOptions onThemeChange ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]

/// Wraps page content in the outer application container.
let pageContainer (children: ReactElement list) =
    Html.div [
        prop.className "min-h-full bg-base-200 text-base-content"
        prop.children children
    ]

/// Wraps page content in a centered responsive content container.
let contentContainer (children: ReactElement list) =
    Html.main [
        prop.className "mx-auto flex max-w-7xl flex-col gap-8 px-4 py-8 lg:px-6"
        prop.children children
    ]

/// Renders the page title and description section for a navigation target.
let pageTitleSection (title: string) (description: string) =
    Html.section [
        prop.className "space-y-2"
        prop.children [
            Html.h1 [ prop.className "text-3xl font-semibold"; prop.text title ]
            Html.p [ prop.className "max-w-3xl text-sm opacity-80"; prop.text description ]
        ]
    ]

/// Renders the full application shell with navigation, theme selection, and page content.
let applicationShell (config: ShellConfig) =
    pageContainer [
        navbar config.ThemeOptions config.Theme config.Navigation config.OnThemeChange
        contentContainer [
            if config.ShowPageHeader then
                pageTitleSection config.Title config.Description
            config.Content
        ]
    ]