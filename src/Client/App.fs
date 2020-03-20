module App

open Elmish
open Elmish.React
open Fable.FontAwesome
open Fable.React
open Fable.React.Props
open Fulma
open Shared

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

let generateNewTodo desc = {
  Id = System.Guid.NewGuid() |> TodoId
  Description = desc; State = Pending
}

let init () : Model * Cmd<Msg> =
  let initialModel = {
    Todos = [ ]
    NewTodo = ""
  }
  initialModel, Cmd.none

let updateTodoStatus (todoId:TodoId) (newStatus:TodoState) (todos:Todo list) =
  todos |> List.map (fun t ->
                if t.Id <> todoId then t
                else {t with State = newStatus })
        |> List.sortBy (fun f -> f.State, f.Description)

let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
  match msg with
  | NewTodoTextChangeMsg desc->
    { model with NewTodo = desc }, Cmd.none

  | TodoAddNewMsg ->
    {
      model with
        Todos = (generateNewTodo model.NewTodo) :: model.Todos |> List.sortBy (fun f-> f.State, f.Description)
        NewTodo = ""
    }, Cmd.none

  | TodoPendingMsg todoId ->
    {
      model with
        Todos = model.Todos |> updateTodoStatus todoId Pending
    }, Cmd.none

  | TodoCompleteMsg todoId ->
    {
      model with
        Todos = model.Todos |> updateTodoStatus todoId Completed
    }, Cmd.none

  | TodoInProgressMsg todoId ->
    {
      model with
        Todos = model.Todos |> updateTodoStatus todoId InProgress
    }, Cmd.none

  | TodoRemoveMsg todoId ->
    {
      model with
        Todos = model.Todos
        |> List.filter (fun t -> t.Id <> todoId )
    }, Cmd.none

let renderFooterDetails =
  p [] [
    str "This follow more or less from "
    a
      [ Href "https://www.youtube.com/watch?v=zAfW_-m1u4k" ]
      [ str "Let's build a Todo List application with React, Elmish, F# and Fable"]
  ]

let renderTodoAdd currentNewTodo dispatch =
  Field.div [ Field.HasAddons ] [
    // Split in 2... left is text right is the add button
    Control.p [Control.IsExpanded; Control.HasIconLeft] [
      Input.text [
        Input.Props [
            Placeholder "Ex: Todo"
            OnChange (fun ev -> ev.Value |> NewTodoTextChangeMsg |> dispatch )
            onKeyDown KeyCode.enter <| fun _ -> TodoAddNewMsg |> dispatch
          ]
        Input.Option.ValueOrDefault currentNewTodo
      ]
      Icon.icon [ Icon.Size IsSmall; Icon.IsLeft ]
          [ Fa.i [ Fa.Regular.StickyNote ] [ ] ]

    ]
    Control.p [][
      renderBtn "Add new todo" IsDark Fa.Solid.Plus (fun _ -> TodoAddNewMsg |> dispatch)
    ]
  ]

let renderTodoDescription (todo:Todo) =
  Control.div [
    Control.IsExpanded
    Control.HasIconLeft
  ] [
    Input.text [
      match todo.State with
      | InProgress -> Input.Modifiers [Modifier.TextWeight TextWeight.Bold]
      | Completed ->
        Input.CustomClass "strike"
        Input.Disabled true
      | _ -> ignore()
      Input.IsReadOnly true
      Input.Value todo.Description // <== Could add the date-time it started or completed...
    ]
    Icon.icon [ Icon.Size IsSmall; Icon.IsLeft ]
              [ Fa.i [ Fa.Solid.StickyNote ] [ ] ]
  ]

let renderTodo (todo:Todo) dispatch =
  Field.div [
      Field.HasAddons
      Field.CustomClass "lessMarginBottom"
    ] [
    // Split in 2... left is text right are the buttons
    renderTodoDescription todo
    Control.p [][
      match todo.State with
        | InProgress ->
            renderBtn "Change back -> todo" IsWarning Fa.Solid.Undo
              (fun _ -> todo.Id |> TodoPendingMsg |> dispatch)

        | Pending ->
            renderBtn "Change to in progress" IsInfo Fa.Regular.Calendar
              (fun _ -> todo.Id |> TodoInProgressMsg |> dispatch)
        | Completed -> ignore ()
    ]

    Control.p [] [
      match todo.State with
        | InProgress -> renderBtn "Complete" IsSuccess Fa.Solid.CheckDouble (fun _ -> todo.Id |> TodoCompleteMsg |> dispatch)
        | _ -> ignore ()

      renderBtn "Delete" IsDanger Fa.Solid.TrashAlt (fun _ -> todo.Id |> TodoRemoveMsg |> dispatch )
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
