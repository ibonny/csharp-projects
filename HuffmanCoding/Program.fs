open System
open System.Text

type HuffmanNode =
    | Leaf of byte
    | Branch of HuffmanNode * HuffmanNode

let buildTree (bytes: byte array) =
    let leaves =
        bytes
        |> Array.countBy id
        |> Array.map (fun (symbol, frequency) -> frequency, symbol, Leaf symbol)
        |> Array.toList

    let rec merge nodes =
        match nodes |> List.sortBy (fun (frequency, minSymbol, _) -> frequency, minSymbol) with
        | [] -> None
        | [ _, _, tree ] -> Some tree
        | (leftFrequency, leftMin, leftTree) :: (rightFrequency, rightMin, rightTree) :: rest ->
            let combined =
                (leftFrequency + rightFrequency, min leftMin rightMin, Branch(leftTree, rightTree))

            merge (combined :: rest)

    merge leaves

let buildCodeTable tree =
    let rec collect prefix node codes =
        match node with
        | Leaf symbol ->
            let code = if prefix = "" then "0" else prefix
            Map.add symbol code codes
        | Branch(left, right) ->
            let codesWithLeft = collect (prefix + "0") left codes
            collect (prefix + "1") right codesWithLeft

    collect "" tree Map.empty

let encode (input: string) =
    let bytes = Encoding.UTF8.GetBytes input

    match buildTree bytes with
    | None -> ""
    | Some tree ->
        let codeTable = buildCodeTable tree

        codeTable
        |> Seq.iter (fun entry -> printfn "('%c', %s)" (char entry.Key) entry.Value)

        bytes
        |> Array.map (fun symbol -> Map.find symbol codeTable)
        |> String.concat ""

let groupIntoBytes (bits: string) =
    bits
    |> Seq.toArray
    |> Array.chunkBySize 8
    |> Array.map (fun chunk -> String(chunk).PadRight(8, '0'))
    |> String.concat " "

[<EntryPoint>]
let main args =
    match args with
    | [| input |] ->
        input
        |> encode
        |> groupIntoBytes
        |> printfn "%s"
        0
    | [||] ->
        eprintfn "Usage: HuffmanCoding <text>"
        1
    | _ ->
        eprintfn "Provide the input as a single quoted command-line argument."
        eprintfn "Usage: HuffmanCoding <text>"
        1