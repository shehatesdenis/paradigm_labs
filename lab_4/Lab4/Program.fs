// Лабораторная работа №4. Функции и обработка данных в F#
// Выполнил: Штыка Д.А., группа ИУ5-35Б, вариант 23
module Lab4

open System

// ---------------------------------------------------------------
// 1. Функция, возвращающая кортеж: параметры кортежем и каррированно
// ---------------------------------------------------------------
// Возвращает (целая часть, остаток) от деления a на b
let divModTupled (a: int, b: int) = (a / b, a % b)

let divModCurried (a: int) (b: int) = (a / b, a % b)

// ---------------------------------------------------------------
// 2. Простой рекурсивный алгоритм: сумма целых чисел в диапазоне [a; b]
// ---------------------------------------------------------------
let rec sumRange (a: int) (b: int) : int =
    if a > b then 0
    else a + sumRange (a + 1) b

// ---------------------------------------------------------------
// 3. Хвостовая рекурсия (накопитель acc)
// ---------------------------------------------------------------
let sumRangeTail (a: int) (b: int) : int =
    let rec loop acc i =
        if i > b then acc
        else loop (acc + i) (i + 1)
    loop 0 a

// ---------------------------------------------------------------
// 4. Конечный автомат из трёх состояний (взаимно-рекурсивные функции)
//    Алфавит: '0' и '1'.
//    S0: '0' -> S0, '1' -> S1
//    S1: '0' -> S2, '1' -> S1
//    S2: '0' -> S0, '1' -> S1
//    Строка "принимается", если автомат закончил работу в S2
//    (т.е. строка оканчивается на "10").
// ---------------------------------------------------------------
let rec state0 (input: char list) : string =
    match input with
    | [] -> "S0"
    | '1' :: rest ->
        printf "S0 -1-> S1; "
        state1 rest
    | _ :: rest ->
        printf "S0 -0-> S0; "
        state0 rest

and state1 (input: char list) : string =
    match input with
    | [] -> "S1"
    | '0' :: rest ->
        printf "S1 -0-> S2; "
        state2 rest
    | _ :: rest ->
        printf "S1 -1-> S1; "
        state1 rest

and state2 (input: char list) : string =
    match input with
    | [] -> "S2"
    | '1' :: rest ->
        printf "S2 -1-> S1; "
        state1 rest
    | _ :: rest ->
        printf "S2 -0-> S0; "
        state0 rest

let runAutomaton (s: string) : string =
    state0 (Seq.toList s)

// ---------------------------------------------------------------
// 5. Три целых числа и лямбда-выражение для суммирования
// ---------------------------------------------------------------
let apply3Tupled (a: int, b: int, c: int, f: int -> int -> int -> int) : int =
    f a b c

let apply3Curried (a: int) (b: int) (c: int) (f: int -> int -> int -> int) : int =
    f a b c

