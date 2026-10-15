let cache = System.Collections.Generic.Dictionary<int64, int64>()

let rec fibonacci n =
    match cache.TryGetValue n with
    | true, result -> result
    | false, _ ->
        let result =
            match n with
            | 1L
            | 2L -> 1L
            | _ -> fibonacci (n - 1L) + fibonacci (n - 2L)

        cache.Add(n, result)
        result

[<EntryPoint>]
let main args =
    match args with
    | [||] ->
        eprintfn "Usage: FSharpTest1 <n>"
        1
    | _ ->
        match System.Int64.TryParse args[0] with
        | true, n when n > 0L ->
            printfn "%d" (fibonacci n)
            0
        | true, _ ->
            eprintfn "The Fibonacci index must be a positive integer."
            1
        | false, _ ->
            eprintfn "The Fibonacci index must be a positive integer."
            1