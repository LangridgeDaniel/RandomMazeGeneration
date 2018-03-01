Imports System.Numerics
Imports System
Module Module1

    'I have used regions in this code as in VS it allows me to section up my code into groups. For example pre and post algorithms. They don't do anything other than organise code nicely

    'also, I have decided that the entrance and exit to the mazes will be the top left and bottom right respectivally. However as all of the algorithms generate perfect mazes (every cell can reach every other cell via one path)
    'I could make these entrances and exits anywhere. 

    'At the top of every algorithm I have included a link to the webpage I used for researching the algorithm.

    Sub Main()

        Dim height, width As ULong 'ULong allows the user to store extremly large number. However with the buffer limit added, there is no way for them to overflow the variables

        Dim Display_Each_Itteration As Boolean = True 'Used for the menu option, however it must be passed through from here.

        Set_Dimensions(height, width, "User", Display_Each_Itteration, "")

    End Sub

    Sub Set_Dimensions(ByRef height As Integer, ByRef width As Integer, ByVal Which_Sub As String, ByRef Display_Each_Itteration As Boolean, ByVal FileName As String)

        Dim Menu As New Menu

        Dim Buffer As Integer

        If Which_Sub = "User" Then 'checks to see if the user has called for a size dimension change, or if the size needs to be changed to accomodate a loaded maze

            Dim Valid As Boolean

            Do
                Buffer = Console.BufferWidth \ 2
                Try
                    Do
                        Console.Clear()
                        Console.Write("Please enter the Width of the maze (must be greater than 1): ")
                        width = Console.ReadLine

                        If width < Buffer Then 'Checks to see if the inputted value is larger than the window width
                            Valid = True
                        Else
                            Console.WriteLine()
                            Console.WriteLine("Width is bigger than the window size, press enter to try again")
                            Valid = False
                            Console.ReadLine()
                        End If
                    Loop Until width > 1
                Catch
                    Valid = False
                End Try

            Loop Until Valid = True

            Do
                Buffer = Console.BufferHeight
                Try
                    Do
                        Console.Clear()
                        Console.Write("Please enter the Height of the maze (Must be greater than 1): ")
                        height = Console.ReadLine

                        If height < Buffer \ 2 Then 'Checks to see if the inputted value is larger than the window height
                            Valid = True
                        Else
                            Console.WriteLine()
                            Console.WriteLine("Height is bigger than the window size, press enter to try again")
                            Valid = False
                            Console.ReadLine()
                        End If
                    Loop Until height > 1
                Catch
                    Valid = False
                End Try
            Loop Until Valid = True

            width -= 1
            height -= 1 'reduces height and width to account for arrays starting from 0 not 1

            Dim Maze(width, height) As String 'creates the 3 required arrays. Maze will store generated mazes, maze_Visited is used during the generation process
            Dim Maze_Visited(width, height) As Boolean 'and Maze_Solved is used to store the solved maze after pathfinding. 
            Dim Maze_Solved(width, height) As String

            Do
                Menu.Menu(Maze, width, height, Maze_Visited, Display_Each_Itteration, Maze_Solved) 'loops the menu so the program doesn't just end when an algorithm has finished. 
            Loop

        ElseIf Which_Sub = "Load" Then 'maze loading

            Dim Maze(width, height) As String
            Dim Maze_Visited(width, height) As Boolean
            Dim Maze_Solved(width, height) As String

            Load_Maze(Maze, width, height, Maze_Visited, Display_Each_Itteration, FileName)

            Console.WriteLine("New Dimensions are: Height - " & height + 1 & ", Width - " & width + 1)
            Console.WriteLine()
            Console.WriteLine("Press enter to continue")
            Console.ReadLine()

            Do
                Menu.Menu(Maze, width, height, Maze_Visited, Display_Each_Itteration, Maze_Solved)
            Loop

        End If

    End Sub

