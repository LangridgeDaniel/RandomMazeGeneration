Public Class Menu

    Public Sub Menu(ByRef Maze(,) As String, ByRef width As Integer, ByVal height As Integer, ByVal maze_visited(,) As Boolean, ByRef Display_Each_Itteration As Boolean, ByRef Maze_solved(,) As String)

        Console.Clear()

        Dim Menu As New Menu

        Dim Ignore As Boolean

        Dim Last As Integer = 1

        Dim MenuKey As ConsoleKey = ConsoleKey.K
        Dim MenuSelection As Integer = 1

        Console.Title = "Random Maze Generation Investigation"
        Console.CursorVisible = False

        Menu.Menu_First() 'Print the initial menu

        Do Until MenuKey = ConsoleKey.Enter 'keep looping until the user presses enter

            MenuKey = Console.ReadKey().Key 'takes the users key input
            Console.SetCursorPosition(0, 20)

            Ignore = False

            If maze_visited(0, 0) = True Then 'checks to see if a maze has been generated yet or not
                Select Case MenuKey 'unlock Post Generation stuff
                    Case ConsoleKey.DownArrow
                        If MenuSelection = 14 Then 'if at bottom, go to top
                            MenuSelection = 1
                        Else
                            MenuSelection += 1
                        End If
                    Case ConsoleKey.UpArrow
                        If MenuSelection = 1 Then 'if at top, go to bottom
                            MenuSelection = 14
                        Else
                            MenuSelection -= 1
                        End If
                    Case Else
                        Ignore = True 'this is used to remove wrong key inputs. as if the uses enters a wrong letter, it will show on the menu without this
                        Console.SetCursorPosition(0, 20)
                        Console.Write("     ")
                End Select

            Else
                Select Case MenuKey
                    Case ConsoleKey.DownArrow 'Lock Post Generation stuff
                        If MenuSelection = 9 Then 'same as above. only skipping post generation
                            MenuSelection = 14
                        ElseIf MenuSelection = 14 Then
                            MenuSelection = 1
                        Else
                            MenuSelection += 1
                        End If
                    Case ConsoleKey.UpArrow 'same as above. only skipping post generation
                        If MenuSelection = 14 Then
                            MenuSelection = 9
                        ElseIf MenuSelection = 1 Then
                            MenuSelection = 14
                        Else
                            MenuSelection -= 1
                        End If
                    Case Else
                        Ignore = True 'this is used to remove wrong key inputs. as if the uses enters a wrong letter, it will show on the menu without this
                        Console.SetCursorPosition(0, 20)
                        Console.Write("     ")
                End Select
            End If

            Select Case MenuSelection 'depending on MenuSelection, set the cursor position to the correct line and write out the menu option with the > < to signify whats selected
                Case 1
                    Console.SetCursorPosition(0, 1)
                    Console.WriteLine(">Change the Dimensions of the maze<")
                Case 2
                    Console.SetCursorPosition(0, 2)
                    Console.WriteLine(">Generation Settings<")
                Case 3
                    Console.SetCursorPosition(0, 5)
                    Console.WriteLine(">Recursive Backtracking<")
                Case 4
                    Console.SetCursorPosition(0, 6)
                    Console.WriteLine(">Aldous-Broder<")
                Case 5
                    Console.SetCursorPosition(0, 7)
                    Console.WriteLine(">Wilson's<")
                Case 6
                    Console.SetCursorPosition(0, 8)
                    Console.WriteLine(">Hunt-and-Kill<")
                Case 7
                    Console.SetCursorPosition(0, 9)
                    Console.WriteLine(">Binary Tree<")
                Case 8
                    Console.SetCursorPosition(0, 10)
                    Console.WriteLine(">Sidewinder<")
                Case 9
                    Console.SetCursorPosition(0, 11)
                    Console.WriteLine(">Load a Maze from text document<")
                Case 10
                    Console.SetCursorPosition(0, 14)
                    Console.WriteLine(">View Last Generated Maze<")
                Case 11
                    Console.SetCursorPosition(0, 15)
                    Console.WriteLine(">Solve Last Generated Maze<")
                Case 12
                    Console.SetCursorPosition(0, 16)
                    Console.WriteLine(">Play last generated maze<")
                Case 13
                    Console.SetCursorPosition(0, 17)
                    Console.WriteLine(">Save last maze to text document<")
                Case 14
                    Console.SetCursorPosition(0, 19)
                    Console.WriteLine(">Exit<")
            End Select

            If Last <> -1 And Ignore = False Then 'use the last variable to do the same as above, however to remove the > < from the last menu option
                Select Case Last
                    Case 1
                        Console.SetCursorPosition(0, 1)
                        Console.WriteLine(" Change the Dimensions of the maze ")
                    Case 2
                        Console.SetCursorPosition(0, 2)
                        Console.WriteLine(" Generation Settings ")
                    Case 3
                        Console.SetCursorPosition(0, 5)
                        Console.WriteLine(" Recursive Backtracking ")
                    Case 4
                        Console.SetCursorPosition(0, 6)
                        Console.WriteLine(" Aldous-Broder ")
                    Case 5
                        Console.SetCursorPosition(0, 7)
                        Console.WriteLine(" Wilson's ")
                    Case 6
                        Console.SetCursorPosition(0, 8)
                        Console.WriteLine(" Hunt-and-Kill ")
                    Case 7
                        Console.SetCursorPosition(0, 9)
                        Console.WriteLine(" Binary Tree ")
                    Case 8
                        Console.SetCursorPosition(0, 10)
                        Console.WriteLine(" Sidewinder ")
                    Case 9
                        Console.SetCursorPosition(0, 11)
                        Console.WriteLine(" Load a Maze from text document ")
                    Case 10
                        Console.SetCursorPosition(0, 14)
                        Console.WriteLine(" View Last Generated Maze ")
                    Case 11
                        Console.SetCursorPosition(0, 15)
                        Console.WriteLine(" Solve Last Generated Maze ")
                    Case 12
                        Console.SetCursorPosition(0, 16)
                        Console.WriteLine(" Play last generated maze ")
                    Case 13
                        Console.SetCursorPosition(0, 17)
                        Console.WriteLine(" Save last maze to text document ")
                    Case 14
                        Console.SetCursorPosition(0, 19)
                        Console.WriteLine(" Exit ")
                End Select
            End If

            Last = MenuSelection 'to tell the program what to remove the > < from on the next run through
            Console.SetCursorPosition(0, 20)

        Loop

        Select Case MenuSelection 'What to run when the user presses enter
            Case 1 'Change Dimension
                If maze_visited(0, 0) = True Then
                    Console.Write("Are you sure? This will overwrite your currently stored maze (Y or N): ")
                    Dim Answer As String = Console.ReadLine()

                    If Answer = "N" Or Answer = "n" Then

                    Else
                        Clear(Maze, width, height, maze_visited)
                        Set_Dimensions(height, width, "User", Display_Each_Itteration, "")
                    End If
                Else
                    Clear(Maze, width, height, maze_visited)
                    Set_Dimensions(height, width, "User", Display_Each_Itteration, "")
                End If

            Case 2 'Display Settings sub-menu
                Dim Settings_MenuKey As ConsoleKey = ConsoleKey.K
                Dim Settings_MenuSelection As Integer = 1
                Console.CursorVisible = False
                Menu.Menu_Settings_Display(Display_Each_Itteration)

                Dim Done As Boolean = False 'This menu system is exactly the same as the manu its currently running in. The only difference is the number of options and the options to be displayed
                'Therefore I feel there is no need to comment this
                Do
                    Do Until Settings_MenuKey = ConsoleKey.Enter
                        Settings_MenuKey = Console.ReadKey().Key

                        Select Case Settings_MenuKey
                            Case ConsoleKey.DownArrow
                                If Settings_MenuSelection = 2 Then
                                    Settings_MenuSelection = 1
                                Else
                                    Settings_MenuSelection += 1
                                End If

                            Case ConsoleKey.UpArrow
                                If Settings_MenuSelection = 1 Then
                                    Settings_MenuSelection = 2
                                Else
                                    Settings_MenuSelection -= 1
                                End If

                            Case ConsoleKey.RightArrow
                                If Settings_MenuSelection = 1 Then
                                    If Display_Each_Itteration = True Then
                                        Display_Each_Itteration = False
                                    Else
                                        Display_Each_Itteration = True
                                    End If
                                End If

                            Case ConsoleKey.LeftArrow
                                If Settings_MenuSelection = 1 Then
                                    If Display_Each_Itteration = True Then
                                        Display_Each_Itteration = False
                                    Else
                                        Display_Each_Itteration = True
                                    End If
                                End If
                        End Select

                        Select Case Settings_MenuSelection
                            Case 1
                                Menu.Menu_Settings_Display(Display_Each_Itteration)
                            Case 2
                                Menu.Menu_Settings_Back(Display_Each_Itteration)
                        End Select
                    Loop

                    Select Case Settings_MenuSelection
                        Case 1
                            If Display_Each_Itteration = True Then
                                Display_Each_Itteration = False
                            Else
                                Display_Each_Itteration = True
                            End If
                            Menu.Menu_Settings_Display(Display_Each_Itteration)
                            Settings_MenuKey = ConsoleKey.K
                        Case 2
                            Done = True
                    End Select

                Loop Until Done = True

                MenuSelection = 1

            Case 3 'Recursive Backtracking

                'Options 3 to 8 are extremly similar (only one major difference), therefore I will only comment this one (other than the major change). 

                Dim StartTime_seconds As DateTime = Date.Now 'Start time
                Dim StartTime_Milli As DateTime = Date.Now

                Dim User_answer As String
                Dim Valid_Answer As Boolean

                Clear(Maze, width, height, maze_visited) 'reset the required variables
                Recursive_Backtracking(Maze, width, height, maze_visited, Display_Each_Itteration) 'run the algorithm

                Dim FinishTime_Seconds As DateTime = Date.Now 'end times
                Dim FinishTime_Milli As DateTime = Date.Now
                Dim TimeTaken_seconds As TimeSpan = FinishTime_Seconds - StartTime_seconds 'calculate the time taken in seconds and miliseconds
                Dim TimeTaken_Milli As TimeSpan = FinishTime_Milli - StartTime_Milli

                Console.WriteLine("Time Taken: " & TimeTaken_seconds.Seconds & "." & TimeTaken_Milli.Milliseconds & " seconds") 'output the time taken
                Console.WriteLine()
                Console.Write("Would you like to Pathfind this maze? (Y or N): ") 'Ask the user if they wish to pathfind the maze without having to go back to the main menu
                User_answer = UCase(Console.ReadLine())
                If User_answer = "Y" Then
                    Do
                        Console.WriteLine()
                        Console.Write("Would you like to solve using Breadth First (B) or A* Pathfinding (A): ")
                        User_answer = UCase(Console.ReadLine())
                        Valid_Answer = True
                        If User_answer = "B" Then
                            Solve_Last_Recursive(Maze, width, height, Maze_solved, Display_Each_Itteration) 'Run Breadth first pathfinding
                        ElseIf User_answer = "A" Then
                            Solve_Last_AStar(Maze, width, height, Maze_solved, Display_Each_Itteration) 'Run A* Pathfinding
                        Else
                            Valid_Answer = False
                            Console.WriteLine("Invalid answer, try again")
                            Console.WriteLine()
                        End If
                    Loop Until Valid_Answer = True

                    Console.WriteLine()
                    Console.WriteLine("Press enter to return to the menu")
                    Console.WriteLine()
                ElseIf User_answer = "N" Then
                    Console.WriteLine()
                    Console.WriteLine("Press enter to return to menu")
                    Console.ReadLine()
                Else
                    Console.WriteLine()
                    Console.WriteLine("Invalid answer taken as no, returning to menu")
                    Console.WriteLine("Press enter to return to menu")
                    Console.ReadLine()
                End If

            Case 4 'Aldous-Broder

                'See "Case 3"

                Dim StartTime_seconds As DateTime = Date.Now
                Dim StartTime_Milli As DateTime = Date.Now
                Dim StartTime_Min As DateTime = Date.Now

                Dim User_answer As String
                Dim Valid_Answer As Boolean

                Clear(Maze, width, height, maze_visited)
                Aldous_Broder(Maze, width, height, maze_visited, Display_Each_Itteration)

                Dim FinishTime_seconds As DateTime = Date.Now
                Dim FinishTime_Milli As DateTime = Date.Now
                Dim FinishTime_Min As DateTime = Date.Now 'Here is the major diffrence. Since this algorithm is extremly inefficient and could theoretically take an infinite amount of time to 
                Dim TimeTaken_seconds As TimeSpan = FinishTime_seconds - StartTime_seconds 'generate (just watching it generate should prove that point), I am timing it in minutes and seconds rather than seconds and miliseconds. 
                Dim TimeTaken_Milli As TimeSpan = FinishTime_Milli - StartTime_Milli 'The process is the same, it will just look slightly different when the program is actually running
                Dim TimeTaken_min As TimeSpan = FinishTime_Min - StartTime_Min

                Console.WriteLine("Time Taken: " & TimeTaken_min.Minutes & "." & TimeTaken_seconds.Seconds & " Minutes")
                Console.WriteLine()
                Console.Write("Would you like to Pathfind this maze? (Y or N): ")
                User_answer = UCase(Console.ReadLine())
                If User_answer = "Y" Then
                    Do
                        Console.WriteLine()
                        Console.Write("Would you like to solve using Breadth First (B) or A* Pathfinding (A): ")
                        User_answer = UCase(Console.ReadLine())
                        Valid_Answer = True
                        If User_answer = "B" Then
                            Solve_Last_Recursive(Maze, width, height, Maze_solved, Display_Each_Itteration)
                        ElseIf User_answer = "A" Then
                            Solve_Last_AStar(Maze, width, height, Maze_solved, Display_Each_Itteration)
                        Else
                            Valid_Answer = False
                            Console.WriteLine("Invalid answer, try again")
                            Console.WriteLine()
                        End If
                    Loop Until Valid_Answer = True

                    Console.WriteLine()
                    Console.WriteLine("Press enter to return to the menu")
                    Console.WriteLine()
                ElseIf User_answer = "N" Then
                    Console.WriteLine()
                    Console.WriteLine("Press enter to return to menu")
                    Console.ReadLine()
                Else
                    Console.WriteLine()
                    Console.WriteLine("Invalid answer taken as no, returning to menu")
                    Console.WriteLine("Press enter to return to menu")
                    Console.ReadLine()
                End If

            Case 5 'Wilsons

                'See "Case 3"

                Dim StartTime_seconds As DateTime = Date.Now
                Dim StartTime_Milli As DateTime = Date.Now

                Dim User_answer As String
                Dim Valid_Answer As Boolean

                Clear(Maze, width, height, maze_visited)
                Wilsons(Maze, width, height, maze_visited, Display_Each_Itteration)

                Dim FinishTime_seconds As DateTime = Date.Now
                Dim FinishTime_Milli As DateTime = Date.Now
                Dim TimeTaken_seconds As TimeSpan = FinishTime_seconds - StartTime_seconds
                Dim TimeTaken_Milli As TimeSpan = FinishTime_Milli - StartTime_Milli

                Console.WriteLine("Time Taken: " & TimeTaken_seconds.Seconds & "." & TimeTaken_Milli.Milliseconds & " seconds")
                Console.WriteLine()
                Console.Write("Would you like to Pathfind this maze? (Y or N): ")
                User_answer = UCase(Console.ReadLine())
                If User_answer = "Y" Then
                    Do
                        Console.WriteLine()
                        Console.Write("Would you like to solve using Breadth First (B) or A* Pathfinding (A): ")
                        User_answer = UCase(Console.ReadLine())
                        Valid_Answer = True
                        If User_answer = "B" Then
                            Solve_Last_Recursive(Maze, width, height, Maze_solved, Display_Each_Itteration)
                        ElseIf User_answer = "A" Then
                            Solve_Last_AStar(Maze, width, height, Maze_solved, Display_Each_Itteration)
                        Else
                            Valid_Answer = False
                            Console.WriteLine("Invalid answer, try again")
                            Console.WriteLine()
                        End If
                    Loop Until Valid_Answer = True

                    Console.WriteLine()
                    Console.WriteLine("Press enter to return to the menu")
                    Console.WriteLine()
                ElseIf User_answer = "N" Then
                    Console.WriteLine()
                    Console.WriteLine("Press enter to return to menu")
                    Console.ReadLine()
                Else
                    Console.WriteLine()
                    Console.WriteLine("Invalid answer taken as no, returning to menu")
                    Console.WriteLine("Press enter to return to menu")
                    Console.ReadLine()
                End If

            Case 6 'Hunt & Kill

                'See "Case 3"

                Dim StartTime_seconds As DateTime = Date.Now
                Dim StartTime_Milli As DateTime = Date.Now

                Dim User_answer As String
                Dim Valid_Answer As Boolean

                Clear(Maze, width, height, maze_visited)
                Hunt_And_Kill(Maze, width, height, maze_visited, Display_Each_Itteration)

                Dim FinishTime_seconds As DateTime = Date.Now
                Dim FinishTime_Milli As DateTime = Date.Now
                Dim TimeTaken_seconds As TimeSpan = FinishTime_seconds - StartTime_seconds
                Dim TimeTaken_Milli As TimeSpan = FinishTime_Milli - StartTime_Milli

                Console.WriteLine("Time Taken: " & TimeTaken_seconds.Seconds & "." & TimeTaken_Milli.Milliseconds & " seconds")
                Console.WriteLine()
                Console.Write("Would you like to Pathfind this maze? (Y or N): ")
                User_answer = UCase(Console.ReadLine())
                If User_answer = "Y" Then
                    Do
                        Console.WriteLine()
                        Console.Write("Would you like to solve using Breadth First (B) or A* Pathfinding (A): ")
                        User_answer = UCase(Console.ReadLine())
                        Valid_Answer = True
                        If User_answer = "B" Then
                            Solve_Last_Recursive(Maze, width, height, Maze_solved, Display_Each_Itteration)
                        ElseIf User_answer = "A" Then
                            Solve_Last_AStar(Maze, width, height, Maze_solved, Display_Each_Itteration)
                        Else
                            Valid_Answer = False
                            Console.WriteLine("Invalid answer, try again")
                            Console.WriteLine()
                        End If
                    Loop Until Valid_Answer = True

                    Console.WriteLine()
                    Console.WriteLine("Press enter to return to the menu")
                    Console.WriteLine()
                ElseIf User_answer = "N" Then
                    Console.WriteLine()
                    Console.WriteLine("Press enter to return to menu")
                    Console.ReadLine()
                Else
                    Console.WriteLine()
                    Console.WriteLine("Invalid answer taken as no, returning to menu")
                    Console.WriteLine("Press enter to return to menu")
                    Console.ReadLine()
                End If

            Case 7 'Binary Tree

                'See "Case 3"

                Dim StartTime_seconds As DateTime = Date.Now
                Dim StartTime_Milli As DateTime = Date.Now

                Dim User_answer As String
                Dim Valid_Answer As Boolean

                Clear(Maze, width, height, maze_visited)
                Binary_Tree(Maze, width, height, maze_visited, Display_Each_Itteration)

                Dim FinishTime_seconds As DateTime = Date.Now
                Dim FinishTime_Milli As DateTime = Date.Now
                Dim TimeTaken_seconds As TimeSpan = FinishTime_seconds - StartTime_seconds
                Dim TimeTaken_Milli As TimeSpan = FinishTime_Milli - StartTime_Milli

                Console.WriteLine("Time Taken: " & TimeTaken_seconds.Seconds & "." & TimeTaken_Milli.Milliseconds & " seconds")
                Console.WriteLine()
                Console.Write("Would you like to Pathfind this maze? (Y or N): ")
                User_answer = UCase(Console.ReadLine())
                If User_answer = "Y" Then
                    Do
                        Console.WriteLine()
                        Console.Write("Would you like to solve using Breadth First (B) or A* Pathfinding (A): ")
                        User_answer = UCase(Console.ReadLine())
                        Valid_Answer = True
                        If User_answer = "B" Then
                            Solve_Last_Recursive(Maze, width, height, Maze_solved, Display_Each_Itteration)
                        ElseIf User_answer = "A" Then
                            Solve_Last_AStar(Maze, width, height, Maze_solved, Display_Each_Itteration)
                        Else
                            Valid_Answer = False
                            Console.WriteLine("Invalid answer, try again")
                            Console.WriteLine()
                        End If
                    Loop Until Valid_Answer = True

                    Console.WriteLine()
                    Console.WriteLine("Press enter to return to the menu")
                    Console.WriteLine()
                ElseIf User_answer = "N" Then
                    Console.WriteLine()
                    Console.WriteLine("Press enter to return to menu")
                    Console.ReadLine()
                Else
                    Console.WriteLine()
                    Console.WriteLine("Invalid answer taken as no, returning to menu")
                    Console.WriteLine("Press enter to return to menu")
                    Console.ReadLine()
                End If

            Case 8 'Sidewinder

                'See "Case 3"

                Dim StartTime_seconds As DateTime = Date.Now
                Dim StartTime_Milli As DateTime = Date.Now

                Dim User_answer As String
                Dim Valid_Answer As Boolean

                Clear(Maze, width, height, maze_visited)
                Sidewinder(Maze, width, height, maze_visited, Display_Each_Itteration)

                Dim FinishTime_seconds As DateTime = Date.Now
                Dim FinishTime_Milli As DateTime = Date.Now
                Dim TimeTaken_seconds As TimeSpan = FinishTime_seconds - StartTime_seconds
                Dim TimeTaken_Milli As TimeSpan = FinishTime_Milli - StartTime_Milli

                Console.WriteLine("Time Taken: " & TimeTaken_seconds.Seconds & "." & TimeTaken_Milli.Milliseconds & " seconds")
                Console.WriteLine()
                Console.Write("Would you like to Pathfind this maze? (Y or N): ")
                User_answer = UCase(Console.ReadLine())
                If User_answer = "Y" Then
                    Do
                        Console.WriteLine()
                        Console.Write("Would you like to solve using Breadth First (B) or A* Pathfinding (A): ")
                        User_answer = UCase(Console.ReadLine())
                        Valid_Answer = True
                        If User_answer = "B" Then
                            Solve_Last_Recursive(Maze, width, height, Maze_solved, Display_Each_Itteration)
                        ElseIf User_answer = "A" Then
                            Solve_Last_AStar(Maze, width, height, Maze_solved, Display_Each_Itteration)
                        Else
                            Valid_Answer = False
                            Console.WriteLine("Invalid answer, try again")
                            Console.WriteLine()
                        End If
                    Loop Until Valid_Answer = True

                    Console.WriteLine()
                    Console.WriteLine("Press enter to return to the menu")
                    Console.WriteLine()
                ElseIf User_answer = "N" Then
                    Console.WriteLine()
                    Console.WriteLine("Press enter to return to menu")
                    Console.ReadLine()
                Else
                    Console.WriteLine()
                    Console.WriteLine("Invalid answer taken as no, returning to menu")
                    Console.WriteLine("Press enter to return to menu")
                    Console.ReadLine()
                End If

            Case 9 'loading from a text file
                Clear(Maze, width, height, maze_visited) 'clear the required variables
                Load_Maze(Maze, width, height, maze_visited, Display_Each_Itteration, "") 'Load the maze

            Case 10 'View last maze
                view_last(Maze, width, height, maze_visited)

            Case 11 'Pathfinding sub-menu
                Dim Settings_MenuKey As ConsoleKey = ConsoleKey.K 'Same as "Case 2" and the gload menu system its running in
                Dim Settings_MenuSelection As Integer = 1

                Dim Solve_Last As Integer = 1
                Console.CursorVisible = False
                Menu.Menu_Solve_Recursive()

                Dim Done As Boolean = False

                Do
                    Console.Title = "Pathfinding Algorithms"

                    Settings_MenuKey = ConsoleKey.K
                    Menu.Menu_Solve_Recursive()

                    Do Until Settings_MenuKey = ConsoleKey.Enter
                        Settings_MenuKey = Console.ReadKey().Key

                        Select Case Settings_MenuKey
                            Case ConsoleKey.DownArrow
                                If Settings_MenuSelection = 3 Then
                                    Settings_MenuSelection = 1
                                Else
                                    Settings_MenuSelection += 1
                                End If

                            Case ConsoleKey.UpArrow
                                If Settings_MenuSelection = 1 Then
                                    Settings_MenuSelection = 3
                                Else
                                    Settings_MenuSelection -= 1
                                End If
                        End Select

                        Select Case Settings_MenuSelection
                            Case 1
                                Console.SetCursorPosition(0, 0)
                                Console.WriteLine(">Solve using Breadth First Searching<")
                            Case 2
                                Console.SetCursorPosition(0, 1)
                                Console.WriteLine(">Solve using A* Pathfinding<")
                            Case 3
                                Console.SetCursorPosition(0, 3)
                                Console.WriteLine(">Back<")
                        End Select

                        If Solve_Last <> -1 Then
                            Select Case Solve_Last
                                Case 1
                                    Console.SetCursorPosition(0, 0)
                                    Console.WriteLine(" Solve using Breadth First Searching ")
                                Case 2
                                    Console.SetCursorPosition(0, 1)
                                    Console.WriteLine(" Solve using A* Pathfinding ")
                                Case 3
                                    Console.SetCursorPosition(0, 3)
                                    Console.WriteLine(" Back ")
                            End Select
                        End If

                        Solve_Last = Settings_MenuSelection
                    Loop

                    Select Case Settings_MenuSelection
                        Case 1
                            Solve_Last_Recursive(Maze, width, height, Maze_solved, Display_Each_Itteration)
                        Case 2
                            Solve_Last_AStar(Maze, width, height, Maze_solved, Display_Each_Itteration)
                        Case 3
                            Done = True
                    End Select

                    Settings_MenuSelection = 1

                Loop Until Done = True

                MenuSelection = 1

            Case 12 'Play the current maze
                Play_Last(Maze, width, height)

            Case 13 'Save the maze to a text file
                Save_To_Text_File(Maze, width, height)

            Case 14
                End 'close the program
        End Select

    End Sub

    Public Sub Clear(ByRef Maze(,) As String, ByVal Width As Integer, ByVal Height As Integer, ByRef Maze_Visited(,) As Boolean)

        For i = 0 To Width
            For y = 0 To Height
                Maze_Visited(i, y) = False 'reset the Maze_visited to False
                Maze(i, y) = "|_" 're initialise the maze array to "|_" for every cell
            Next
        Next

    End Sub

