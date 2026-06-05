module Pages.TodoPage

open Elmish
open Feliz
open SAFE
open Shared
open Design.Components

type Model = {
    Todos: RemoteData<Todo list>
    Input: string
}

type Msg =
    | SetInput of string
    | LoadTodos of ApiCall<unit, Todo list>
    | SaveTodo of ApiCall<string, Todo list>

type TodoRow = {
    Identifier: string
    Description: string
}

let private todosApi = Api.makeProxy<ITodosApi> ()

/// Initializes the Todo sample state and starts loading the server-backed list.
let init () =
    { Todos = NotStarted; Input = "" }, LoadTodos(Start()) |> Cmd.ofMsg

/// Updates the Todo sample state in response to SAFE remote-data actions.
let update msg model =
    match msg with
    | SetInput input -> { model with Input = input }, Cmd.none
    | LoadTodos loadMsg ->
        match loadMsg with
        | Start() ->
            let command = Cmd.OfAsync.perform todosApi.getTodos () (Finished >> LoadTodos)

            {
                model with
                    Todos = model.Todos.StartLoading()
            },
            command
        | Finished todos -> { model with Todos = Loaded todos }, Cmd.none
    | SaveTodo saveMsg ->
        match saveMsg with
        | Start input when Todo.isValid input ->
            let command =
                input
                |> Todo.create
                |> fun todo -> Cmd.OfAsync.perform todosApi.addTodo todo (Finished >> SaveTodo)

            { model with Input = "" }, command
        | Start _ -> model, Cmd.none
        | Finished todos -> { model with Todos = Loaded todos }, Cmd.none

/// Maps server Todo values to compact table rows for the gallery page.
let private todoRows (todos: Todo list) : TodoRow list =
    todos
    |> List.map (fun todo -> {
        Identifier = todo.Id.ToString("N").Substring(0, 8)
        Description = todo.Description
    })

/// Renders the Todo starter page using shared design-system primitives.
let view model dispatch =
    Html.div [
        prop.className "space-y-6"
        prop.children [
            infoAlert
                "Working sample"
                "This Todo page keeps the original SAFE server contract and applies the shared SAFE Clinical Design System components."

            Html.div [
                prop.className "grid gap-6 xl:grid-cols-[minmax(0,22rem)_minmax(0,1fr)]"
                prop.children [
                    card
                        "Add item"
                        "Existing pages can adopt shared inputs and actions without changing their server model."
                        [
                            Html.div [
                                prop.className "space-y-4"
                                prop.children [
                                    textInput
                                        "Todo description"
                                        "Demonstrates a simple text input."
                                        model.Input
                                        "Describe the next task"
                                        false
                                        (SetInput >> dispatch)
                                    primaryAction "Add todo" (not (Todo.isValid model.Input)) (fun () ->
                                        dispatch (SaveTodo(Start model.Input)))
                                ]
                            ]
                        ]

                    Html.section [
                        prop.className "space-y-4"
                        prop.children [
                            sectionTitle "Todo list" "The list uses the shared empty state and table surface."

                            match model.Todos with
                            | NotStarted
                            | Loading None -> infoAlert "Loading" "Todo items are loading from the SAFE server."
                            | Loading(Some todos)
                            | Loaded todos when todos |> List.isEmpty ->
                                emptyState
                                    "No todos"
                                    "Create an item to see how the shared table pattern works in an existing feature."
                                    None
                            | Loading(Some todos)
                            | Loaded todos ->
                                simpleTable
                                    [
                                        {
                                            Header = "Id"
                                            Cell = fun (row: TodoRow) -> Html.code row.Identifier
                                        }
                                        {
                                            Header = "Description"
                                            Cell = fun (row: TodoRow) -> Html.span row.Description
                                        }
                                    ]
                                    (todoRows todos)
                        ]
                    ]
                ]
            ]
        ]
    ]