#Region "Generation Algorithms"

    Sub Recursive_Backtracking(ByVal Maze(,) As String, ByVal width As Integer, ByVal height As Integer, ByVal Maze_Visited(,) As Boolean, ByVal Display_Each_Itteration As Boolean)

        'http://weblog.jamisbuck.org/2010/12/27/maze-generation-recursive-backtracking

        Console.SetCursorPosition(0, 0)
        Console.Title = "Recursive Backtracking Generating..."

        Dim Stack_Move As New Stack
        Dim Stack_X As New Stack 'Stacks are needed to track the path which will be used to backtrack.
        Dim Stack_Y As New Stack

        Dim LastX As Integer = -1
        Dim lastY As Integer = -1 'I set these to -1 to tell the program that this is the first move

        Dim Random As New Random
        Dim Random_Int As Integer
        Dim Current_Cell_X As Integer
        Dim Current_Cell_Y As Integer

        Dim Direction As String

        Dim Right, Left, Up, Down As Boolean 'used to determine what directions the program will take

        Current_Cell_X = Random.Next(0, width + 1) 'picks a random starting position
        Current_Cell_Y = Random.Next(0, height + 1) 'not really necessary, but its just another way of making sure the program won't ever repeat (within reason) a maze

        Display_Maze_First(Maze, width, height, -1, -1, Maze_Visited, "Generate") 'Displays the initial maze. I need to do this so I can use console.setcursorposition(x,y) to 
        'update the display as its more efficient.
        Do

            Direction = ""

            Maze_Visited(Current_Cell_X, Current_Cell_Y) = True 'means if this cell is revisited, it won't change its state.

            Stack_Move.Clear()

            If (Current_Cell_X - 1) < 0 Then
                Left = False
            Else
                If Maze_Visited(Current_Cell_X - 1, Current_Cell_Y) = False Then
                    Left = True                                     'determines what directions the program can travel from the current location.
                    Stack_Move.Push("Left")
                Else
                    Left = False                                    'for example:

                End If
            End If

            If (Current_Cell_X + 1) > width Then
                Right = False
            Else
                If Maze_Visited(Current_Cell_X + 1, Current_Cell_Y) = False Then
                    Right = True                                    'if the program increases the x coord by 1 (or moving right), and tries to move to a cell that has already been visited
                    Stack_Move.Push("Right")
                Else
                    Right = False                           'this stops that from happening by making Right = False. This will be adressed later in the program
                End If
            End If                                                      'This section is the same repeated for each direction

            If (Current_Cell_Y - 1) < 0 Then
                Up = False
            Else
                If Maze_Visited(Current_Cell_X, Current_Cell_Y - 1) = False Then
                    Up = True
                    Stack_Move.Push("Up")
                Else
                    Up = False
                End If
            End If

            If (Current_Cell_Y + 1) > height Then
                Down = False
            Else
                If Maze_Visited(Current_Cell_X, Current_Cell_Y + 1) = False Then
                    Down = True
                    Stack_Move.Push("Down")
                Else
                    Down = False
                End If
            End If

            If Stack_Move.Count = 0 Then 'if this is True, the algorithm is stuck (Up = false, Down = false, Right = false and left = false). so will run the backtracking part of the algorithm

                If Stack_X.Count - 1 = 0 Then

                    Stack_X.Pop()
                    Stack_Y.Pop()

                Else

                    Stack_X.Pop()
                    Stack_Y.Pop()

                    Current_Cell_X = Stack_X.Peek 'Sets the X and Y coords to the stack values. This is the backtracking part
                    Current_Cell_Y = Stack_Y.Peek

                End If

            Else

                Stack_X.Push(Current_Cell_X)
                Stack_Y.Push(Current_Cell_Y)

                Random_Int = Random.Next(0, Stack_Move.Count)

                If Random_Int <> 0 Then

                    For i = 1 To Random_Int
                        Stack_Move.Pop()
                    Next
                End If

                Direction = Stack_Move.Peek

                If Direction = "Up" Then 'Direction is the direction the program will go. 
                    If Maze_Visited(Current_Cell_X, Current_Cell_Y - 1) = True Then
                        Current_Cell_Y -= 1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = "|_" Then
                        Current_Cell_Y -= 1
                        Maze(Current_Cell_X, Current_Cell_Y) = "| "

                    ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = " _" Then
                        Current_Cell_Y -= 1
                        Maze(Current_Cell_X, Current_Cell_Y) = "  "

                    ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = "| " Then
                        Current_Cell_Y -= 1
                    End If

                ElseIf Direction = "Right" Then
                    If Maze_Visited(Current_Cell_X + 1, Current_Cell_Y) = True Then
                        Current_Cell_X += 1
                    ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = "|_" Then
                        Current_Cell_X += 1
                        Maze(Current_Cell_X, Current_Cell_Y) = " _"
                    ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = "| " Then
                        Current_Cell_X += 1
                        Maze(Current_Cell_X, Current_Cell_Y) = "  "
                    ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = " _" Then    'this is used to draw the individual cells. as where each cell draws the top wall for the one below and the right wall for the left cell, it can get kinda complex...
                        Current_Cell_X += 1                                        'hence the elaborate If statement to the left
                    End If

                ElseIf Direction = "Left" Then
                    If Maze_Visited(Current_Cell_X - 1, Current_Cell_Y) = True Then
                        Current_Cell_X = -1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "|_" Then
                        Maze(Current_Cell_X, Current_Cell_Y) = " _"
                        Current_Cell_X -= 1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = " _" Then
                        Current_Cell_X -= 1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "| " Then
                        Maze(Current_Cell_X, Current_Cell_Y) = "  "
                        Current_Cell_X -= 1
                    End If

                ElseIf Direction = "Down" Then
                    If Maze_Visited(Current_Cell_X, Current_Cell_Y + 1) = True Then
                        Current_Cell_Y += 1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "|_" Then
                        Maze(Current_Cell_X, Current_Cell_Y) = "| "
                        Current_Cell_Y += 1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = " _" Then
                        Maze(Current_Cell_X, Current_Cell_Y) = "  "
                        Current_Cell_Y += 1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "| " Then
                        Current_Cell_Y += 1
                    End If
                End If

            End If

            If Display_Each_Itteration = True Then
                Display_Maze(Maze, width, height, Current_Cell_X, Current_Cell_Y, Maze_Visited, "Generate", LastX, lastY) 'Second Display is commented so it can be used as a comparison as to which displaying method is more efficient
                'Display_Maze_First(Maze, width, height, Current_Cell_X, Current_Cell_Y, Maze_Visited, "Generate")
            End If

        Loop Until Stack_X.Count = 0 And Stack_Y.Count = 0

        If Maze(width, height) = "|_" Then
            Maze(width, height) = "| "
        ElseIf Maze(width, height) = " _" Then    'removes the floor for the cell above the exit. I have decided for the exit to be the bottom right across all the mazes. out of pure ease. 
            Maze(width, height) = "  "
        End If

        Display_Maze_First(Maze, width, height, -1, -1, Maze_Visited, "Display")

        Console.Title = "Recursive Backtracking Done!!!"
        Console.WriteLine()

    End Sub

    Sub Aldous_Broder(ByVal Maze(,) As String, ByVal width As Integer, ByVal height As Integer, ByVal Maze_Visited(,) As Boolean, ByVal Display_Each_Itteration As Boolean)

        'http://weblog.jamisbuck.org/2011/1/17/maze-generation-aldous-broder-algorithm

        Console.SetCursorPosition(0, 0)
        Console.Title = "Aldous Broader Generating..."

        'This Algorithm is fundamentally the same as the Recursive_Backtrackerm, however is it horrifically inefficent
        'as there are no limitations on where the current cell can go. meaning old cells can be visited. and you are compleatly at the mercy of RNG
        'for how long this takes... So it could take a matter of seconds, or minuets depending on how unlucky you get. 
        'However as it is very similar to Recurcive.. I have reused a lot of the code. Making life a lot easier for me.

        Dim Stack_Move As New Stack

        Dim LastX As Integer = -1
        Dim LastY As Integer = -1

        Dim Random As New Random
        Dim Random_Int As Integer
        Dim Current_Cell_X As Integer
        Dim Current_Cell_Y As Integer

        Dim Direction As String
        Dim Right, Left, Up, Down As Boolean 'used to determine what directions the 

        Dim Not_Done As Boolean

        Current_Cell_X = Random.Next(0, width + 1) 'picks a random starting position
        Current_Cell_Y = Random.Next(0, height + 1) 'not really necessary, but its just another way of making sure the program won't ever repeat (within reason) a maze

        Display_Maze_First(Maze, width, height, -1, -1, Maze_Visited, "Generate")

        Do

            Direction = ""

            Maze_Visited(Current_Cell_X, Current_Cell_Y) = True 'means if this cell is revisited, it won't change its state.

            Stack_Move.Clear()

            If (Current_Cell_X - 1) < 0 Then
                Left = False
            Else
                Left = True                                     'determines what directions the program can travel from the current location. and adds them to the move stack
                Stack_Move.Push("Left")

            End If

            If (Current_Cell_X + 1) > width Then
                Right = False
            Else
                Right = True
                Stack_Move.Push("Right")
            End If

            If (Current_Cell_Y - 1) < 0 Then
                Up = False
            Else
                Up = True
                Stack_Move.Push("Up")
            End If

            If (Current_Cell_Y + 1) > height Then
                Down = False
            Else
                Down = True
                Stack_Move.Push("Down")
            End If

            Random_Int = Random.Next(0, Stack_Move.Count)

            If Random_Int <> 0 Then

                For i = 1 To Random_Int
                    Stack_Move.Pop() 'pops a random amount off, and the stack value is the direction the program will move
                Next
            End If

            Direction = Stack_Move.Peek

            If Direction = "Up" Then 'Direction is the direction the program will go. 
                If Maze_Visited(Current_Cell_X, Current_Cell_Y - 1) = True Then
                    Current_Cell_Y -= 1
                ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = "|_" Then
                    Current_Cell_Y -= 1
                    Maze(Current_Cell_X, Current_Cell_Y) = "| "
                ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = " _" Then
                    Current_Cell_Y -= 1
                    Maze(Current_Cell_X, Current_Cell_Y) = "  "
                ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = "| " Then
                    Current_Cell_Y -= 1
                End If

            ElseIf Direction = "Right" Then
                If Maze_Visited(Current_Cell_X + 1, Current_Cell_Y) = True Then
                    Current_Cell_X += 1
                ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = "|_" Then
                    Current_Cell_X += 1
                    Maze(Current_Cell_X, Current_Cell_Y) = " _"
                ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = "| " Then
                    Current_Cell_X += 1
                    Maze(Current_Cell_X, Current_Cell_Y) = "  "
                ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = " _" Then    'this is used to draw the individual cells. as where each cell draws the top wall for the one below and the right wall for the left cell, it can get kinda complex...
                    Current_Cell_X += 1                                        'hence the elaborate If statement to the left
                End If

            ElseIf Direction = "Left" Then
                If Maze_Visited(Current_Cell_X - 1, Current_Cell_Y) = True Then
                    Current_Cell_X -= 1
                ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "|_" Then
                    Maze(Current_Cell_X, Current_Cell_Y) = " _"
                    Current_Cell_X -= 1
                ElseIf Maze(Current_Cell_X, Current_Cell_Y) = " _" Then
                    Current_Cell_X -= 1
                ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "| " Then
                    Maze(Current_Cell_X, Current_Cell_Y) = "  "
                    Current_Cell_X -= 1
                End If

            ElseIf Direction = "Down" Then
                If Maze_Visited(Current_Cell_X, Current_Cell_Y + 1) Then
                    Current_Cell_Y += 1
                ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "|_" Then
                    Maze(Current_Cell_X, Current_Cell_Y) = "| "
                    Current_Cell_Y += 1
                ElseIf Maze(Current_Cell_X, Current_Cell_Y) = " _" Then
                    Maze(Current_Cell_X, Current_Cell_Y) = "  "
                    Current_Cell_Y += 1
                ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "| " Then
                    Current_Cell_Y += 1
                End If
            End If

            Not_Done = False

            For i = 0 To width
                For y = 0 To height
                    If Maze_Visited(i, y) = True Then
                    Else
                        Not_Done = True
                        Exit For
                    End If
                Next
                If Not_Done = True Then
                    Exit For
                End If
            Next

            If Display_Each_Itteration = True Then
                Display_Maze(Maze, width, height, Current_Cell_X, Current_Cell_Y, Maze_Visited, "Generate", LastX, LastY)
            End If

        Loop Until Not_Done = False

        If Maze(width, height) = "|_" Then
            Maze(width, height) = "| "
        ElseIf Maze(width, height) = " _" Then    'removes the floor for the cell above the exit. I have decided for the exit to be the bottom right across all the mazes. 
            Maze(width, height) = "  "
        End If

        Display_Maze_First(Maze, width, height, -1, -1, Maze_Visited, "Display")

        Console.Title = "Aldous Border Done!!!"

    End Sub

    Sub Wilsons(ByVal Maze(,) As String, ByVal width As Integer, ByVal height As Integer, ByVal Maze_Visited(,) As Boolean, ByVal Display_Each_Itteration As Boolean)

        'http://weblog.jamisbuck.org/2011/1/20/maze-generation-wilson-s-algorithm

        Console.Title = "Wilsons Generating..."

        Console.Clear()

        Dim Menu As New Menu()

        Dim Done As Boolean
        Dim Final_Done As Boolean 'There are two main loops running in this algorithm, so I am using these boolean variables to check both are done before I finish the algorithm

        Menu.Clear(Maze, width, height, Maze_Visited)

        Dim Remaining As Integer = (width * height) - 1

        Dim random As New Random
        Dim Random_Int As Integer

        Dim LastX As Integer = -1
        Dim LastY As Integer = -1

        Dim Current_X, Current_Y As New Queue
        Dim Direction_Stack, Current_X_Stack, Current_Y_Stack As New Stack

        Dim Direction(width, height) As String 'Used to track what directionn the program traveled from each cell during the generation process

        Dim Current_Cell_X As Integer
        Dim Current_Cell_Y As Integer
        Dim Current_Direction As String

        Dim First_X, First_Y As Integer

        Dim Walking As Boolean = True

        Dim Left As Boolean = False
        Dim Right As Boolean = False
        Dim Up As Boolean = False
        Dim Down As Boolean = False

        Dim Count_X, Count_Y As New Stack

        Maze_Visited(random.Next(0, width + 1), random.Next(0, height + 1)) = True 'Randomizes the first visited cell to be used in he first "walk"

        Display_Maze_First(Maze, width, height, -1, -1, Maze_Visited, "Generate")

        Do

            Count_X.Clear()
            Count_Y.Clear()

            Final_Done = True

            For i = 0 To width
                For y = 0 To height
                    If Maze_Visited(i, y) = False Then 'Adds all unvisited cells to 2 stacks. X and Y
                        Count_X.Push(i)
                        Count_Y.Push(y)
                        Final_Done = False
                    End If
                Next
            Next

            If Final_Done = True Then
                Exit Do
            End If

            If Final_Done = False Then
                Walking = True
            End If

            Do

                Random_Int = random.Next(0, Count_X.Count)

                For i = 1 To Random_Int 'Picks a random cell from the stacks by popping off a random number of values
                    Count_X.Pop()
                    Count_Y.Pop()
                Next

                Current_Cell_X = Count_X.Peek
                Current_Cell_Y = Count_Y.Peek

            Loop Until Maze_Visited(Current_Cell_X, Current_Cell_Y) = False

            First_X = Current_Cell_X
            First_Y = Current_Cell_Y

            Do

                If (Current_Cell_X - 1) < 0 Then
                    Left = False
                Else
                    Left = True
                End If

                If (Current_Cell_X + 1) > width Then 'Checks to see where the program can go by checking if the current cell is on the boundary
                    Right = False
                Else
                    Right = True
                End If

                If (Current_Cell_Y - 1) < 0 Then
                    Up = False
                Else
                    Up = True
                End If

                If (Current_Cell_Y + 1) > height Then
                    Down = False
                Else
                    Down = True
                End If

                Current_X.Enqueue(Current_Cell_X)
                Current_Y.Enqueue(Current_Cell_Y)

                Random_Int = random.Next(1, 5)

                If Random_Int = 1 Then
                    If Up = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Up"
                        Current_Direction = "Up"
                    ElseIf Down = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Down"
                        Current_Direction = "Down"
                    ElseIf Right = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Right"
                        Current_Direction = "Right"
                    ElseIf Left = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Left"
                        Current_Direction = "Left"
                    End If

                ElseIf Random_Int = 2 Then
                    If Down = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Down"
                        Current_Direction = "Down"
                    ElseIf Right = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Right" 'Same code repeated to add exta randomibality (different order in each)
                        Current_Direction = "Right"
                    ElseIf Left = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Left"
                        Current_Direction = "Left"
                    ElseIf Up = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Up"
                        Current_Direction = "Up"
                    End If

                ElseIf Random_Int = 3 Then
                    If Right = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Right"
                        Current_Direction = "Right"
                    ElseIf Left = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Left"
                        Current_Direction = "Left"
                    ElseIf Up = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Up"
                        Current_Direction = "Up"
                    ElseIf Down = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Down"
                        Current_Direction = "Down"
                    End If

                ElseIf Random_Int = 4 Then
                    If Left = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Left"
                        Current_Direction = "Left"
                    ElseIf Up = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Up"
                        Current_Direction = "Up"
                    ElseIf Down = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Down"
                        Current_Direction = "Down"
                    ElseIf Right = True Then
                        Direction(Current_Cell_X, Current_Cell_Y) = "Right"
                        Current_Direction = "Right"
                    End If
                End If

                If Current_Direction = "Up" Then
                    If Maze_Visited(Current_Cell_X, Current_Cell_Y - 1) = True Then
                        Walking = False
                    Else
                        Current_Cell_Y -= 1
                    End If
                ElseIf Current_Direction = "Down" Then
                    If Maze_Visited(Current_Cell_X, Current_Cell_Y + 1) = True Then 'Checks to see if the program has encountered a visited cell. If so, end the walk. else, carry on the walk.
                        Walking = False
                    Else
                        Current_Cell_Y += 1
                    End If
                ElseIf Current_Direction = "Right" Then
                    If Maze_Visited(Current_Cell_X + 1, Current_Cell_Y) = True Then
                        Walking = False
                    Else
                        Current_Cell_X += 1
                    End If
                ElseIf Current_Direction = "Left" Then
                    If Maze_Visited(Current_Cell_X - 1, Current_Cell_Y) = True Then
                        Walking = False
                    Else
                        Current_Cell_X -= 1
                    End If

                Else
                    Walking = True
                    If Current_Direction = "Up" Then
                        Current_Cell_Y -= 1
                    ElseIf Current_Direction = "Down" Then
                        Current_Cell_Y += 1
                    ElseIf Current_Direction = "Right" Then
                        Current_Cell_X += 1
                    ElseIf Current_Direction = "Left" Then
                        Current_Cell_X -= 1
                    End If
                End If

            Loop Until Walking = False 'keep looping till the program is done with its walk

            Current_Cell_X = First_X
            Current_Cell_Y = First_Y

            Do 'This whole loop basically retraces the walk, taking the direction from each cell and traveling that route

                Maze_Visited(Current_Cell_X, Current_Cell_Y) = True

                Current_Direction = Direction(Current_Cell_X, Current_Cell_Y)

                Done = False

                If Current_Direction = "Up" Then
                    If Maze_Visited(Current_Cell_X, Current_Cell_Y - 1) = True Then
                        Done = True
                        If Maze(Current_Cell_X, Current_Cell_Y - 1) = "|_" Then
                            Current_Cell_Y -= 1
                            Maze(Current_Cell_X, Current_Cell_Y) = "| "

                        ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = " _" Then
                            Current_Cell_Y -= 1
                            Maze(Current_Cell_X, Current_Cell_Y) = "  "

                        ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = "| " Then
                            Current_Cell_Y -= 1
                        End If
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = "|_" Then
                        Current_Cell_Y -= 1
                        Maze(Current_Cell_X, Current_Cell_Y) = "| "

                    ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = " _" Then
                        Current_Cell_Y -= 1
                        Maze(Current_Cell_X, Current_Cell_Y) = "  "

                    ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = "| " Then
                        Current_Cell_Y -= 1
                    End If

                ElseIf Current_Direction = "Right" Then
                    If Maze_Visited(Current_Cell_X + 1, Current_Cell_Y) = True Then
                        Done = True
                        If Maze(Current_Cell_X + 1, Current_Cell_Y) = "|_" Then
                            Current_Cell_X += 1
                            Maze(Current_Cell_X, Current_Cell_Y) = " _"
                        ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = "| " Then
                            Current_Cell_X += 1
                            Maze(Current_Cell_X, Current_Cell_Y) = "  "
                        ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = " _" Then
                            Current_Cell_X += 1
                        End If
                    ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = "|_" Then
                        Current_Cell_X += 1
                        Maze(Current_Cell_X, Current_Cell_Y) = " _"
                    ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = "| " Then
                        Current_Cell_X += 1
                        Maze(Current_Cell_X, Current_Cell_Y) = "  "
                    ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = " _" Then
                        Current_Cell_X += 1
                    End If

                ElseIf Current_Direction = "Left" Then
                    If Maze_Visited(Current_Cell_X - 1, Current_Cell_Y) = True Then
                        Done = True
                        If Maze(Current_Cell_X, Current_Cell_Y) = "|_" Then
                            Maze(Current_Cell_X, Current_Cell_Y) = " _"
                            Current_Cell_X -= 1
                        ElseIf Maze(Current_Cell_X, Current_Cell_Y) = " _" Then
                            Current_Cell_X -= 1
                        ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "| " Then
                            Maze(Current_Cell_X, Current_Cell_Y) = "  "
                            Current_Cell_X -= 1
                        End If
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "|_" Then
                        Maze(Current_Cell_X, Current_Cell_Y) = " _"
                        Current_Cell_X -= 1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = " _" Then
                        Current_Cell_X -= 1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "| " Then
                        Maze(Current_Cell_X, Current_Cell_Y) = "  "
                        Current_Cell_X -= 1
                    End If

                ElseIf Current_Direction = "Down" Then
                    If Maze_Visited(Current_Cell_X, Current_Cell_Y + 1) = True Then
                        Done = True
                        If Maze(Current_Cell_X, Current_Cell_Y) = "|_" Then
                            Maze(Current_Cell_X, Current_Cell_Y) = "| "
                            Current_Cell_Y += 1
                        ElseIf Maze(Current_Cell_X, Current_Cell_Y) = " _" Then
                            Maze(Current_Cell_X, Current_Cell_Y) = "  "
                            Current_Cell_Y += 1
                        ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "| " Then
                            Current_Cell_Y += 1
                        End If
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "|_" Then
                        Maze(Current_Cell_X, Current_Cell_Y) = "| "
                        Current_Cell_Y += 1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = " _" Then
                        Maze(Current_Cell_X, Current_Cell_Y) = "  "
                        Current_Cell_Y += 1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "| " Then
                        Current_Cell_Y += 1
                    End If
                End If

                If Display_Each_Itteration = True Then
                    Display_Maze(Maze, width, height, Current_Cell_X, Current_Cell_Y, Maze_Visited, "Generate", LastX, LastY)
                End If

            Loop Until Done = True

            For i = 0 To width
                For y = 0 To height
                    Direction(i, y) = ""
                Next
            Next

        Loop Until Final_Done = True

        If Maze(width, height) = "|_" Then
            Maze(width, height) = "| "
        ElseIf Maze(width, height) = " _" Then
            Maze(width, height) = "  "
        End If

        Console.Title = "Wilsons Done"

        Display_Maze_First(Maze, width, height, -1, -1, Maze_Visited, "Display")

    End Sub

    Sub Hunt_And_Kill(ByVal Maze(,) As String, ByVal width As Integer, ByVal height As Integer, ByVal Maze_Visited(,) As Boolean, ByVal Display_Each_Itteration As Boolean)

        'http://weblog.jamisbuck.org/2011/1/24/maze-generation-hunt-and-kill-algorithm

        Console.SetCursorPosition(0, 0)
        Console.Title = "Hunt and Kill Generating..."

        Dim Stack_Move As New Stack
        Dim found_x As New Queue
        Dim Found_Y As New Queue

        Dim LastX As Integer = -1
        Dim LastY As Integer = -1

        Dim Random As New Random
        Dim Random_Int As Integer
        Dim Current_Cell_X As Integer
        Dim Current_Cell_Y As Integer

        Dim Direction As String

        Dim Right, Left, Up, Down As Boolean 'used to determine what directions the 

        Dim Found_Empty As Boolean

        Current_Cell_X = Random.Next(0, width + 1) 'picks a random starting position
        Current_Cell_Y = Random.Next(0, height + 1) 'not really necessary, but its just another way of making sure the program won't ever repeat (within reason) a maze

        Display_Maze_First(Maze, width, height, -1, -1, Maze_Visited, "Generate")

        Do

            Direction = ""

            Maze_Visited(Current_Cell_X, Current_Cell_Y) = True 'means if this cell is revisited, it won't change its state.

            Stack_Move.Clear()

            If (Current_Cell_X - 1) < 0 Then
                Left = False
            Else
                If Maze_Visited(Current_Cell_X - 1, Current_Cell_Y) = False Then
                    Left = True                                     'determines what directions the program can travel from the current location.
                    Stack_Move.Push("Left")
                Else
                    Left = False                                    'for example:

                End If
            End If

            If (Current_Cell_X + 1) > width Then
                Right = False
            Else
                If Maze_Visited(Current_Cell_X + 1, Current_Cell_Y) = False Then
                    Right = True                                    'if the program increases the x coord by 1 (eqilivent to moving right), and tries to move to a cell that has already been visited
                    Stack_Move.Push("Right")
                Else
                    Right = False                           'this stops that from happening by making Right = False. This is basically error preventition as it prevent the program from overflowing the Maze array
                End If
            End If

            If (Current_Cell_Y - 1) < 0 Then
                Up = False
            Else
                If Maze_Visited(Current_Cell_X, Current_Cell_Y - 1) = False Then
                    Up = True
                    Stack_Move.Push("Up")
                Else
                    Up = False
                End If
            End If

            If (Current_Cell_Y + 1) > height Then
                Down = False
            Else
                If Maze_Visited(Current_Cell_X, Current_Cell_Y + 1) = False Then
                    Down = True
                    Stack_Move.Push("Down")
                Else
                    Down = False
                End If
            End If

            If Stack_Move.Count = 0 Then    'Stuck Bit

                Found_Empty = False

                For y = 0 To height
                    For i = 0 To width

                        If Maze_Visited(i, y) = False Then
                            found_x.Enqueue(i)
                            Found_Y.Enqueue(y)

                            Up = False And Down = False And Right = False And Left = False

                            If i + 1 < width Then
                                If Maze_Visited(i + 1, y) = True Then
                                    Right = True
                                End If
                            End If

                            If i - 1 > -1 Then
                                If Maze_Visited(i - 1, y) = True Then
                                    Left = True
                                End If
                            End If

                            If y + 1 < height Then
                                If Maze_Visited(i, y + 1) = True Then
                                    Down = True
                                End If
                            End If

                            If y - 1 > -1 Then
                                If Maze_Visited(i, y - 1) = True Then
                                    Up = True
                                End If
                            End If

                            If Up = False And Left = False And Right = False And Down = False Then
                                found_x.Dequeue()
                                Found_Y.Dequeue()

                                Found_Empty = False

                            ElseIf Right = True Or Left = True Or Down = True Or Up = True Then
                                Current_Cell_X = found_x.Peek()
                                Current_Cell_Y = Found_Y.Peek()

                                Found_Empty = True

                                If Right = True Then
                                    If Maze(Current_Cell_X + 1, Current_Cell_Y) = "|_" Then
                                        Maze(Current_Cell_X + 1, Current_Cell_Y) = " _"

                                    ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = "| " Then
                                        Maze(Current_Cell_X + 1, Current_Cell_Y) = "  "

                                    ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = " _" Then

                                    End If

                                ElseIf Left = True Then
                                    If Maze(Current_Cell_X, Current_Cell_Y) = "|_" Then
                                        Maze(Current_Cell_X, Current_Cell_Y) = " _"

                                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = " _" Then

                                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "| " Then
                                        Maze(Current_Cell_X, Current_Cell_Y) = "  "

                                    End If

                                ElseIf Down = True Then
                                    If Maze(Current_Cell_X, Current_Cell_Y) = "|_" Then
                                        Maze(Current_Cell_X, Current_Cell_Y) = "| "

                                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = " _" Then
                                        Maze(Current_Cell_X, Current_Cell_Y) = "  "

                                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "| " Then

                                    End If

                                ElseIf Up = True Then
                                    If Maze(Current_Cell_X, Current_Cell_Y - 1) = "|_" Then
                                        Maze(Current_Cell_X, Current_Cell_Y - 1) = "| "

                                    ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = " _" Then
                                        Maze(Current_Cell_X, Current_Cell_Y - 1) = "  "

                                    ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = "| " Then

                                    End If
                                End If

                                Exit For
                            End If
                        End If
                    Next

                    If Right = True Or Left = True Or Down = True Or Up = True Then
                        found_x.Dequeue()
                        Found_Y.Dequeue()
                        Exit For
                    End If
                Next

                If Found_Empty = False Then
                    Exit Do
                End If

            Else

                Random_Int = Random.Next(0, Stack_Move.Count)

                If Random_Int <> 0 Then

                    For i = 1 To Random_Int
                        Stack_Move.Pop()
                    Next
                End If

                Direction = Stack_Move.Peek

                If Direction = "Up" Then 'Direction is the direction the program will go. 
                    If Maze(Current_Cell_X, Current_Cell_Y - 1) = "|_" Then
                        Current_Cell_Y -= 1
                        Maze(Current_Cell_X, Current_Cell_Y) = "| "

                    ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = " _" Then
                        Current_Cell_Y -= 1
                        Maze(Current_Cell_X, Current_Cell_Y) = "  "

                    ElseIf Maze(Current_Cell_X, Current_Cell_Y - 1) = "| " Then
                        Current_Cell_Y -= 1
                    End If

                ElseIf Direction = "Right" Then
                    If Maze(Current_Cell_X + 1, Current_Cell_Y) = "|_" Then
                        Current_Cell_X += 1
                        Maze(Current_Cell_X, Current_Cell_Y) = " _"
                    ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = "| " Then
                        Current_Cell_X += 1
                        Maze(Current_Cell_X, Current_Cell_Y) = "  "
                    ElseIf Maze(Current_Cell_X + 1, Current_Cell_Y) = " _" Then    'this is used to draw the individual cells. as where each cell draws the top wall for the one below and the right wall for the left cell, it can get kinda complex...
                        Current_Cell_X += 1                                        'hence the elaborate If statement to the left
                    End If

                ElseIf Direction = "Left" Then
                    If Maze(Current_Cell_X, Current_Cell_Y) = "|_" Then
                        Maze(Current_Cell_X, Current_Cell_Y) = " _"
                        Current_Cell_X -= 1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = " _" Then
                        Current_Cell_X -= 1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "| " Then
                        Maze(Current_Cell_X, Current_Cell_Y) = "  "
                        Current_Cell_X -= 1
                    End If

                ElseIf Direction = "Down" Then
                    If Maze(Current_Cell_X, Current_Cell_Y) = "|_" Then
                        Maze(Current_Cell_X, Current_Cell_Y) = "| "
                        Current_Cell_Y += 1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = " _" Then
                        Maze(Current_Cell_X, Current_Cell_Y) = "  "
                        Current_Cell_Y += 1
                    ElseIf Maze(Current_Cell_X, Current_Cell_Y) = "| " Then
                        Current_Cell_Y += 1
                    End If
                End If
            End If

            If Display_Each_Itteration = True Then
                Display_Maze(Maze, width, height, Current_Cell_X, Current_Cell_Y, Maze_Visited, "Generate", LastX, LastY)
            End If

        Loop

        If Maze(width, height) = "|_" Then
            Maze(width, height) = "| "
        ElseIf Maze(width, height) = " _" Then    'removes the floor for the cell above the exit. I have decided for the exit to be the bottom right across all the mazes. out of pure ease. 
            Maze(width, height) = "  "
        End If

        Display_Maze_First(Maze, width, height, Current_Cell_X, Current_Cell_Y, Maze_Visited, "Display")

        Console.Title = "Hunt and Kill Done!!!"

    End Sub

    Sub Binary_Tree(ByVal Maze(,) As String, ByVal width As Integer, ByVal height As Integer, ByVal Maze_Visited(,) As Boolean, ByVal Display_Each_Itteration As Boolean)

        'http://weblog.jamisbuck.org/2011/2/1/maze-generation-binary-tree-algorithm

        Console.SetCursorPosition(0, 0)
        Console.Title = "Binary Tree Generating..."

        Dim Count As Double = 0
        Dim Total As Double = width * height

        Dim LastX As Integer = -1
        Dim LastY As Integer = -1

        Dim Random As New Random
        Dim random_Int As Integer

        Display_Maze_First(Maze, width, height, -1, -1, Maze_Visited, "Generate")

        For i = width To 0 Step -1
            For y = height To 0 Step -1 'Steps over every cell in the maze, working from bottom right to top left
                If i - 1 < 0 And y - 1 < 0 Then 'Checks to see if the program is at the final cell
                    Maze_Visited(i, y) = True
                    random_Int = 3 'When Random_Int is 3, nothing will happen
                ElseIf i = 0 Then
                    random_Int = 1 'Program can only carve a path up
                ElseIf y = 0 Then
                    random_Int = 2 'Program can only carve a path up
                Else
                    random_Int = Random.Next(1, 3) 'if Progam can do both direction, randomly pick between 1 (up) and 2 (left)
                End If

                If random_Int = 1 Then 'North
                    If Maze(i, y - 1) = "|_" Then
                        Maze(i, y - 1) = "| "
                    ElseIf Maze(i, y - 1) = " _" Then  'carve a path up
                        Maze(i, y - 1) = "  "
                    ElseIf Maze(i, y - 1) = "| " Then
                    End If

                ElseIf random_Int = 2 Then 'West
                    If Maze(i, y) = "|_" Then
                        Maze(i, y) = " _"
                    ElseIf Maze(i, y) = " _" Then 'carve a path Left
                    ElseIf Maze(i, y) = "| " Then
                        Maze(i, y) = "  "
                    End If
                Else

                End If

                If Display_Each_Itteration = True Then
                    Display_Maze(Maze, width, height, i, y, Maze_Visited, "Generate", LastX, LastY)
                End If

                Maze_Visited(i, y) = True 'set the cell to visited, to be used for displaying the maze

            Next
        Next

        If Maze(width, height) = "|_" Then
            Maze(width, height) = "| "
        ElseIf Maze(width, height) = " _" Then    'removes the floor for the cell above the exit. I have decided for the exit to be the bottom right across all the mazes. out of pure ease. 
            Maze(width, height) = "  "
        End If

        Display_Maze_First(Maze, width, height, 0, 0, Maze_Visited, "Display")

    End Sub

    Sub Sidewinder(ByVal Maze(,) As String, ByVal width As Integer, ByVal height As Integer, ByVal Maze_Visited(,) As Boolean, ByVal Display_Each_Itteration As Boolean)

        'http://weblog.jamisbuck.org/2011/2/3/maze-generation-sidewinder-algorithm.html

        Console.Clear()
        Console.Title = "Sidewinder Generating..."

        Dim Done As Boolean = False

        Dim Run As New Queue 'could have used any of the 3 major structures here, but Queues just proved to be the easiest
        Dim Up_Run As New Queue
        Dim Run_Done As Boolean
        Dim Row_Done As Boolean

        Dim Current_X As Integer = 0
        Dim Current_Y As Integer = 0

        Dim LastX As Integer = -1
        Dim LastY As Integer = -1

        Dim Up, Right, Top_Right As Boolean

        Dim Random As New Random
        Dim Random_Int As Integer

        Current_X = 0

        Run.Clear()

        Display_Maze_First(Maze, width, height, -1, -1, Maze_Visited, "Generate")

        Do

            Up_Run.Clear()
            Run.Clear()

            If Row_Done = True Then
                Current_X = 0
            End If

            Do 'Loop until the current run is finished (program carvs a path up)

                Run_Done = False

                Run.Enqueue(Current_X) 'Add the current cell to the run queue
                Up_Run.Enqueue(Current_X)

                If Current_X + 1 > width Then
                    Right = False
                Else
                    Right = True
                End If

                If Current_Y - 1 < 0 Then 'Decides if the program can go up or right (depending on maze boundaries)
                    Up = False
                Else
                    Up = True
                End If

                If Up = False And Right = False Then 'If these are both false, the program must be in the top right
                    Top_Right = True
                Else
                    Top_Right = False
                End If

                Random_Int = Random.Next(0, 2)

                If Top_Right <> True Then
                    If Random_Int = 0 Then
                        If Right = True Then
                            Current_X += 1 'go right
                        Else
                            Run_Done = True 'end the run as the program has gone up.
                        End If

                    Else
                        If Up = True Then
                            Run_Done = True 'end the run as the program has gone up.
                        Else
                            Current_X += 1 'go right
                        End If

                    End If

                Else
                    Run_Done = True 'if in top right, run is done as on last cell

                End If

            Loop Until Run_Done = True

            If Current_X = width Then
                Row_Done = True 'used to check when to move on to the next row
            Else
                Row_Done = False
                Current_X += 1 'Move right one in preperation for a new run
            End If

            If Run.Count <> 1 Then
                For i = 1 To Run.Count

                    If Run.Peek <> 0 Then 'Makes sure the program doesn't change the x = 0 cell.
                        If i <> 1 Then
                            If Maze(Run.Peek, Current_Y) = "|_" Then
                                Maze(Run.Peek, Current_Y) = " _"

                            ElseIf Maze(Run.Peek, Current_Y) = "| " Then
                                Maze(Run.Peek, Current_Y) = "  "

                            End If
                        End If
                    End If

                    Maze_Visited(Run.Peek, Current_Y) = True 'used for the displaying (not essential to the algorithm)

                    If Display_Each_Itteration = True Then
                        Display_Maze(Maze, width, height, Run.Peek, Current_Y, Maze_Visited, "Generate", LastX, LastY)
                    End If

                    Run.Dequeue() 'stops the same cell from being processed twice

                Next
            End If

            Maze_Visited(Current_X - 1, Current_Y) = True
            Maze_Visited(Current_X, Current_Y) = True

            Random_Int = Random.Next(0, Up_Run.Count)

            For i = 1 To Random_Int
                Up_Run.Dequeue() 'pop off a random number of values from the queue
            Next

            If Current_Y <> 0 Then

                If Maze(Up_Run.Peek, Current_Y - 1) = "|_" Then 'change the cell above the up_run value (random up path)
                    Maze(Up_Run.Peek, Current_Y - 1) = "| "

                ElseIf Maze(Up_Run.Peek, Current_Y - 1) = " _" Then 
                    Maze(Up_Run.Peek, Current_Y - 1) = "  "

                End If

                If Display_Each_Itteration = True Then
                    Display_Maze(Maze, width, height, Up_Run.Peek, Current_Y - 1, Maze_Visited, "Generate", LastX, LastY)
                    Display_Maze(Maze, width, height, Up_Run.Peek, Current_Y, Maze_Visited, "Generate", LastX, LastY)
                End If
            End If

            If Row_Done = True Then
                Current_Y += 1 'move down a row
            End If

            If Current_Y = width + 1 Then
                Done = True 'finish the algorithm
            Else
                Done = False
            End If

        Loop Until Done = True

        If Maze(width, height) = "|_" Then
            Maze(width, height) = "| "
        ElseIf Maze(width, height) = " _" Then    'removes the floor for the cell above the exit. I have decided for the exit to be the bottom right across all the mazes. out of pure ease. 
            Maze(width, height) = "  "
        End If

        Display_Maze_First(Maze, width, height, Current_X, Current_Y, Maze_Visited, "Display")

    End Sub

    Sub Load_Maze(ByRef Maze(,) As String, ByVal width As Integer, ByVal height As Integer, ByRef Maze_visited(,) As Boolean, ByVal Display_Each_Itteration As Boolean, ByVal FileName As String)

        'In all honestly, this was one of the hardest parts of the program to get working. Mainly due to the need to redefine the dimensions for the maze. I had to restructure a lot of the sub's to make it work
        'I would rank this sub higher than A* in difficulty...

        Console.Clear()

        Dim FileHandle As IO.StreamReader 'Open in read format
        Dim MazeRow As String

        Dim Temp_Height As Integer
        Dim Temp_Width As Integer

        If FileName = "" Then
            Console.Write("Please enter the file name (Do not include .Txt): ")
            FileName = Console.ReadLine()
        End If

        Try 'if program crashes, either file doesn't exist, or dimensions currently saved by the maze are incorrect.

            If Lcase(mid(FileName, len(FileName) - 3)) = ".txt" Then 'Checks for a .Txt at the end of the filename
                FileHandle = New IO.StreamReader(FileName) 'user didn't listen to instructions, so don't bother adding .txt
            else
                FileHandle = New IO.StreamReader(FileName & ".Txt") 'adds ".txt" to the file name. 
            End if
                        
            Temp_Height = FileHandle.ReadLine()
            Temp_Width = FileHandle.ReadLine()

            If Temp_Height <> height Or Temp_Width <> width Then
                Dim x As Integer
                x = "Crash" 'Intentianally crash the code to execute the catch statement
            End If

            For row = 0 To width
                MazeRow = FileHandle.ReadLine()
                For column = 0 To height
                    Maze(row, column) = Mid(MazeRow, (column * 2) + 1, 2)
                Next
            Next

            FileHandle.Close()

            Maze_visited(0, 0) = True 'Tells the menu to unlock the post generation options

            Console.WriteLine("Maze Loaded, please press enter")
            Console.ReadLine()

        Catch
            Try 'if this crashes, the file name is wrong. if not, the filename is correct and the dimensions are wrong

            If Lcase(mid(FileName, len(FileName) - 3)) = ".txt" Then 'Checks for a .Txt at the end of the filename
                FileHandle = New IO.StreamReader(FileName) 'user didn't listen to instructions, so don't bother adding .txt. the same as above
            else
                FileHandle = New IO.StreamReader(FileName & ".Txt") 'adds ".txt" to the file name. 
            End if

                Set_Dimensions(FileHandle.ReadLine(), FileHandle.ReadLine(), "Load", Display_Each_Itteration, FileName) 'change the dimensions to that which are identified in the first two lines of the file

                FileHandle.Close()

            Catch
                Console.WriteLine("Text document does not exist")
            End Try

            Console.WriteLine("Press enter the return to the menu")
            Console.ReadLine()

        End Try

    End Sub