#Region "Menu Layouts"

    Private Sub Menu_Settings_Display(ByRef Display_Every_Itteration As Boolean)

        If Display_Every_Itteration = True Then
            Console.Clear()
            Console.WriteLine("Display Settings:")
            Console.WriteLine()
            Console.WriteLine("Display after every itteration >Yes<  No") 'Two options for The Display menu. 
            Console.WriteLine()
            Console.WriteLine(" Back")
        Else
            Console.Clear()
            Console.WriteLine("Display Settings:")
            Console.WriteLine()
            Console.WriteLine("Display after every itteration  Yes  >No<")
            Console.WriteLine()
            Console.WriteLine(" Back")
        End If

    End Sub

    Private Sub Menu_Settings_Back(Display_Each_Itteration)

        Console.Clear()
        Console.WriteLine("Display Settings:")
        Console.WriteLine()
        Console.WriteLine("Display after every itteration  Yes   No") 'Settings Menu
        Console.WriteLine()
        Console.WriteLine(">Back<")

    End Sub

    Private Sub Menu_Solve_Recursive()

        Console.Clear()
        Console.WriteLine(">Solve using Breadth First Searching<")
        Console.WriteLine(" Solve using A* Pathfinding") 'Layout for pathfinding menu
        Console.WriteLine()
        Console.WriteLine(" Back")

    End Sub

    Private Sub Menu_First()
        Console.Clear()
        Console.WriteLine("SYSTEM STUFF")
        Console.WriteLine(">Change the Dimensions of the maze<")
        Console.WriteLine(" Generation Settings")
        Console.WriteLine()
        Console.WriteLine("ALGORITHMS:")
        Console.WriteLine(" Recursive Backtracking")
        Console.WriteLine(" Aldous-Broder")
        Console.WriteLine(" Wilson's")
        Console.WriteLine(" Hunt-and-Kill")
        Console.WriteLine(" Binary Tree") 'Menu Layout
        Console.WriteLine(" Sidewinder")
        Console.WriteLine(" Load a Maze from text document")
        Console.WriteLine()
        Console.WriteLine("POST GENERATION STUFF:")
        Console.WriteLine(" View Last Generated Maze")
        Console.WriteLine(" Solve Last Generated Maze")
        Console.WriteLine(" Play last generated maze")
        Console.WriteLine(" Save last maze to text document")
        Console.WriteLine()
        Console.WriteLine(" Exit")
    End Sub

#End Region

End Class
