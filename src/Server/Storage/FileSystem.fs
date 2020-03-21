module ServerCode.Storage.FileSystem

open System.IO
// open ServerCode
open Shared
open Thoth.Json.Net

/// Get the file name used to store the data for a specific user
let getJSONFileName dbName = sprintf "./temp/db/%s.json" dbName

let getTodoListFromDB dbName=
    let fi = FileInfo(getJSONFileName dbName)
    if not fi.Exists then Defaults.defaultTodoList
    else
        File.ReadAllText(fi.FullName)
        |> Decode.Auto.unsafeFromString<Todo list>

let saveTodoListToDB todoList =
    let fi = FileInfo(getJSONFileName "todoList")
    if not fi.Directory.Exists then
        fi.Directory.Create()
    File.WriteAllText(fi.FullName, Encode.Auto.toString(2, todoList))