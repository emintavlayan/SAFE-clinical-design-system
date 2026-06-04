module Design.Theme

open Browser.Dom
open Fable.Core.JsInterop

let private themeStorageKey = "safe-clinical-design-system.theme"

/// Returns whether the browser currently prefers a dark color scheme.
let private prefersDark () =
    let mediaQueryResult: obj = window?matchMedia ("(prefers-color-scheme: dark)")
    mediaQueryResult?matches

/// Returns the supported daisyUI theme names for this repository.
let availableThemes = [ "light"; "dark"; "corporate"; "business"; "emerald"; "synthwave" ]

/// Normalizes a requested theme to one of the supported names.
let private normalizeTheme (theme: string) =
    if availableThemes |> List.contains theme then
        theme
    else
        "light"

/// Persists the selected theme name in localStorage.
let saveTheme (theme: string) =
    let normalizedTheme = normalizeTheme theme
    window.localStorage.setItem (themeStorageKey, normalizedTheme)

/// Applies the selected theme name to the document root.
let setTheme (theme: string) =
    let normalizedTheme = normalizeTheme theme
    document.documentElement.setAttribute ("data-theme", normalizedTheme)

/// Loads the preferred theme from localStorage or falls back to the system preference.
let loadTheme () =
    let storedTheme = window.localStorage.getItem themeStorageKey

    if storedTheme |> isNull |> not then
        normalizeTheme storedTheme
    elif prefersDark () then
        "dark"
    else
        "light"