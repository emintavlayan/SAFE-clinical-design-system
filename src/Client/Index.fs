module Index

open Elmish
open Design
open Pages

type Page =
    | Principles
    | Components
    | Wizard
    | Validation
    | ReviewBeforeRun
    | Todo

type Model = {
    CurrentPage: Page
    Theme: string
    ComponentsPage: ComponentsPage.Model
    WizardPage: WizardPage.Model
    ValidationPage: ValidationPage.Model
    ReviewPage: ReviewPage.Model
    TodoPage: TodoPage.Model
}

type Msg =
    | NavigateTo of Page
    | ThemeChanged of string
    | ComponentsPageMsg of ComponentsPage.Msg
    | WizardPageMsg of WizardPage.Msg
    | ValidationPageMsg of ValidationPage.Msg
    | ReviewPageMsg of ReviewPage.Msg
    | TodoPageMsg of TodoPage.Msg

let private pageDetails page =
    match page with
    | Principles ->
        "Principles",
        "Design principles that explain why the patterns exist, when they should be used, and when they should be avoided."
    | Components ->
        "Components", "Reusable Feliz and daisyUI primitives for actions, alerts, fields, empty states, and tables."
    | Wizard ->
        "Wizard",
        "A four-step workflow that demonstrates context selection, configuration, validation, and review-before-run gating."
    | Validation ->
        "Validation",
        "Reference validation patterns for blocking errors, warnings, informational messages, successful checks, and field validation."
    | ReviewBeforeRun ->
        "Review Before Run",
        "A realistic mock review surface that exposes summary data, warnings, provenance, and execution controls."
    | Todo -> "Todo", "The original SAFE Todo feature, refactored to consume shared design-system components."

let private navigationItems currentPage dispatch : Shell.NavigationItem list =
    [ Principles; Components; Wizard; Validation; ReviewBeforeRun; Todo ]
    |> List.map (fun page ->
        let label, _ = pageDetails page

        {
            Label = label
            IsActive = currentPage = page
            OnClick = fun () -> dispatch (NavigateTo page)
        })

let init () =
    let theme = Theme.loadTheme ()
    Theme.setTheme theme

    let todoPage, todoCommand = TodoPage.init ()

    {
        CurrentPage = Principles
        Theme = theme
        ComponentsPage = ComponentsPage.initialModel
        WizardPage = WizardPage.initialModel
        ValidationPage = ValidationPage.initialModel
        ReviewPage = ReviewPage.initialModel
        TodoPage = todoPage
    },
    Cmd.map TodoPageMsg todoCommand

let update msg model =
    match msg with
    | NavigateTo page -> { model with CurrentPage = page }, Cmd.none
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
    | WizardPageMsg pageMsg ->
        {
            model with
                WizardPage = WizardPage.update pageMsg model.WizardPage
        },
        Cmd.none
    | ValidationPageMsg pageMsg ->
        {
            model with
                ValidationPage = ValidationPage.update pageMsg model.ValidationPage
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

let private pageContent model dispatch =
    match model.CurrentPage with
    | Principles -> PrinciplesPage.view ()
    | Components -> ComponentsPage.view model.ComponentsPage (ComponentsPageMsg >> dispatch)
    | Wizard -> WizardPage.view model.WizardPage (WizardPageMsg >> dispatch)
    | Validation -> ValidationPage.view model.ValidationPage (ValidationPageMsg >> dispatch)
    | ReviewBeforeRun -> ReviewPage.view model.ReviewPage (ReviewPageMsg >> dispatch)
    | Todo -> TodoPage.view model.TodoPage (TodoPageMsg >> dispatch)

let view model dispatch =
    let title, description = pageDetails model.CurrentPage

    Shell.applicationShell {
        Title = title
        Description = description
        Navigation = navigationItems model.CurrentPage dispatch
        Theme = model.Theme
        ThemeOptions = Theme.availableThemes
        OnThemeChange = ThemeChanged >> dispatch
        Content = pageContent model dispatch
    }