// ---------------------------------------------------------------
// 6. Обобщённая функция с тремя параметрами, возвращающая кортеж
// ---------------------------------------------------------------
let tuple3 (a: 'a) (b: 'b) (c: 'c) = (a, b, c)

// Типы не указаны явно, но выражения в теле фиксируют int, float, string
let tuple3Typed a b c = (a + 0, b + 0.0, c + "")

// ---------------------------------------------------------------
// 7. Двухэтапное создание обобщённых функций
//    Этап 1: обобщённая функция, принимающая операцию сложения.
//    Этап 2: специализация для конкретных типов.
// ---------------------------------------------------------------
let addGeneric (add: 'a -> 'a -> 'a) (x: 'a) (y: 'a) (z: 'a) : 'a =
    add (add x y) z

let addInt3 : int -> int -> int -> int = addGeneric (+)
let addFloat3 : float -> float -> float -> float = addGeneric (+)
let addString3 : string -> string -> string -> string = addGeneric (+)

// ---------------------------------------------------------------
// 8. List comprehension: для чётных элементов [1..10] кортеж (x, x^2, x^3)
// ---------------------------------------------------------------
let evenTuples =
    [ for x in [1 .. 10] do
        if x % 2 = 0 then
            yield (x, x * x, x * x * x) ]

// ---------------------------------------------------------------
// 9. Квадраты элементов списка через Head и Tail
// ---------------------------------------------------------------
// Вариант 1: оператор if
let rec squaresIf (lst: int list) : int list =
    if lst.IsEmpty then []
    else (lst.Head * lst.Head) :: squaresIf lst.Tail

// Вариант 2: сопоставление с образцом на уровне функции
let rec squaresMatch : int list -> int list =
    function
    | [] -> []
    | lst -> (lst.Head * lst.Head) :: squaresMatch lst.Tail

// ---------------------------------------------------------------
// 10. map, sort, filter, fold, zip и агрегирование (три комбинации)
// ---------------------------------------------------------------
let data = [5; 3; 8; 1; 9; 2; 7; 4; 6; 10]

let isEven (x: int) = x % 2 = 0
let square (x: int) = x * x

// Комбинация 1: заканчивается агрегированием (List.sum)
let comb1 =
    List.sum (List.map square (List.filter isEven (List.sort data)))

// Комбинация 2: заканчивается fold, результат тот же
let comb2 =
    List.fold (fun acc x -> acc + x) 0 (List.map square (List.filter isEven (List.sort data)))

// Комбинация 3: заканчивается zip (соединяет два списка)
let comb3 =
    let evens = List.filter isEven (List.sort data)
    List.zip evens (List.map square evens)

// ---------------------------------------------------------------
// 11. То же с оператором потока |>
// ---------------------------------------------------------------
let comb1Pipe =
    data
    |> List.sort
    |> List.filter isEven
    |> List.map square
    |> List.sum

let comb2Pipe =
    data
    |> List.sort
    |> List.filter isEven
    |> List.map square
    |> List.fold (fun acc x -> acc + x) 0

let comb3Pipe =
    let evens =
        data
        |> List.sort
        |> List.filter isEven
    evens
    |> List.map square
    |> List.zip evens

// ---------------------------------------------------------------
// 12. То же с оператором композиции >>
// ---------------------------------------------------------------
let comb1Comp : int list -> int =
    List.sort
    >> List.filter isEven
    >> List.map square
    >> List.sum

let comb2Comp : int list -> int =
    List.sort
    >> List.filter isEven
    >> List.map square
    >> List.fold (fun acc x -> acc + x) 0

let comb3Comp : int list -> (int * int) list =
    List.sort
    >> List.filter isEven
    >> (fun evens -> List.zip evens (List.map square evens))

// ---------------------------------------------------------------
// Точка входа
// ---------------------------------------------------------------
let header (title: string) =
    printfn ""
    printfn "=== %s ===" title

[<EntryPoint>]
let main _ =
    Console.OutputEncoding <- Text.Encoding.UTF8

    header "1. Кортеж: параметры кортежем / каррированно"
    printfn "divModTupled (17, 5)  = %A" (divModTupled (17, 5))
    printfn "divModCurried 17 5    = %A" (divModCurried 17 5)

    header "2. Рекурсия: сумма чисел от 1 до 100"
    printfn "sumRange 1 100 = %d" (sumRange 1 100)

    header "3. Хвостовая рекурсия: сумма чисел от 1 до 100"
    printfn "sumRangeTail 1 100 = %d" (sumRangeTail 1 100)

    header "4. Конечный автомат (взаимная рекурсия)"
    for s in ["0110"; "111"; "1010"; ""] do
        printf "Вход \"%s\": " s
        let final = runAutomaton s
        printfn "-> конечное состояние %s, строка %s" final (if final = "S2" then "принята" else "не принята")

    header "5. Три числа и лямбда"
    printfn "apply3Tupled (1, 2, 3, fun x y z -> x + y + z)  = %d" (apply3Tupled (1, 2, 3, (fun x y z -> x + y + z)))
    printfn "apply3Curried 1 2 3 (fun x y z -> x + y + z)    = %d" (apply3Curried 1 2 3 (fun x y z -> x + y + z))

    header "6. Обобщённая функция, возвращающая кортеж"
    printfn "tuple3 1 2.5 \"abc\"       = %A" (tuple3 1 2.5 "abc")
    printfn "tuple3 true 'x' [1; 2]   = %A" (tuple3 true 'x' [1; 2])
    printfn "tuple3Typed 1 2.5 \"abc\"  = %A" (tuple3Typed 1 2.5 "abc")

    header "7. Двухэтапные обобщённые функции"
    printfn "addInt3 1 2 3                = %d" (addInt3 1 2 3)
    printfn "addFloat3 1.5 2.5 3.0        = %f" (addFloat3 1.5 2.5 3.0)
    printfn "addString3 \"a\" \"b\" \"c\"       = %s" (addString3 "a" "b" "c")

    header "8. List comprehension"
    printfn "%A" evenTuples

    header "9. Квадраты через Head и Tail"
    printfn "squaresIf    [1; 2; 3; 4; 5] = %A" (squaresIf [1; 2; 3; 4; 5])
    printfn "squaresMatch [1; 2; 3; 4; 5] = %A" (squaresMatch [1; 2; 3; 4; 5])

    header "10. map / sort / filter / fold / zip"
    printfn "Исходный список: %A" data
    printfn "Комбинация 1 (sum):  %d" comb1
    printfn "Комбинация 2 (fold): %d" comb2
    printfn "Комбинация 3 (zip):  %A" comb3

    header "11. Оператор потока |>"
    printfn "Комбинация 1: %d" comb1Pipe
    printfn "Комбинация 2: %d" comb2Pipe
    printfn "Комбинация 3: %A" comb3Pipe

    header "12. Оператор композиции >>"
    printfn "Комбинация 1: %d" (comb1Comp data)
    printfn "Комбинация 2: %d" (comb2Comp data)
    printfn "Комбинация 3: %A" (comb3Comp data)

    0
