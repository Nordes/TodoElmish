module TodoElmish.Server.Api

open FSharp.Control.Tasks.V2
open Giraffe
open Saturn

let apiRouter = router {
  pipe_through (pipeline { set_header "x-pipeline-type" "Api" })

  get "/todos" (fun next ctx ->
    task {
      let todoList = ServerCode.Storage.FileSystem.getTodoListFromDB "todoList"

      return! json (todoList) next ctx
    })

  post "/todos" (fun next ctx ->
    task {
      let! todoList = ctx.BindJsonAsync<Shared.Todo list>()
      ServerCode.Storage.FileSystem.saveTodoListToDB todoList
      return! json todoList next ctx
    })
  }
