module Index

open Browser.Dom
open Elmish
open Design
open Pages

type Page =
    | Home
    | WizardUi
    | Steps
    | Accordion
    | Validation
    | ReviewBeforeRun
    | Components
    | Todo

type Model = {
    CurrentPage: Page
    Theme: string
    ComponentsPage: ComponentsPage.Model
    ReviewPage: ReviewPage.Model
    TodoPage: TodoPage.Model
}

type Msg =
    | NavigateTo of Page
    | ThemeChanged of string
    | ComponentsPageMsg of ComponentsPage.Msg
    | ReviewPageMsg of ReviewPage.Msg
    | TodoPageMsg of TodoPage.Msg

/// Returns the title for the supplied page.
let private pageTitle page =
    match page with
    | Home -> "Home"
    | WizardUi -> "Wizard UI"
    | Steps -> "Steps"
    | Accordion -> "Accordion"
    | Validation -> "Validation"
    | ReviewBeforeRun -> "Review Before Run"
    | Components -> "Components"
    | Todo -> "Todo"

/// Returns the shell description for the supplied page.
let private pageDescription page =
    match page with
    | Home -> "Reusable SAFE/Feliz/daisyUI patterns for clinical and safety-critical applications."
    | WizardUi -> "Starter space for staged wizard layouts and future workflow patterns."
    | Steps -> "Starter space for step indicators and staged workflow references."
    | Accordion -> "Starter space for clinical disclosure patterns built with daisyUI collapse surfaces."
    | Validation -> "Starter validation surfaces for blocking, warning, and success messages."
    | ReviewBeforeRun -> "Starter review gate with summary, warnings, provenance, and controlled execution."
    | Components -> "Compact gallery of shared buttons, alerts, badges, fields, cards, and tables."
    | Todo -> "The SAFE Todo sample restyled with shared SAFE Clinical Design System components."

/// Returns whether the shell should render the page title section.
let private showPageHeader page = page <> Home

/// Returns the ordered navigation pages for the application shell.
let private navigationPages = [
    Home
    WizardUi
    Steps
    Accordion
    Validation
    ReviewBeforeRun
    Components
    Todo
]

/// Returns the document title for the supplied page.
let private browserTitle page =
    if page = Home then
        "SAFE Clinical Design System"
    else
        $"{pageTitle page} | SAFE Clinical Design System"

/// Applies the browser title for the current page.
let private setBrowserTitle page = document.title <- browserTitle page

/// Builds the shell navigation items for the current page.
let private navigationItems currentPage dispatch : Shell.NavigationItem list =
    navigationPages
    |> List.map (fun page -> {
        Label = pageTitle page
        IsActive = currentPage = page
        OnClick = fun () -> dispatch (NavigateTo page)
    })

/// Builds the home-page library cards with page navigation callbacks.
let private homeLibraryPages dispatch : HomePage.LibraryPage list =
    [ WizardUi; Steps; Accordion; Validation; ReviewBeforeRun; Components; Todo ]
    |> List.map (fun page -> {
        Title = pageTitle page
        Description = pageDescription page
        OnOpen = fun () -> dispatch (NavigateTo page)
    })

/// Initializes the top-level application model.
let init () =
    let theme = Theme.loadTheme ()
    Theme.setTheme theme
    setBrowserTitle Home

    let todoPage, todoCommand = TodoPage.init ()

    {
        CurrentPage = Home
        Theme = theme
        ComponentsPage = ComponentsPage.initialModel
        ReviewPage = ReviewPage.initialModel
        TodoPage = todoPage
    },
    Cmd.map TodoPageMsg todoCommand

/// Updates the top-level model and delegates page-specific messages.
let update msg model =
    match msg with
    | NavigateTo page ->
        setBrowserTitle page
        { model with CurrentPage = page }, Cmd.none
    | ThemeChanged theme ->
        Theme.saveTheme theme
        Theme.setTheme theme
        { model with Theme = theme }, Cmd.none
    | ComponentsPageMsg pageMsg ->
        {
            model with
                ComponentsPage = ComponentsPage.update pageMsg model.ComponentsPage
        },
        Cmd.none
    | ReviewPageMsg pageMsg ->
        {
            model with
                ReviewPage = ReviewPage.update pageMsg model.ReviewPage
        },
        Cmd.none
    | TodoPageMsg pageMsg ->
        let todoPage, todoCommand = TodoPage.update pageMsg model.TodoPage
        { model with TodoPage = todoPage }, Cmd.map TodoPageMsg todoCommand

/// Returns the current page content for the shell.
let private pageContent model dispatch =
    match model.CurrentPage with
    | Home -> HomePage.view (homeLibraryPages dispatch)
    | WizardUi -> WizardUiPage.view ()
    | Steps -> StepsPage.view ()
    | Accordion -> AccordionPage.view ()
    | Validation -> ValidationPage.view ()
    | ReviewBeforeRun -> ReviewPage.view model.ReviewPage (ReviewPageMsg >> dispatch)
    | Components -> ComponentsPage.view model.ComponentsPage (ComponentsPageMsg >> dispatch)
    | Todo -> TodoPage.view model.TodoPage (TodoPageMsg >> dispatch)

/// Renders the top-level application shell.
let view model dispatch =
    let currentPage = model.CurrentPage

    Shell.applicationShell {
        Title = pageTitle currentPage
        Description = pageDescription currentPage
        ShowPageHeader = showPageHeader currentPage
        Navigation = navigationItems currentPage dispatch
        Theme = model.Theme
        ThemeOptions = Theme.availableThemes
        OnThemeChange = ThemeChanged >> dispatch
        Content = pageContent model dispatch
    }