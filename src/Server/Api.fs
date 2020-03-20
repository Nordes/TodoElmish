module TodoElmish.Server.Api
open System.IO
open System.Threading.Tasks

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open FSharp.Control.Tasks.V2
open Giraffe
open Saturn
open Shared

// Use a type provider to save as csv? or .... whatever?

let apiRouter = router {
  pipe_through (pipeline { set_header "x-pipeline-type" "Api" })
  get "/todos" (fun next ctx ->
    task {
      return! json "something" next ctx
    })
  getf "/todos/%s" (fun (todoId:string) next ctx ->
    task {
      // ctx.BindModelAsync<TodoId>() // ??
      return! json "something guid?" next ctx
    })
  post "/todos" (fun next ctx ->
    task {
        // let counter = {Value = 42}
      return! json "something" next ctx
      // return! json "hello"
    })
  }

let a = 1