#End Region

#Region "Post Generation Stuff"

    Sub view_last(ByVal Maze(,) As String, ByVal Width As Integer, ByVal height As Integer, ByVal Maze_visited(,) As Boolean)

        Console.Clear()

        Dim Current_X As Integer = -1
        Dim Current_Y As Integer = -1

        Display_Maze_First(Maze, Width, height, Current_X, Current_Y, Maze_visited, "Display") 'Displays the current maze. Fairly simple

        Console.WriteLine("DONE, please press enter to return to the Menu...")
        Console.ReadLine()

    End Sub

    Sub Play_Last(ByVal Maze(,) As String, ByVal Width As Integer, ByVal Height As Integer)

        Dim Valid_Answer, Valid_move As Boolean
        Dim finished As Boolean
        Dim up, down, right, left As Boolean

        Dim user_answer As String 'used to store the last cell the user was in, so the program can remove the + character

        Dim LastX As Integer = 0
        Dim LastY As Integer = 0

        Dim Player_Current_X As Integer = 0
        Dim Player_Current_Y As Integer = 0

        Dim MoveKey As ConsoleKey = ConsoleKey.K

        Do

            Console.Clear()

            Console.WriteLine("This is your last generated maze: ")
            Console.WriteLine()

            Console.Write("|S|")
            For i = 0 To Width - 1
                Console.Write("_ ")
            Next

            Console.WriteLine()

            For i = 0 To Height
                For y = 0 To Width
                    Console.Write(Maze(y, i)) 'just displaying the maze
                Next
                Console.Write("|")
                Console.WriteLine()
            Next

            For i = 0 To Width - 1
                Console.Write("  ")
            Next

            Console.WriteLine("|F|")
            Console.WriteLine()

            Console.Write("Are you sure you wish to play this maze? (Y or N) (You can quit at any time by typing 'Quit' or 'Q'): ")
            user_answer = Mid(UCase(Console.ReadLine()), 1, 1)

            Select Case user_answer
                Case "Y"
                    Valid_Answer = True
                Case "N"
                    Valid_Answer = True
                    Exit Sub
                Case Else
                    Valid_Answer = False
                    Console.Clear()
            End Select

        Loop Until Valid_Answer = True

        Console.Clear()
        Console.WriteLine("Move your + character around with the arrow keys")
        Console.WriteLine()
        Console.WriteLine("Press Enter to start")
        Console.ReadLine()

        Play_Display_Maze_First(Maze, Width, Height, Player_Current_X, Player_Current_Y)

        Do
            If Player_Current_X = Width And Player_Current_Y = Height + 1 Then 'if player is 1 below bottom right, maze has been solved
                Console.WriteLine()
                Console.WriteLine("Well done you finished the maze, please press enter to return to the Menu")
                Console.ReadLine()
                Exit Sub
            End If

            If Player_Current_X = Width And Player_Current_Y = Height Then
                down = True 'As the 
            Else
                down = False
            End If

            If Player_Current_X - 1 >= 0 Then 'Left
                If Maze(Player_Current_X, Player_Current_Y) = "|_" Then
                    left = False
                ElseIf Maze(Player_Current_X, Player_Current_Y) = "| " Then 'Checks where the user can go by looking at all surrounding cells. If there is a wall blocking, user can't go that way.
                    left = False
                ElseIf Maze(Player_Current_X, Player_Current_Y) = " _" Then
                    left = True
                ElseIf Maze(Player_Current_X, Player_Current_Y) = "  " Then
                    left = True
                End If
            Else
                left = False
            End If

            If Player_Current_X + 1 <= Width Then 'Right
                If Maze(Player_Current_X + 1, Player_Current_Y) = "|_" Then
                    right = False
                ElseIf Maze(Player_Current_X + 1, Player_Current_Y) = "| " Then 'This is the repeated, only change is direction
                    right = False
                ElseIf Maze(Player_Current_X + 1, Player_Current_Y) = " _" Then
                    right = True
                ElseIf Maze(Player_Current_X + 1, Player_Current_Y) = "  " Then
                    right = True
                End If
            Else
                right = False
            End If

            If Player_Current_Y - 1 >= 0 Then 'Up
                If Maze(Player_Current_X, Player_Current_Y - 1) = "|_" Then
                    up = False
                ElseIf Maze(Player_Current_X, Player_Current_Y - 1) = "| " Then
                    up = True
                ElseIf Maze(Player_Current_X, Player_Current_Y - 1) = " _" Then
                    up = False
                ElseIf Maze(Player_Current_X, Player_Current_Y - 1) = "  " Then
                    up = True
                End If
            Else
                up = False
            End If

            If down = False Then
                If Player_Current_Y + 1 <= Height Then 'Down
                    If Maze(Player_Current_X, Player_Current_Y) = "|_" Then
                        down = False
                    ElseIf Maze(Player_Current_X, Player_Current_Y) = "| " Then
                        down = True
                    ElseIf Maze(Player_Current_X, Player_Current_Y) = " _" Then
                        down = False
                    ElseIf Maze(Player_Current_X, Player_Current_Y) = "  " Then
                        down = True
                    End If
                Else
                    down = False
                End If
            End If

            Do

                Console.SetCursorPosition(0, Height + 4)

                Console.WriteLine("Your current location is: X- " & Player_Current_X + 1 & ", Y- " & Player_Current_Y + 1)

                Console.WriteLine()
                Console.WriteLine("Please press the corresponding arrow key")

                MoveKey = Console.ReadKey().Key 'takes user key input

                Select Case MoveKey
                    Case ConsoleKey.DownArrow
                        If down = True Then
                            Valid_move = True
                            Player_Current_Y += 1
                        Else
                            Valid_move = False
                        End If

                    Case ConsoleKey.UpArrow
                        If up = True Then
                            Valid_move = True
                            Player_Current_Y -= 1
                        Else
                            Valid_move = False
                        End If

                    Case ConsoleKey.RightArrow 'check to see move is valid, if not loop again. 
                        If right = True Then
                            Valid_move = True
                            Player_Current_X += 1
                        Else
                            Valid_move = False
                        End If

                    Case ConsoleKey.LeftArrow
                        If left = True Then
                            Valid_move = True
                            Player_Current_X -= 1
                        Else
                            Valid_move = False
                        End If

                    Case ConsoleKey.Q
                        Console.WriteLine()
                        Console.WriteLine("Are you sure you with to exit the maze? (Y or N)")
                        user_answer = Mid(UCase(Console.ReadLine()), 1, 1)
                        If user_answer = "Y" Then
                            Console.WriteLine("Please press enter to exit to the Menu")
                            Console.ReadLine()
                            Exit Sub
                        Else
                            Console.WriteLine("Please press enter to return to the Maze")
                            Console.ReadLine()
                        End If
                    Case Else
                        Console.WriteLine("Invalid input, please press enter to try again")
                        Valid_move = False
                        Console.ReadLine()

                End Select

            Loop Until Valid_move = True

            If Player_Current_Y <> Height + 1 Then
                Play_Display_Maze(Maze, Width, Height, Player_Current_X, Player_Current_Y, LastX, LastY)
            End If

        Loop Until finished = True

    End Sub

    Sub Display_Maze_First(ByVal maze(,) As String, ByVal width As Integer, ByVal height As Integer, ByVal Current_X As Integer, ByVal Current_Y As Integer, ByVal Maze_visited(,) As Boolean, ByVal Which As String)

        Console.Clear()
        Console.Write("|S|") 'writes the start

        For i = 0 To width - 1
            Console.Write("_ ") 'followed by the top row
        Next

        Console.WriteLine()

        For i = 0 To height 'double for to cover every cell in the maze
            For y = 0 To width
                Select Case Which
                    Case "Generate"

                        If y = Current_X And i = Current_Y Then
                            Console.BackgroundColor = ConsoleColor.Green
                        ElseIf Maze_visited(y, i) = True Then
                            Console.BackgroundColor = ConsoleColor.DarkMagenta 'if generate, colours are included. green for current cell, purple for visited cells and black for everything else
                        Else
                            Console.BackgroundColor = ConsoleColor.Black
                        End If

                    Case "Display"
                        Console.BackgroundColor = ConsoleColor.Black 'if just display, all black

                End Select
                Console.Write(maze(y, i))
            Next
            Console.Write("|") 'Add the right hand wall and move down a line
            Console.WriteLine()

        Next
        
        Console.BackgroundColor = ConsoleColor.Black

        For i = 0 To width - 1
            Console.Write("  ") 'add the spaces before the finish
        Next

        Console.WriteLine("|F|") 'add the finish
        Console.WriteLine()

        System.Threading.Thread.Sleep(60) 'pause to prevent flickering

    End Sub

    Sub Display_Maze(ByVal maze(,) As String, ByVal width As Integer, ByVal height As Integer, ByVal Current_X As Integer, ByVal Current_Y As Integer, ByVal Maze_visited(,) As Boolean, ByVal Which As String, ByRef LastX As Integer, ByRef LastY As Integer)

        Console.SetCursorPosition(Current_X * 2, Current_Y + 1)

        Select Case Which
            Case "Generate"
                Console.BackgroundColor = ConsoleColor.Green
            Case "A*"
                Console.BackgroundColor = ConsoleColor.Red
        End Select

        If maze(Current_X, Current_Y) = "|_" Then
            Console.Write("|_")
        ElseIf maze(Current_X, Current_Y) = " _" Then 'Display Current Cell in green
            Console.Write(" _")
        ElseIf maze(Current_X, Current_Y) = "| " Then
            Console.Write("| ")
        ElseIf maze(Current_X, Current_Y) = "  " Then
            Console.Write("  ")
        End If

        If LastX = -1 And LastY = -1 Then
            LastX = Current_X 
            LastY = Current_Y

        Else
            If Maze_visited(LastX, LastY) = True Then

                Console.BackgroundColor = ConsoleColor.DarkMagenta

                Console.SetCursorPosition(LastX * 2, LastY + 1)

                If maze(LastX, LastY) = "|_" Then
                    Console.Write("|_")
                ElseIf maze(LastX, LastY) = " _" Then 'Display Visited cells in Magenta
                    Console.Write(" _")
                ElseIf maze(LastX, LastY) = "| " Then
                    Console.Write("| ")
                ElseIf maze(LastX, LastY) = "  " Then
                    Console.Write("  ")
                End If
            End If

            LastX = Current_X
            LastY = Current_Y

        End If

        System.Threading.Thread.Sleep(50) 'same pause as before to prevent flickering

        Console.BackgroundColor = ConsoleColor.Black

    End Sub

    Sub Play_Display_Maze_First(ByVal Maze(,) As String, ByVal Width As Integer, ByVal height As Integer, ByVal current_x As Integer, ByVal Current_y As Integer)

        If Maze(current_x, Current_y) = "|_" Then
            Maze(current_x, Current_y) = "|±"
        ElseIf Maze(current_x, Current_y) = " _" Then
            Maze(current_x, Current_y) = " ±"
        ElseIf Maze(current_x, Current_y) = "| " Then
            Maze(current_x, Current_y) = "|+"
        ElseIf Maze(current_x, Current_y) = "  " Then
            Maze(current_x, Current_y) = " +"
        End If

        Console.Clear()
        Console.Write("|S|")

        For i = 0 To Width - 1
            Console.Write("_ ")
        Next

        Console.WriteLine()

        For i = 0 To height
            For y = 0 To Width
                Console.Write(Maze(y, i))
            Next
            Console.Write("|")
            Console.WriteLine()

        Next

        For i = 0 To Width - 1
            Console.Write("  ")
        Next

        Console.WriteLine("|F|")
        Console.WriteLine()

        If Maze(current_x, Current_y) = "|±" Then
            Maze(current_x, Current_y) = "|_"
        ElseIf Maze(current_x, Current_y) = " ±" Then
            Maze(current_x, Current_y) = " _"
        ElseIf Maze(current_x, Current_y) = "|+" Then
            Maze(current_x, Current_y) = "| "
        ElseIf Maze(current_x, Current_y) = " +" Then
            Maze(current_x, Current_y) = "  "
        End If

        Console.WriteLine()

    End Sub

    Sub Play_Display_Maze(ByVal Maze(,) As String, ByVal Width As Integer, ByVal height As Integer, ByVal current_x As Integer, ByVal Current_y As Integer, ByRef LastX As Integer, ByRef LastY As Integer) 'Needs Fixing

        Console.SetCursorPosition(current_x * 2, Current_y + 1)

        If Maze(current_x, Current_y) = "|_" Then
            Console.Write("|±")
        ElseIf Maze(current_x, Current_y) = " _" Then
            Console.Write(" ±")
        ElseIf Maze(current_x, Current_y) = "| " Then
            Console.Write("|+")
        ElseIf Maze(current_x, Current_y) = "  " Then
            Console.Write(" +")
        End If

        If LastX <> -1 Then
            Console.SetCursorPosition(LastX * 2, LastY + 1)

            If Maze(LastX, LastY) = "|_" Then
                Console.Write("|_")
            ElseIf Maze(LastX, LastY) = " _" Then
                Console.Write(" _")
            ElseIf Maze(LastX, LastY) = "| " Then
                Console.Write("| ")
            ElseIf Maze(LastX, LastY) = "  " Then
                Console.Write("  ")
            End If
        End If

        LastX = current_x
        LastY = Current_y

        Console.SetCursorPosition(0, height + 4)

    End Sub

