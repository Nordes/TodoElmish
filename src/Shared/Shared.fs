namespace Shared

type TodoId = TodoId of System.Guid

type TodoState =
  | InProgress
  | Pending
  | Completed
  // | Deleted

type Todo = {
    Id: TodoId
    Description: string
    State: TodoState
  }
