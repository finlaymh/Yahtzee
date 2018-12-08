Imports System.Text.RegularExpressions

Module Module1

    Sub Main()
        Console.WindowHeight = 40
        Console.WindowWidth = 80
        Dim Player1Game As New ScoreTable ' Setting up a class instance for each player
        Dim Player2Game As New ScoreTable
        Dim Playing As Boolean = True
        Dim Choice As String
        Do
            Console.WriteLine("Player One's Turn!")
            Play(Player1Game)
            Console.WriteLine("Player Two's Turn!")
            Play(Player2Game)
            Console.WriteLine("Player One's Score: " & Player1Game.Score)
            Console.WriteLine("Player Two's Score: " & Player2Game.Score)
            If Player1Game.Score > Player2Game.Score Then
                Console.WriteLine("Player One Wins!")
            ElseIf Player2Game.Score > Player1Game.Score Then
                Console.WriteLine("Player Two Wins!")
            Else
                Console.WriteLine("A Draw!")
            End If
            Console.Write("Play Again? [Y/N]: ")
            Choice = Console.ReadLine
            If Choice = "Y" Or Choice = "y" Then
                Playing = True
            Else
                Playing = False
            End If
        Loop Until Not Playing

    End Sub

    Sub Play(ByRef Game As ScoreTable) ' Holds main code, in a seperate function to allow for repeat games.
        Dim Choice As Integer
        Dim IsValid As Boolean = False
        Dim Dice As New Die
        Dim Rolls(5) As Integer
        Dim FuncRand As New Random
        Console.WriteLine("Dice Rolled:")
        For i = 0 To 4
            Call Dice.Roll(Rolls(i), i * FuncRand.Next(0, 1000)) ' Generates a seed for the roll number generator
        Next
        Dice.DisplayNumbers(Rolls)
        RollAgain(Dice, Rolls)
        Dice.DisplayNumbers(Rolls)
        Do
            Console.WriteLine()
            Game.DisplayCategories()
            Console.WriteLine("Current Points: " & Game.Score)
            Console.Write("Enter tactic to use: ")
            Try
                Choice = Console.ReadLine
                Select Case Choice
                    Case < 1
                        Console.WriteLine("Number Too Low!")
                        IsValid = False
                    Case > 7
                        Console.WriteLine("Number Too High!")
                        IsValid = False
                    Case 1
                        IsValid = Game.ThreeOfAKind(Rolls, IsValid)
                    Case 2
                        IsValid = Game.FourOfAKind(Rolls, IsValid)
                    Case 3
                        IsValid = Game.FullHouse(Rolls, IsValid)
                    Case 4
                        IsValid = Game.SmallStraight(Rolls, IsValid)
                    Case 5
                        IsValid = Game.LargeStraight(Rolls, IsValid)
                    Case 6
                        IsValid = Game.Yahtzee(Rolls, IsValid)
                    Case 7
                        IsValid = Game.Chance(Rolls, IsValid)
                End Select
            Catch ex As Exception
                Console.WriteLine("Invalid Input! ")
                IsValid = False
            End Try
            Console.WriteLine("=========================")
        Loop Until IsValid

    End Sub

    Class ScoreTable
        Public Categories(7) As String ' Probably doesn't need to exist as an array, but makes it easier to output in DisplayCategories
        Public Score As Integer

        Sub New() ' Sets up variables for a new game
            Score = 0
            Categories(0) = "Three Of A Kind"
            Categories(1) = "Four Of A Kind"
            Categories(2) = "Full House"
            Categories(3) = "Small Straight"
            Categories(4) = "Large Straight"
            Categories(5) = "Yahtzee"
            Categories(6) = "Chance"
        End Sub

        Sub DisplayCategories()
            For i = 0 To 6
                Console.WriteLine("[" & i + 1 & "]: " & Categories(i))
            Next
        End Sub

        'Below functions are for the different score methods, regex is used to validate choice

        Function ThreeOfAKind(ByVal Rolls As Array, ByRef IsValid As Boolean)
            Dim RollString As String = ArrayToString(Rolls)
            If Regex.IsMatch(RollString, "(1{3,}|2{3,}|3{3,}|4{3,}|5{3,}|6{3,})") Then
                Score = Score + Rolls(0) + Rolls(1) + Rolls(2) + Rolls(3) + Rolls(4)
                Console.WriteLine("Score: " & Score)
                IsValid = True
            Else
                Console.WriteLine("Invalid Tactic")
                IsValid = False
            End If
            Return IsValid
        End Function

        Function FourOfAKind(ByVal Rolls As Array, ByRef IsValid As Boolean)
            Dim RollString As String = ArrayToString(Rolls)
            If Regex.IsMatch(RollString, "(1{4,}|2{4,}|3{4,}|4{4,}|5{4,}|6{4,})") Then
                Score = Score + Rolls(0) + Rolls(1) + Rolls(2) + Rolls(3) + Rolls(4)
                Console.WriteLine("Score: " & Score)
                IsValid = True
            Else
                Console.WriteLine("Invalid Tactic")
                IsValid = False
            End If
            Return IsValid
        End Function

        Function FullHouse(ByVal Rolls As Array, ByRef IsValid As Boolean)
            Dim RollString As String = ArrayToString(Rolls)
            If Regex.IsMatch(RollString, "(1{3}|2{3}|3{3}|4{3}|5{3}|6{3})(1{2}|2{2}|3{2}|4{2}|5{2}|6{2})") Then
                Score = Score + 25
                Console.WriteLine("Score: " & Score)
                IsValid = True
            Else
                Console.WriteLine("Invalid Tactic")
                IsValid = False
            End If
            Return IsValid
        End Function

        Function SmallStraight(ByVal Rolls As Array, ByRef IsValid As Boolean)
            Dim RollString As String = ArrayToString(Rolls)
            If Regex.IsMatch(RollString, "(1234)|(2345)|(3456)") Then
                Score = Score + 30
                Console.WriteLine("Score: " & Score)
                IsValid = True
            Else
                Console.WriteLine("Invalid Tactic")
                IsValid = False
            End If
            Return IsValid
        End Function

        Function LargeStraight(ByVal Rolls As Array, ByRef IsValid As Boolean)
            Dim RollString As String = ArrayToString(Rolls)
            If Regex.IsMatch(RollString, "(12345)|(23456)") Then
                Score = Score + 40
                Console.WriteLine("Score: " & Score)
                IsValid = True
            Else
                Console.WriteLine("Invalid Tactic")
                IsValid = False
            End If
            Return IsValid
        End Function

        Function Yahtzee(ByVal Rolls As Array, ByRef IsValid As Boolean)
            Dim RollString As String = ArrayToString(Rolls)
            If Regex.IsMatch(RollString, "1{5}|2{5}|3{5}|4{5}|5{5}|6{5}") Then
                Score = Score + 50
                Console.WriteLine("Score: " & Score)
                IsValid = True
            Else
                Console.WriteLine("Invalid Tactic")
                IsValid = False
            End If
            Return IsValid
        End Function

        Function Chance(ByVal Rolls As Array, ByRef IsValid As Boolean)
            Score = Score + Rolls(0) + Rolls(1) + Rolls(2) + Rolls(3) + Rolls(4) + Rolls(5)
            Console.WriteLine("Score: " & Score)
            IsValid = True
            Return IsValid
        End Function

    End Class

    Class Die ' Class to hold functions and subroutines related to the dice
        Function Roll(ByRef Output, Seed)
            Dim Rand As New Random(Seed)

            Output = Rand.Next(1, 7)
            Return Output
        End Function

        Sub DisplayNumbers(ByVal Rolls As Array)
            Dim CRoll As Integer
            Array.Sort(Rolls)
            Console.Write("Numbers: ")
            For i = 0 To 4
                CRoll = Rolls(i + 1)
                Console.Write(CRoll & " ")
            Next
            Console.WriteLine()
        End Sub

    End Class

    Function ArrayToString(ByVal Arr As Array) ' Effectively the Array.Join() Function, but written by me instead to use numbers
        Dim Result As String = ""
        For i = 0 To Arr.Length - 1
            Result = Result & Arr(i)
        Next
        Return Result
    End Function

    Sub RollAgain(ByVal Dice As Die, ByVal Rolls As Array)
        Dim Choice As String
        Dim IsValid As Boolean = False
        Dim DiceToRoll As String
        Do
            Console.Write("ReRoll? [Y/N]: ")
            Choice = Console.ReadLine
            Select Case Choice
                Case "Y"
                    Console.WriteLine("Which Dice would you like to ReRoll?")
                    Console.Write("Enter the position of the dice seperated by a comma (ie. 1,2,3,4,5): ")
                    DiceToRoll = Console.ReadLine
                    Dim ChoiceArr(DiceToRoll.Length / 2) As String ' The / 2 is to account for commas.
                    ChoiceArr = DiceToRoll.Split(",")
                    Dim FuncRand As New Random
                    For i = 0 To ChoiceArr.Length - 1
                        ChoiceArr(i) = Int(ChoiceArr(i))
                        Dice.Roll(Rolls(ChoiceArr(i)), i * FuncRand.Next(0, 1000))
                    Next
                    IsValid = True
                Case "N"
                    IsValid = True
            End Select
        Loop Until IsValid

    End Sub

End Module