#Region "Pathfinding"

    Sub Solve_Last_Recursive(ByVal maze(,) As String, ByVal width As Integer, ByVal height As Integer, ByRef Maze_solved(,) As String, ByVal DisplayEach As Boolean)

        Console.Title = "Breadth First Search"

        For i = 0 To height
            For y = 0 To width
                Maze_solved(y, i) = maze(y, i)
            Next
        Next

        Dim Valid_answer As Boolean

        Dim Maze_Visited(width, height) As Boolean

        For i = 0 To width
            For y = 0 To height
                Maze_Visited(i, y) = False
            Next
        Next

        Dim Up, Down, Right, Left, stuck As Boolean

        Dim Stack_X As New Stack
        Dim Stack_Y As New Stack
        Dim Temp As New Stack
        Dim Stack_Move As New Stack
        Dim Current_Cell_X As Integer = 0
        Dim Current_Cell_Y As Integer = 0

        Dim Random As New Random
        Dim Random_Int As Integer

        Dim LastX As Integer = -1
        Dim LastY As Integer = -1

        Dim Direction As String

        Dim Done As Boolean = False

        Do
            Console.Clear()
            Console.WriteLine("This is your last generated maze:")
            Console.WriteLine()

            Console.Write("|S|")
            For i = 0 To width - 1
                Console.Write("_ ")
            Next
            Console.WriteLine()
            For i = 0 To height
                For y = 0 To width
                    Console.Write(maze(y, i))
                Next
                Console.Write("|")
                Console.WriteLine()
            Next

            For i = 0 To width - 1
                Console.Write("  ")
            Next
            Console.WriteLine("|F|")
            Console.WriteLine()

            Console.WriteLine()
            Console.Write("Are you sure you wish to solve this maze? (Y or N) ")
            Dim user_answer As String = Console.ReadLine()
            user_answer = Mid(UCase(user_answer), 1, 1)

            If user_answer = "Y" Then
                Valid_answer = True
            ElseIf user_answer = "N" Then
                Exit Sub
            Else
                Valid_answer = False
            End If
        Loop Until Valid_answer = True

        Console.Clear()
        Console.Title = "Calculating Route"

        Current_Cell_X = 0
        Current_Cell_Y = 0

        Right = False
        Down = False
        Up = False
        Left = False

        Stack_Y.Push("0")
        Stack_X.Push("0")

        If DisplayEach = True Then
            Display_Maze_First(Maze_solved, width, height, Current_Cell_X, Current_Cell_Y, Maze_Visited, "Generate")
        End If

        Do

            If DisplayEach = True Then
                Display_Maze(Maze_solved, width, height, Current_Cell_X, Current_Cell_Y, Maze_Visited, "Generate", LastX, LastY)
            End If

            Maze_Visited(Current_Cell_X, Current_Cell_Y) = True

            If Current_Cell_X = width And Current_Cell_Y = height Then
                Done = True
            Else

                Right = False
                Down = False
                Up = False
                Left = False

                Stack_Move.Clear()

                If (Current_Cell_X - 1) < 0 Then
                    Left = False
                Else
                    If Maze_Visited(Current_Cell_X - 1, Current_Cell_Y) = False Then
                        If maze(Current_Cell_X, Current_Cell_Y) = " _" Or maze(Current_Cell_X, Current_Cell_Y) = "  " Then
                            Left = True
                            Stack_Move.Push("Left")
                        Else
                            Left = False
                        End If
                    Else
                        Left = False
                    End If
                End If

                If (Current_Cell_X + 1) > width Then
                    Right = False
                Else
                    If Maze_Visited(Current_Cell_X + 1, Current_Cell_Y) = False Then
                        If maze(Current_Cell_X + 1, Current_Cell_Y) = " _" Or maze(Current_Cell_X + 1, Current_Cell_Y) = "  " Then
                            Right = True
                            Stack_Move.Push("Right")
                        Else
                            Right = False
                        End If
                    Else
                        Right = False
                    End If
                End If

                If (Current_Cell_Y - 1) < 0 Then
                    Up = False
                Else
                    If Maze_Visited(Current_Cell_X, Current_Cell_Y - 1) = False Then
                        If maze(Current_Cell_X, Current_Cell_Y - 1) = "| " Or maze(Current_Cell_X, Current_Cell_Y - 1) = "  " Then
                            Up = True
                            Stack_Move.Push("Up")
                        Else
                            Up = False
                        End If
                    Else
                        Up = False
                    End If
                End If

                If (Current_Cell_Y + 1) > height Then
                    Down = False
                Else
                    If Maze_Visited(Current_Cell_X, Current_Cell_Y + 1) = False Then
                        If maze(Current_Cell_X, Current_Cell_Y) = "| " Or maze(Current_Cell_X, Current_Cell_Y) = "  " Then
                            Down = True
                            Stack_Move.Push("Down")
                        Else
                            Down = False
                        End If
                    Else
                        Down = False
                    End If
                End If


                If Up = False And Right = False And Down = False And Left = False Then
                    stuck = True
                Else
                    stuck = False
                End If

                If stuck = False Then

                    Random_Int = Random.Next(0, Stack_Move.Count)

                    If Random_Int <> 0 Then

                        For i = 1 To Random_Int
                            Stack_Move.Pop()
                        Next
                    End If

                    Direction = Stack_Move.Peek

                    If Direction = "Up" Then 'Direction is the direction the program will go. 
                        Current_Cell_Y -= 1

                    ElseIf Direction = "Right" Then
                        Current_Cell_X += 1

                    ElseIf Direction = "Left" Then
                        Current_Cell_X -= 1

                    ElseIf Direction = "Down" Then
                        Current_Cell_Y += 1

                    End If

                    Stack_X.Push(Current_Cell_X)
                    Stack_Y.Push(Current_Cell_Y)

                Else

                    If Stack_X.Count = 1 Then

                        Stack_X.Pop()
                        Stack_Y.Pop()

                    ElseIf Stack_X.Count = 0 Then

                        Current_Cell_Y = 0
                        Current_Cell_X = 0

                    Else

                        Stack_X.Pop()
                        Stack_Y.Pop()

                        Current_Cell_X = Stack_X.Peek
                        Current_Cell_Y = Stack_Y.Peek

                    End If
                End If
            End If

        Loop Until Done = True

        For i = 0 To width
            For y = 0 To height
                Maze_solved(i, y) = maze(i, y)
            Next
        Next

        For i = 0 To Stack_X.Count - 1

            If DisplayEach = True Then
                Display_Maze(Maze_solved, width, height, Stack_X.Peek, Stack_Y.Peek, Maze_Visited, "A*", 0, 0)
            End If

            If Maze_solved(Stack_X.Peek, Stack_Y.Peek) = "|_" Then
                Maze_solved(Stack_X.Peek, Stack_Y.Peek) = "|±"

            ElseIf Maze_solved(Stack_X.Peek, Stack_Y.Peek) = "| " Then
                Maze_solved(Stack_X.Peek, Stack_Y.Peek) = "|+"

            ElseIf Maze_solved(Stack_X.Peek, Stack_Y.Peek) = " _" Then
                Maze_solved(Stack_X.Peek, Stack_Y.Peek) = " ±"

            ElseIf Maze_solved(Stack_X.Peek, Stack_Y.Peek) = "  " Then
                Maze_solved(Stack_X.Peek, Stack_Y.Peek) = " +"

            End If

            Stack_X.Pop()
            Stack_Y.Pop()

        Next

        If Maze_solved(width, height) = "|_" Then
            Maze_solved(width, height) = "|±"

        ElseIf Maze_solved(width, height) = "| " Then
            Maze_solved(width, height) = "|+"

        ElseIf Maze_solved(width, height) = " _" Then
            Maze_solved(width, height) = " ±"

        ElseIf Maze_solved(width, height) = "  " Then
            Maze_solved(width, height) = " +"

        End If

        Display_Maze_First(Maze_solved, width, height, -1, -1, Maze_Visited, "Display")

        Console.Title = "Route Found"

        Console.WriteLine()
        Console.WriteLine("Press enter to return to the menu")
        Console.ReadLine()

    End Sub

    Sub Solve_Last_AStar(Maze(,) As String, width As Integer, height As Integer, Maze_Solved(,) As String, ByVal DisplayEach As Boolean)

        Console.Title = "A* Search"

        For i = 0 To height
            For y = 0 To width
                Maze_Solved(y, i) = Maze(y, i)
            Next
        Next

        Dim FScore(width, height) As Integer
        Dim FTemp As Integer
        Dim MovementScore(width, height) As Integer
        Dim DistanceScore(width, height) As Integer

        Dim ParentCell(width, height) As String

        Dim LastX As Integer = -1
        Dim LastY As Integer = -1

        Dim Valid_Answer As Boolean

        Dim Openlist(width, height) As Boolean
        Dim ClosedList(width, height) As Boolean

        Dim Random As New Random
        Dim RandomInt As Integer

        Dim Neighbours As New Queue
        Dim Change As String
        Dim NeighbourTempX, NeighbourTempY As String

        For i = 0 To width
            For y = 0 To height
                ClosedList(i, y) = False
                Openlist(i, y) = False
            Next
        Next

        Dim CurrentX, CurrentY As Integer

        Dim Done As Boolean

        Dim Space As Integer

        CurrentX = 0
        CurrentY = 0

        NeighbourTempX = ""
        NeighbourTempY = ""

        FScore(0, 0) = width + height - 1
        MovementScore(0, 0) = 0

        ClosedList(0, 0) = True

        Do
            Console.Clear()
            Console.WriteLine("This is your last generated maze:")
            Console.WriteLine()

            Console.Write("|S|")
            For i = 0 To width - 1
                Console.Write("_ ")
            Next
            Console.WriteLine()
            For i = 0 To height
                For y = 0 To width
                    Console.Write(Maze(y, i))
                Next
                Console.Write("|")
                Console.WriteLine()
            Next

            For i = 0 To width - 1
                Console.Write("  ")
            Next
            Console.WriteLine("|F|")
            Console.WriteLine()

            Console.WriteLine()
            Console.Write("Are you sure you wish to solve this maze? (Y or N) ")
            Dim user_answer As String = Console.ReadLine()
            user_answer = Mid(UCase(user_answer), 1, 1)

            If user_answer = "Y" Then
                Valid_Answer = True
            ElseIf user_answer = "N" Then
                Exit Sub
            Else
                Valid_Answer = False
            End If
        Loop Until Valid_Answer = True

        For i = 0 To width
            For y = 0 To height
                DistanceScore(i, y) = (width - CurrentX) + (height - CurrentY)
            Next
        Next

        If DisplayEach = True Then
            Display_Maze_First(Maze, width, height, -1, -1, ClosedList, "Generate")
        End If

        Do
            Console.Title = "Generating Route"

            Neighbours.Clear()

            If (CurrentX - 1) < 0 Then
            Else
                If Maze(CurrentX, CurrentY) = " _" Or Maze(CurrentX, CurrentY) = "  " Then
                    If ClosedList(CurrentX - 1, CurrentY) = False Then
                        Openlist(CurrentX - 1, CurrentY) = True
                        Neighbours.Enqueue(CurrentX - 1 & " " & CurrentY)
                    End If
                End If
            End If

            If (CurrentX + 1) > width Then
            Else
                If Maze(CurrentX + 1, CurrentY) = " _" Or Maze(CurrentX + 1, CurrentY) = "  " Then
                    If ClosedList(CurrentX + 1, CurrentY) = False Then
                        Openlist(CurrentX + 1, CurrentY) = True
                        Neighbours.Enqueue(CurrentX + 1 & " " & CurrentY)
                    End If
                End If
            End If

            If (CurrentY - 1) < 0 Then
            Else
                If Maze(CurrentX, CurrentY - 1) = "| " Or Maze(CurrentX, CurrentY - 1) = "  " Then
                    If ClosedList(CurrentX, CurrentY - 1) = False Then
                        Openlist(CurrentX, CurrentY - 1) = True
                        Neighbours.Enqueue(CurrentX & " " & CurrentY - 1)
                    End If
                End If
            End If

            If (CurrentY + 1) > height Then
            Else
                If Maze(CurrentX, CurrentY) = "| " Or Maze(CurrentX, CurrentY) = "  " Then
                    If ClosedList(CurrentX, CurrentY + 1) = False Then
                        Openlist(CurrentX, CurrentY + 1) = True
                        Neighbours.Enqueue(CurrentX & " " & CurrentY + 1)
                    End If
                End If
            End If

            For Each element As String In Neighbours

                For i = 1 To Len(element)
                    If Mid(element, i, 1) = " " Then
                        Space = i
                    End If
                Next

                For i = 1 To Space
                    NeighbourTempX = NeighbourTempX + Mid(element, i, 1)
                Next

                For i = Space + 1 To Len(element)
                    NeighbourTempY = NeighbourTempY + Mid(element, i, 1)
                Next

                MovementScore(NeighbourTempX, NeighbourTempY) = MovementScore(CurrentX, CurrentY) + 1

                FScore(NeighbourTempX, NeighbourTempY) = (((width - NeighbourTempX + height - NeighbourTempY)) + (MovementScore(NeighbourTempX, NeighbourTempY)))

                NeighbourTempX = ""
                NeighbourTempY = ""

            Next

            If DisplayEach = True Then
                Console.WriteLine()
            End If

            FTemp = -1

            For i = 0 To width
                For y = 0 To height
                    If Openlist(i, y) = True Then
                        If FTemp = -1 Then
                            FTemp = FScore(i, y)
                            Change = i & " " & y
                        Else
                            If FScore(i, y) <= FTemp Then
                                If FScore(i, y) = FTemp Then
                                    RandomInt = Random.Next(0, 2)
                                    If RandomInt <> 0 Then
                                        Change = i & " " & y
                                    End If
                                Else
                                    Change = i & " " & y
                                    FTemp = FScore(i, y)
                                End If
                            End If
                        End If
                    End If
                Next
            Next

            NeighbourTempX = ""
            NeighbourTempY = ""

            For i = 1 To Len(Change)
                If Mid(Change, i, 1) = " " Then
                    Space = i
                End If
            Next

            For i = 1 To Space
                NeighbourTempX = NeighbourTempX + Mid(Change, i, 1)
            Next

            For i = Space + 1 To Len(Change)
                NeighbourTempY = NeighbourTempY + Mid(Change, i, 1)
            Next

            CurrentX = NeighbourTempX
            CurrentY = NeighbourTempY

            NeighbourTempY = ""
            NeighbourTempX = ""

            If DisplayEach = True Then
                Display_Maze(Maze, width, height, CurrentX, CurrentY, ClosedList, "Generate", LastX, LastY)
            End If

            Dim ParentUp, ParentDown, ParentRight, ParentLeft As Boolean

            If CurrentX = 0 And CurrentY = 0 Then
                ParentCell(0, 0) = "0 0"

            Else
                ParentUp = False
                ParentDown = False
                ParentRight = False
                ParentLeft = False

                If (CurrentX + 1) > width Then
                    ParentRight = False
                Else
                    If ClosedList(CurrentX + 1, CurrentY) = True Then
                        If Maze(CurrentX + 1, CurrentY) = " _" Or Maze(CurrentX + 1, CurrentY) = "  " Then
                            ParentRight = True
                        End If
                    End If
                End If

                If (CurrentX - 1) < 0 Then
                    ParentLeft = False
                Else
                    If ClosedList(CurrentX - 1, CurrentY) = True Then
                        If Maze(CurrentX, CurrentY) = " _" Or Maze(CurrentX, CurrentY) = "  " Then
                            ParentLeft = True
                        End If
                    End If
                End If

                If (CurrentY - 1) < 0 Then
                    ParentUp = False
                Else
                    If ClosedList(CurrentX, CurrentY - 1) = True Then
                        If Maze(CurrentX, CurrentY - 1) = "| " Or Maze(CurrentX, CurrentY - 1) = "  " Then
                            ParentUp = True
                        End If
                    End If
                End If

                If (CurrentY + 1) > height Then
                    ParentDown = False
                Else
                    If ClosedList(CurrentX, CurrentY + 1) = True Then
                        If Maze(CurrentX, CurrentY) = "| " Or Maze(CurrentX, CurrentY) = "  " Then
                            ParentDown = True
                        End If
                    End If
                End If

                If ParentRight = True Then
                    ParentCell(CurrentX, CurrentY) = CurrentX + 1 & " " & CurrentY
                ElseIf ParentLeft = True Then
                    ParentCell(CurrentX, CurrentY) = CurrentX - 1 & " " & CurrentY
                ElseIf ParentDown = True Then
                    ParentCell(CurrentX, CurrentY) = CurrentX & " " & CurrentY + 1
                ElseIf ParentUp = True Then
                    ParentCell(CurrentX, CurrentY) = CurrentX & " " & CurrentY - 1
                End If

            End If

            Openlist(CurrentX, CurrentY) = False
            ClosedList(CurrentX, CurrentY) = True

            If CurrentX = width And CurrentY = height Then
                Done = True
            Else
                Done = False
            End If

        Loop Until Done = True

        CurrentX = width
        CurrentY = height

        Dim up, left, down, right As Boolean

        For i = 0 To width
            For y = 0 To height
                ClosedList(i, y) = False
            Next
        Next

        Do
            NeighbourTempX = ""
            NeighbourTempY = ""

            up = False
            down = False
            right = False
            left = False

            For i = 1 To Len(ParentCell(CurrentX, CurrentY))
                If Mid(ParentCell(CurrentX, CurrentY), i, 1) = " " Then
                    Space = i
                End If
            Next

            For i = 1 To Space - 1
                NeighbourTempX = NeighbourTempX + Mid(ParentCell(CurrentX, CurrentY), i, 1)
            Next

            For i = Space + 1 To Len(ParentCell(CurrentX, CurrentY))
                NeighbourTempY = NeighbourTempY + Mid(ParentCell(CurrentX, CurrentY), i, 1)
            Next

            If NeighbourTempY < CurrentY Then
                up = True
            ElseIf NeighbourTempY > CurrentY Then
                down = True
            ElseIf NeighbourTempX < CurrentX Then
                left = True
            ElseIf NeighbourTempX > CurrentX Then
                right = True
            End If

            If Maze(CurrentX, CurrentY) = "|_" Then
                Maze_Solved(CurrentX, CurrentY) = "|±"
            ElseIf Maze(CurrentX, CurrentY) = "| " Then
                Maze_Solved(CurrentX, CurrentY) = "|+"
            ElseIf Maze(CurrentX, CurrentY) = " _" Then
                Maze_Solved(CurrentX, CurrentY) = " ±"
            ElseIf Maze(CurrentX, CurrentY) = "  " Then
                Maze_Solved(CurrentX, CurrentY) = " +"
            End If

            If up = True Then
                CurrentX = NeighbourTempX
                CurrentY = NeighbourTempY

            ElseIf down = True Then
                CurrentX = NeighbourTempX
                CurrentY = NeighbourTempY

            ElseIf right = True Then
                CurrentX = NeighbourTempX
                CurrentY = NeighbourTempY

            ElseIf left = True Then
                CurrentX = NeighbourTempX
                CurrentY = NeighbourTempY

            End If

            ClosedList(CurrentX, CurrentY) = True

            If DisplayEach = True Then
                Display_Maze(Maze_Solved, width, height, CurrentX, CurrentY, ClosedList, "A*", 0, 0)
            End If

        Loop Until CurrentX = 0 And CurrentY = 0

        If Maze(0, 0) = "|_" Then
            Maze_Solved(0, 0) = "|±"
        ElseIf Maze(0, 0) = " _" Then
            Maze_Solved(0, 0) = " ±"
        ElseIf Maze(0, 0) = "| " Then
            Maze_Solved(0, 0) = "|+"
        ElseIf Maze(0, 0) = "  " Then
            Maze_Solved(0, 0) = " +"
        End If

        For i = 0 To width
            For y = 0 To height
                ClosedList(i, y) = False
            Next
        Next

        Display_Maze_First(Maze_Solved, width, height, -1, -1, ClosedList, "Generate")

        Console.Title = "Route Found"

        Console.WriteLine()
        Console.WriteLine("Maze solved, press enter to return to the Menu")
        Console.ReadLine()

    End Sub

#End Region

    Sub Save_To_Text_File(ByVal maze(,) As String, ByVal Width As Integer, ByVal Height As Integer)

        Console.Clear()

        Dim FileHandle As IO.StreamWriter
        Console.Write("Please enter the name you wish for the file to be named (Do not include .txt): ")
        Dim FileName As String = Console.ReadLine()

        FileName = FileName + ".Txt"

        FileHandle = New IO.StreamWriter(FileName)

        FileHandle.WriteLine(Height)
        FileHandle.WriteLine(Width)

        For i = 0 To Width
            For y = 0 To Height
                FileHandle.Write(maze(i, y))
            Next
            FileHandle.WriteLine()
        Next

        FileHandle.Close()

        Console.WriteLine("Maze Saved, please press enter to continue")
        Console.ReadLine()

    End Sub

#End Region

End Module
