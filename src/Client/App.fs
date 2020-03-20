module App

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fulma

open Shared
open Fable.FontAwesome

type TodoId = TodoId of System.Guid
type TodoState =
  | Pending
  | InProgress
  | Completed

type Todo = {
    Id: TodoId
    Description: string
    State: TodoState
  }
type Model = {
    Todos: Todo list
    NewTodo: string
  }

type Msg =
  | TodoAddNewMsg
  | TodoPendingMsg of TodoId
  | TodoInProgressMsg of TodoId
  | TodoCompleteMsg of TodoId
  | TodoRemoveMsg of TodoId
  | NewTodoTextChangeMsg of string

let generateNewTodo desc =
  { Id = System.Guid.NewGuid() |> TodoId; Description = desc; State = Pending}

let init () : Model * Cmd<Msg> =
    let initialModel = { Todos = [ generateNewTodo "test" ]; NewTodo = "" }
    initialModel, Cmd.none

let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
    match msg with
    | NewTodoTextChangeMsg desc->
        { model with NewTodo = desc }, Cmd.none

    | TodoAddNewMsg ->
        {
          model with
            Todos = (generateNewTodo model.NewTodo) :: model.Todos
            NewTodo = ""
        }, Cmd.none

    | TodoPendingMsg todoId ->
        {
          model with
            Todos = model.Todos
              |> List.map (fun t ->
                              if t.Id <> todoId then t
                              else {t with State = Pending} )
        }, Cmd.none

    | TodoCompleteMsg todoId ->
        {
          model with
            Todos = model.Todos
              |> List.map (fun t ->
                              if t.Id <> todoId then t
                              else {t with State = Completed} )
        }, Cmd.none

    | TodoInProgressMsg todoId ->
        {
          model with
            Todos = model.Todos
              |> List.map (fun t ->
                              if t.Id <> todoId then t
                              else {t with State = InProgress} )
        }, Cmd.none
    | TodoRemoveMsg todoId ->
        {model with Todos = model.Todos |> List.filter (fun t -> t.Id <> todoId )}, Cmd.none

let renderFooterDetails =
    p [] [
        str "This follow more or less from "
        a [ Href "https://www.youtube.com/watch?v=zAfW_-m1u4k" ][ str "Let's build a Todo List application with React, Elmish, F# and Fable"]
    ]

let renderTodoAdd currentNewTodo dispatch =
  Field.div [ Field.HasAddons ] [
    // Split in 2... left is text right is the add button
    Control.p [Control.IsExpanded] [
      Input.text [
        Input.Props [
            Placeholder "Ex: Todo"
            OnChange (fun ev -> ev.Value |> NewTodoTextChangeMsg |> dispatch )
            onKeyDown KeyCode.enter <| fun _ -> TodoAddNewMsg |> dispatch
          ]
        Input.Option.ValueOrDefault currentNewTodo
      ]
    ]
    Control.p [][
      Button.button [
        Button.OnClick (fun _ -> TodoAddNewMsg |> dispatch)
      ] [
        Fa.i [ Fa.Solid.Plus ] []
      ]
    ]
  ]

let renderTodo (todo:Todo) dispatch =
  Field.div [
      Field.HasAddons
    ] [
    // Split in 2... left is text right are the buttons
    Control.p [
      match todo.State with
        | InProgress -> Control.Modifiers [Modifier.TextWeight TextWeight.Bold]
        | Completed -> Control.CustomClass "strike"
        | _ -> ignore()
      Control.IsExpanded
    ] [
      Input.text [
        Input.IsReadOnly true
        Input.Value todo.Description
      ]
    ]
    Control.p [][
      match todo.State with
        | Completed -> ignore ()
        | InProgress ->
          Button.button [
            Button.Props [
              Title "Change to back -> todo"
            ]
            Button.OnClick (fun _ -> todo.Id |> TodoPendingMsg |> dispatch)
            Button.Color IsSuccess ] [ Fa.i [ Fa.Regular.CalendarCheck ] [] ]

        | Pending ->
          Button.button [
            Button.Props [
              Title "Change to in progress"
            ]
            Button.OnClick (fun _ -> todo.Id |> TodoInProgressMsg |> dispatch)
            Button.Color IsSuccess ] [ Fa.i [ Fa.Regular.Calendar ] [] ]
    ]

    Control.p [] [
      match todo.State with
        | InProgress ->
            Button.button [
              Button.Props [
                Title "Complete"
              ]
              Button.OnClick (fun _ -> todo.Id |> TodoCompleteMsg |> dispatch)
              Button.Color IsWarning ] [ Fa.i [ Fa.Solid.CheckDouble ] [] ]
        | _ -> ignore ()

      Button.button [
        Button.Props [
          Title "Delete"
        ]
        Button.OnClick (fun _ -> todo.Id |> TodoRemoveMsg |> dispatch )
        Button.Color IsDanger ] [ Fa.i [ Fa.Solid.TrashAlt ] [] ]
    ]
  ]

let renderTodos todos dispatch =
  div [] [
    for todo in todos -> renderTodo todo dispatch
  ]

let renderView (model : Model) (dispatch : Msg -> unit) =
  div [] [
    Navbar.navbar [ Navbar.Color IsPrimary ] [
      Navbar.Item.div [] [
        Heading.h2 [] [
          str "Todo's" ]
        ]
      ]

    Container.container [ Container.IsFluid ] [
      Columns.columns [] [
        Column.column [] [
          br []
          Box.box' [] [
            renderTodoAdd model.NewTodo dispatch
            renderTodos model.Todos dispatch
          ]
        ]
      ]
    ]

    Footer.footer [ ] [
      Content.content
        [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
        [ renderFooterDetails ]
    ]
  ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update renderView
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
