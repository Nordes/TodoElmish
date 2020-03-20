[<AutoOpen>]
module ViewHelpers

open Fable.React
open Fable.React.Props

module KeyCode =
    let enter = 13.
    let upArrow = 38.
    let downArrow =  40.

let onKeyDown keyCode action =
    OnKeyDown (fun (ev:Browser.Types.KeyboardEvent) ->
        if ev.keyCode = keyCode then
            ev.preventDefault()
            action ev)
