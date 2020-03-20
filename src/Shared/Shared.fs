namespace Shared

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
