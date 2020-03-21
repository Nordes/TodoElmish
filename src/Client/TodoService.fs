module TodoService

open Fable.Core.JsInterop
open Fetch.Types
open Shared
open Thoth.Json

/// Get the wish list from the server, used to populate the model
let getTodoList () =
  promise {
    let url = "/api/todos"
    let props = [ ]

    let! res = Fetch.fetch url props
    let! rawJson = res.text()
    return Decode.Auto.unsafeFromString<Todo list> rawJson
  }

let postTodoList (todoList:Todo list) =
    promise {
        let url = "/api/todos"
        let body = Encode.Auto.toString(0, todoList)
        let props =
            [ Method HttpMethod.POST
              Fetch.requestHeaders [
                // Authorization ("Bearer " + token)
                ContentType "application/json" ]
              Body !^body ]

        let! res = Fetch.fetch url props
        let! txt = res.text()
        return Decode.Auto.unsafeFromString<Todo list> txt
    }