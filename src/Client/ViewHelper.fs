[<AutoOpen>]
module ViewHelpers

open Fable.React
open Fable.React.Props
open Fulma
open Fable.FontAwesome

module KeyCode =
    let enter = 13.
    let upArrow = 38.
    let downArrow =  40.

let onKeyDown keyCode action =
    OnKeyDown (fun (ev:Browser.Types.KeyboardEvent) ->
        if ev.keyCode = keyCode then
            ev.preventDefault()
            action ev)

let renderBtn (title:string) color icon func =
  Button.button [
    Button.Props [
      Title title
    ]
    Button.OnClick func
    Button.Color color
  ] [ Fa.i [ icon ] [] ]
