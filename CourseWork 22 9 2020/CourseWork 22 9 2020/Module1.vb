
Module Module1
    Dim numOfPlayerInSystem As Integer
    'this variable will be used to store the total number of players that have been stored in the text files to ensure that the system loops the correct number of times in certain sections| e.g. it could store 2 for two players| it will be a global variable since many sections require loops to ensure all the players have been involved in the calculations and will also be used in the search for player sections where the user can see how many players fit the criteria out of total
    Dim playerDetailsArray(20, 3), loggedInPlayer As String
    'this array will store the personal information on each player | 0 = place val, 1 = name, 2 = DOB, 3 = Position for the second num e.g. (0,0) = 1, (0,1) = Charlie Jackson , (0,2) 22/Dec/2002, (0,3) FWD | this needs to be global since multiple subroutines require this data like outputOnePlayer()
    'the loggedInPlayer will allow the user to use their name as their username and ensure they can only access their own data and nobody elses | e.g. Charlie Jackson | This needs to be global because multiple different subroutines (the ones used in the player menu) require it to ensure that the player can only access and update their own data
    Dim PlayerMatchStatisticsArray(20, 10), PlayerTimesArray(20, 2) As Integer
    'the first array will store the all of the statistics for each player with each row (first num) representing a new player | (x,0) = place val, (x,1) = minutes played, (x,2) = Goals scored, (x,3) = Assists made, (x,4) = Passes made, (x,5) = Tackles made, (x,6) = Fouls made, (x,7) = Saves made, (x,8) = Goals conceded, (x,9) = Yellow cards, (x,10) = Red Cards | All values stored will be integers e.g. 3 | This will be global since multiple subroutines such as OutputOnePlayer() and SortMatchStats() will be using this to monitor player performance
    'This array will be used to store the training and academic times spent for each of the players | (x,0) = place val (x,1) = training time, (x,2) = academic time | All values stored will be integers e.g. 3 | This will be global since multiple subroutines such as OutputOnePlayer() and SortMatchTimes() will be using this to monitor whether how the player's are spending their time
    Dim placenumber As Integer
    'this variable will be used when reading and writing to files so that the same amount of players read from a file get written back afterwards to avoid not missing out players | Values which could be stored will be integers e.g. 3 | This is a global variable since there are multiple subroutines which utilise file handling

    Dim objStreamWriter As IO.StreamWriter
    Dim objStreamReader As IO.StreamReader
    'Allows system to both write and read from text files
    Dim writeIt As String
    'Allows system to write whatever is currently in the variable to get written to the text file
    Dim readLine As String
    'allows system to store whatever was read from the file into a variable
    'All of these variables are global because file handling is required in multiple different subroutines

    'both of these variables allows sections to looped over and over until the user doesn't require it anymore | True or False | This needs to be global since nearly all of the subroutines require this so that users can reuse that section which different inputs after they have gone through once
    Dim loopSection As Boolean
    Dim continueLoop As String

    Sub Main()
        'sets screen to right size so that all outputs can be viewed comfortably
        Console.WindowHeight = 50
        Console.WindowWidth = 135
        'NEW FEATURE/SECTION
        'checks if the necessary files (for player details, player match stats and player times) exist if not new files are created
        If My.Computer.FileSystem.FileExists("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Personal Details.txt") Then
        Else
            objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Personal Details.txt")
            objStreamWriter.Close()
        End If

        If My.Computer.FileSystem.FileExists("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Match Stats.txt") Then
        Else
            objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Match Stats.txt")
            objStreamWriter.Close()
        End If

        If My.Computer.FileSystem.FileExists("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Player Times.txt") Then
        Else
            objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Player Times.txt")
            objStreamWriter.Close()
        End If

        Console.ForegroundColor = ConsoleColor.White
        'allows system to loop through all of the player names to see if they match up with the username to provided to allow players to login
        Dim playerLoop As Integer
        'Allows user to be authorised before using program | e.g. Password123 | This will be local since it will make my program more efficient due to it requiring less system memory
        Dim username, password As String
        'Allows system to allow user three attempts at logging in | e.g. 2 | This will be local since it will make my program more efficient due to it requiring less system memory
        Dim loginLoop As Integer
        SetUpArrays()
        'This loop ensures that the user only gets three attempts to enter in a correct username and password
        'NEW FEATURE/SECTION
        For loginLoop = 1 To 3
            'allows user to enter username and password
            Console.WriteLine("Players enter their full names and coaches enter 'Coach' for the username - Enter Exit in username input to close program")
            Console.WriteLine("Enter your username")
            Console.ForegroundColor = ConsoleColor.Cyan
            username = Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White
            'allows user to exit immediately if they do not want to use the system
            Select Case username
                Case "Exit"
                    End
            End Select

            Console.WriteLine("Enter your password")
            Console.ForegroundColor = ConsoleColor.Cyan
            password = Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White

            Select Case username
                Case "Coach"
                    'if a coach tries to login they will prompted with the coach menu if they input the password below
                    If password = "FootballAcademy123" Then
                        CoachMenu()
                    Else
                        Console.WriteLine("Invalid Login")
                        Console.ReadLine()
                        Console.Clear()
                    End If
                Case Else
                    'checks if the username inputs matches anyone of the player's full name
                    For playerLoop = 0 To numOfPlayerInSystem
                        If username = playerDetailsArray(playerLoop, 1) And password = "Player123" Then
                            'if the username and password is right the system can store who has logged in by storing the player's place number/ or the x value in the array (x,y)
                            loggedInPlayer = playerLoop
                            PlayerMenu()
                        End If
                    Next
                    'if the username doesn't match the word "coach"s or a player's name it is considered a invalid login no matter the password input
                    If playerLoop > numOfPlayerInSystem - 1 Then
                        Console.WriteLine("Invalid Login")
                        Console.ReadLine()
                        Console.Clear()
                    End If
            End Select
            'tells user how many attempts they have had so far
            Console.WriteLine("You have had " & loginLoop & " attempts out of 3")
            Console.ReadLine()
            Console.Clear()
        Next
        'informs user if attempts have exceeded the maximum 3
        If loginLoop = 3 Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("You have had too many attempts")
        End If
        Console.ReadLine()
        Console.Clear()
    End Sub

    'Subroutine = Prompts the player with all of the menu options that they have available to them (fulfilling the first objective). The user will be able to enter the corresponding menu option and then the correct subroutine for that option will run.
    Sub PlayerMenu()
        'Allows user to choose a specific menu choice | e.g. value stored = 5 |  This will be local since it will make my program more efficient due to it requiring less system memory
        Dim menuChoice As String
        Console.ForegroundColor = ConsoleColor.White
        'clears screen of previous content
        Console.Clear()
        'welcomes player with menu and allows them to input a choice
        Console.WriteLine("Welcome " & playerDetailsArray(loggedInPlayer, 1))
        Console.ForegroundColor = ConsoleColor.DarkRed
        Console.WriteLine("Enter 1 to update your training time")
        Console.WriteLine("Enter 2 to see your stats")
        Console.WriteLine("Enter 3 to logout")
        Console.WriteLine("Enter 4 to close program completely")
        Console.ForegroundColor = ConsoleColor.Cyan
        menuChoice = Console.ReadLine()

        Console.Clear()
        Console.ForegroundColor = ConsoleColor.Red
        'tells user what they have chosen then starts the subroutine they will be using
        Select Case menuChoice
            Case 1
                SelfTimeUpdate()
            Case 2
                Console.WriteLine("You have chosen to view your own stats")
                Console.ForegroundColor = ConsoleColor.White
                ViewOwnStats()
            Case 3
                'NEW FEATURE/SECTION
                Console.WriteLine("You have chosen to logout. Goodbye")
                Console.ReadLine()
                Console.Clear()
                Main()
            Case 4
                End
            Case Else
                Console.WriteLine("Invalid Input")
                Console.ReadLine()
                PlayerMenu()
        End Select
        Console.ReadLine()
    End Sub

    'Subroutine = This prompts the coach with all of the menu options that are available to them (fulfils the first objective) and once an input is given the corresponding subroutine is run.
    Sub CoachMenu()
        SetUpArrays()
        'Allows user to choose a specific menu choice | e.g. value stored = 5 |  This will be local since it will make my program more efficient due to it requiring less system memory
        Dim menuChoice As String
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        Console.WriteLine("Welcome Coach")
        Console.ForegroundColor = ConsoleColor.DarkRed
        'shows the coach what oprions are available and allows them to input a choice
        Console.WriteLine("Enter 1 To Enter Personal Details For a New Player")
        Console.WriteLine("Enter 2 To Add Onto a Player's Match Statistics")
        Console.WriteLine("Enter 3 To Add Onto a Player's Training/ Academic Times")
        Console.WriteLine("Enter 4 To Create a Sorted List of Player's For a Chosen Match Stat")
        Console.WriteLine("Enter 5 To Create a Sorted List of Player's For a Chosen Time")
        Console.WriteLine("Enter 6 To Output All the Data on One Player")
        Console.WriteLine("Enter 7 To Search For a Player with Specific Match Stats")
        Console.WriteLine("Enter 8 To Search For a Player with Specfic Training/ Academic Times")
        Console.WriteLine("Enter 9 To Suggest a Random Player Who is Doing Well")
        Console.WriteLine("Enter 10 To Suggest the Best Role for a Player")
        Console.WriteLine("Enter 11 to Backup Current Text Files ")
        Console.WriteLine("Enter 12 To logout")
        Console.WriteLine("Enter 13 To Close Program Completely")

        Console.ForegroundColor = ConsoleColor.Cyan
        menuChoice = Console.ReadLine()
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()

        Console.ForegroundColor = ConsoleColor.Red
        Select Case menuChoice
            'starts right subroutine based on menu option
            Case 1
                EnterPlayerDetails()
            Case 2
                EnterPlayerStats()
            Case 3
                EnterPlayerTimes()
            Case 4
                SortMatchStats()
            Case 5
                SortTimes()
            Case 6
                OutputOnePlayer()
            Case 7
                SearchMatchStats()
            Case 8
                SearchPlayerTimes()
            Case 9
                SuggestGoodPlayer()
            Case 10
                SuggestGoodRole()
            Case 11
                Backup()
            Case 12
                'NEW FEATURE/SECTION
                Console.WriteLine("You have chosen to logout. Goodbye")
                Console.ReadLine()
                Console.Clear()
                Main()
            Case 13
                End
            Case Else
                Console.WriteLine("Invalid Input")
                Console.ReadLine()
                CoachMenu()
        End Select
        Console.ReadLine()
    End Sub

    'Subroutine = This subroutine reads each of the individual text files into their respective arrays (e.g. PlayerDetailArray() reads from Player Details.text file) so that the rest of the system can use the data that has been stored in the text files in all of the subroutines which require player data (e.g. allows coaches to update player stats rather than starting from 0 each time). It does this through I.O StreamReader and reads each individual line of a text file into the corresponding cell of the array. It also checks where the array becomes empty to work out the number of players which are currently store in the system which is used for loops in other parts of the system.
    Sub SetUpArrays()
        'sets up player detail array using personal details text file
        'selects the player details file
        objStreamReader = New IO.StreamReader("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Personal Details.txt")
        readLine = objStreamReader.ReadLine
        'place number will make sure the row is switched after one player has been read into the array
        placenumber = 0

        'keeps reading the file until there is nothing left
        Do Until readLine = Nothing
            For y = 0 To 3
                ' y = 0 - 'sets whatever has been read as the first column for whatever row the loop is on - this is for the place value
                playerDetailsArray(placenumber, y) = readLine
                'moves reader onto the next line
                readLine = objStreamReader.ReadLine
                ' y = 1 - 'this line sets the name of the player in the array
                ' y = 2 - player's date of birth
                ' y = 3   'player's position
            Next
            'allows the next player to read into the next row in the array
            placenumber = placenumber + 1
        Loop
        'closes text file
        objStreamReader.Close()

        'set match statistics array using match stats text file using same method as before
        objStreamReader = New IO.StreamReader("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Match Stats.txt")
        readLine = objStreamReader.ReadLine
        placenumber = 0
        Do Until readLine = Nothing
            For y = 0 To 10
                'sets whatever has been read as the first column for whatever row the loop is on - this is for the place value
                PlayerMatchStatisticsArray(placenumber, y) = readLine
                'moves reader onto the next line
                readLine = objStreamReader.ReadLine
                ' y=1 - sets minutes played
                ' y=2 - sets goals scored
                ' y=3 - assists
                ' y=4 - passes
                ' y=5 - tackles
                ' y=6 - fouls
                ' y=7 - saves
                ' y=8 - goals condceded
                ' y=9 - yellow cards
                ' y=10 - red cards
            Next
            'moves onto next player to read into the next row in the array
            placenumber = placenumber + 1
        Loop
        objStreamReader.Close()

        'setting player times array using player times text file using same method as before
        objStreamReader = New IO.StreamReader("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Player Times.txt")
        readLine = objStreamReader.ReadLine
        placenumber = 0
        Do Until readLine = Nothing
            For y = 0 To 2
                PlayerTimesArray(placenumber, y) = readLine
                readLine = objStreamReader.ReadLine
                ' y = 0 - place number
                ' y = 1 - training time
                ' y = 2 - academic time
            Next
            placenumber = placenumber + 1
        Loop
        objStreamReader.Close()

        'allows system to continue looping until no player can be read in the array to determine how many players are being stored in the system | True Or False
        Dim checkBlank = False
        numOfPlayerInSystem = 0

        'checks how many players have been stored in a system
        While checkBlank = False
            'if no more player names can be read the system must have counted all of the players
            If playerDetailsArray(numOfPlayerInSystem, 1) = "" Then
                checkBlank = True
            Else
                'if player names can still be read then the number of player's in the system must go up
                numOfPlayerInSystem = numOfPlayerInSystem + 1
            End If
        End While
    End Sub

    'Function = Outputs correct error message in a clear red so that the user understands what input is worng. It also allows my code to become shorter and less repetitive.
    'NEW FEATURE/SECTION
    Function ErrorMessage(errorType)
        Console.ForegroundColor = ConsoleColor.Red
        Select Case errorType
            Case "Input Data Type"
                Console.WriteLine("Invalid Data Type / Format")
            Case "Invalid Player Choice"
                Console.WriteLine("Invalid Player Choice")
            Case "Invalid Name"
                Console.WriteLine("Invalid Name")
            Case "Invalid Day"
                Console.WriteLine("Invalid Day")
            Case "Invalid Month"
                Console.WriteLine("Invalid Month")
            Case "Invalid Year"
                Console.WriteLine("Invalid Year")
            Case "Invalid Position"
                Console.WriteLine("Invalid Position")
            Case "Less Than 0"
                Console.WriteLine("Value less than 0")
            Case "Invalid Stat"
                Console.WriteLine("Invalid Stat Chosen")
            Case "Invalid Time"
                Console.WriteLine("Invalid Time Chosen")
            Case "Invalid MaxOrMin"
                Console.WriteLine("Invalid Choice")
            Case "Invalid Choice"
                Console.WriteLine("Invalid Choice")
            Case Else
                Console.WriteLine("Invalid Input")
        End Select
        Console.ReadLine()
        Console.ForegroundColor = ConsoleColor.White
    End Function

    'Subroutine = This allows users to input the details for a new player (to fulfil the 2nd objective) and allows for the input of a name, date of birth and position which will be added to the PlayerDetailArray() so that it can be written into the Player Details.text file. Then the blank values will be added to Match Stat.txt and Player Times.txt so that the player can start off with 0 in their statistics and times.
    Sub EnterPlayerDetails()
        loopSection = True
        'NEW FEATURE/SECTION
        While loopSection = True
            SetUpArrays()
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("You have chosen to enter a Player's Details")
            Console.ForegroundColor = ConsoleColor.White

            'Allows user to enter the name of the player | e.g.  value stored = Charlie Hulme
            'Allows user to store the positon for a player after it has been validated so it can be written| e.g. value stored = FWD
            'Allows user to enter the date of birth for the player | e.g. value stored = 22/12/2002
            Dim nameEntry, playerPosition, DOBentry As String
            'allows system to loop until an valid input is given for the name | True or False
            Dim LengthCheck = False
            'All of these variables are local because it will make my program more efficient due to local variables requiring less system memory

            'takes in player name input
            While LengthCheck = False
                Console.WriteLine("Enter the full name of the player (between 3-25 characters)")
                Console.ForegroundColor = ConsoleColor.Cyan
                nameEntry = Console.ReadLine
                Console.ForegroundColor = ConsoleColor.White
                'Validation - Length Check / Presence Check - Stops user from using names that would take up too much storage or a empty input
                If nameEntry.Length > 2 And nameEntry.Length < 26 Then
                    LengthCheck = True
                Else
                    ErrorMessage("Invalid Name")
                End If
            End While

            'Both variables allow the system to loop until valid data has been input | True or False
            Dim RangeCheck = False, PresenceCheck = False
            'allows system to individually check each number given for the date to ensure that the date is valid | e.g. value stored = 07
            Dim Days, Month, Year As String
            'Allows user to store the date of birth for the player after the data has been validated where the variable can then get written to a file
            Dim playerDOB As Date
            'All of these variables are local because it will make my program more efficient due to local variables requiring less system memory

            While RangeCheck = False
                While PresenceCheck = False
                    'takes in player date of birth
                    Console.WriteLine("Enter the date of birth in the format dd/mm/yyyy")
                    Console.ForegroundColor = ConsoleColor.Cyan
                    DOBentry = Console.ReadLine
                    Console.ForegroundColor = ConsoleColor.White
                    Days = Left(DOBentry, 2)
                    Month = Mid(DOBentry, 4, 2)
                    Year = Right(DOBentry, 4)
                    'Validation - Data Type / Presence Check - Stops users from entering string or empty values for their date
                    If IsNumeric(Days) = True And IsNumeric(Month) And IsNumeric(Year) Then
                        PresenceCheck = True
                    Else
                        ErrorMessage("Input Data Type")
                    End If
                End While
                'Validation - Range Check - Stops users from entering dates that do not exist (32nd of a month)
                If Year < 2020 And Year > 1990 Then
                    If Month < 13 And Month > 0 Then
                        If Days < 32 And Days > 0 Then
                            Console.WriteLine("Valid Date")
                            RangeCheck = True
                        Else
                            ErrorMessage("Invalid Day")
                            PresenceCheck = False
                        End If
                    Else
                        ErrorMessage("Invalid Month")
                        RangeCheck = False
                        PresenceCheck = False
                    End If
                Else
                    ErrorMessage("Invalid Year")
                    RangeCheck = False
                    PresenceCheck = False
                End If
            End While

            playerDOB = Days & "/" & Month & "/" & Year

            'Allows user to choose from a list of different positions to which they can assign to the player | e.g. value stored = 1
            Dim positionEntry As String
            'Allows system to loop until a valid position for a player has been inputted
            Dim PositionCheck = False

            While PositionCheck = False
                'allows you to enter the player's position
                Console.WriteLine("Enter 1 if player is a foward, Enter 2 for midfielder, Enter 3 for defender or 4 for goalkeeper")
                Console.ForegroundColor = ConsoleColor.Cyan
                positionEntry = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White
                'Validation - Data Type / Presence / Range Check - Ensures non existent positions cannot be entered
                Select Case positionEntry
                    Case 1
                        playerPosition = "FWD"
                        PositionCheck = True
                    Case 2
                        playerPosition = "MID"
                        PositionCheck = True
                    Case 3
                        playerPosition = "DEF"
                        PositionCheck = True
                    Case 4
                        playerPosition = "GKP"
                        PositionCheck = True
                    Case Else
                        ErrorMessage("Invalid Position")
                End Select
            End While
            SetUpArrays()

            'adds input values into player details array
            playerDetailsArray(placenumber, 0) = placenumber
            playerDetailsArray(placenumber, 1) = nameEntry
            playerDetailsArray(placenumber, 2) = DOBentry
            playerDetailsArray(placenumber, 3) = playerPosition

            'Allows system to write to the personal details text file
            objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Personal Details.txt")
            For x = 0 To placenumber
                For y = 0 To 3
                    'writes all the contents of the array including the new inputs added
                    writeIt = playerDetailsArray(x, y)
                    objStreamWriter.WriteLine(writeIt)
                Next
            Next
            objStreamWriter.Close()

            'adds blank new player values to match stats text file
            objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Match Stats.txt", True)
            objStreamWriter.WriteLine(placenumber)
            For x = 0 To 9
                objStreamWriter.WriteLine("0")
            Next
            objStreamWriter.Close()

            'adds blank new player values to player times text file
            objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Player Times.txt", True)
            objStreamWriter.WriteLine(placenumber)
            For x = 0 To 1
                objStreamWriter.WriteLine("0")
            Next
            objStreamWriter.Close()

            'informs coach of success
            Console.WriteLine("Player name, DOB and position entered")

            'allows coach to reuse section to add another new player
            Console.WriteLine("Enter 1 To Reuse Section - Any other input will return to menu")
            Console.ForegroundColor = ConsoleColor.Cyan
            continueLoop = Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White

            Select Case continueLoop
                Case 1
                    loopSection = True
                    Console.Clear()
                Case Else
                    loopSection = False
            End Select
        End While
        CoachMenu()
    End Sub

    'Subroutine = This allows users to update the match statistics (to fulfil the 3rd objective) and allows you to input which player is being updated, choose which statistic you wish to update for that player and finally input how much that statistic has gone up by. After the inputs have been validated, the value is added to the correct (which is worked out by the inputs) cell of “Player Match Statistics” array. After that, the array gets written to the “Match Stats” text file to be stored.
    Sub EnterPlayerStats()
        'allows section to looped over and over until the user doesn't require it anymore | True or False
        loopSection = True

        While loopSection = True
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("You have chosen to enter a Player's Match Statistics")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()

            'Allows system to loop until valid data has been input | True or False
            Dim typeCheck = False
            'Allows user to choose from a list of players using a number | e.g. value stored = 3
            'Allows the user to choose which statistic they are updating | e.g. value stored = 2
            'Allows user to enter any value for the statistic they have chosen to increase the player's statistic | e.g. value stored = 11
            Dim chosenPlayer, statSelect, statIncrease As String
            'All of these variables are local because it will make my program more efficient due to local variables requiring less system memory

            While typeCheck = False
                Console.Clear()
                SetUpArrays()

                'shows coach  all available choices and what place number to enter for who
                placenumber = 0
                Do Until playerDetailsArray(placenumber, 1) = ""
                    Console.WriteLine(playerDetailsArray(placenumber, 0))
                    Console.WriteLine(playerDetailsArray(placenumber, 1))
                    Console.WriteLine()
                    placenumber = placenumber + 1
                Loop

                'allows coach to choose what stat to update and for who
                Console.WriteLine("Enter the place value of the player you want")
                Console.ForegroundColor = ConsoleColor.Cyan
                chosenPlayer = Console.ReadLine
                Console.ForegroundColor = ConsoleColor.White
                Console.WriteLine("Enter number value of the stat you want to add onto")
                Console.WriteLine("1 = minutes played, 2 = Goals scored, 3 = Assists made, 4 = Passes made, 5 = Tackles made")
                Console.WriteLine("6 = Fouls made, 7 = Saves made, 8 = Goals conceded , 9 = Yellow cards , 10 = Red Cards")
                Console.ForegroundColor = ConsoleColor.Cyan
                statSelect = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White
                'allows coach to choose how much the player's stat has gone up by
                Console.WriteLine("Enter how much the stat has gone up by")
                Console.ForegroundColor = ConsoleColor.Cyan
                statIncrease = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White

                'Validation - Data Type / Range / Presence Check - Ensures that only positive integers, the correct type of statistic and actual players can be input
                If IsNumeric(chosenPlayer) = True And IsNumeric(statSelect) And IsNumeric(statIncrease) Then
                    If 0 <= chosenPlayer And chosenPlayer <= numOfPlayerInSystem - 1 Then
                        If 0 < statSelect And statSelect < 11 Then
                            If statIncrease > 0 Then
                                typeCheck = True
                            Else
                                ErrorMessage("Less Than 0")
                            End If
                        Else
                            ErrorMessage("Invalid Stat")
                        End If
                    Else
                        ErrorMessage("Invalid Player Choice")
                    End If
                Else
                    ErrorMessage("Input Data Type")
                End If
            End While

            'Calculates the increase in stat by adding it the original stat and sets the new value into the correct position in the array
            PlayerMatchStatisticsArray(chosenPlayer, statSelect) = PlayerMatchStatisticsArray(chosenPlayer, statSelect) + statIncrease

            'writes match stats array back into file with updated values
            objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Match Stats.txt")
            'loops through how many players are currently in the system
            For x = 0 To placenumber - 1
                'loops through all the columns in the array (place number and all the match statistic)
                For y = 0 To 10
                    'writes all of the values stored in the player match statistics array into the file
                    writeIt = PlayerMatchStatisticsArray(x, y)
                    objStreamWriter.WriteLine(writeIt)
                Next
            Next
            objStreamWriter.Close()

            'prompts coach of success
            Console.WriteLine("Stat has been updated for " & playerDetailsArray(chosenPlayer, 1))

            'allows user to repeat with another player
            Console.WriteLine("Enter 1 To Reuse Section - Any other input will return to menu")
            Console.ForegroundColor = ConsoleColor.Cyan
            continueLoop = Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White

            Select Case continueLoop
                Case 1
                    loopSection = True
                    Console.Clear()
                Case Else
                    loopSection = False
            End Select
        End While
        CoachMenu()
    End Sub

    'Subroutine = This allows users to update the training and academic times (to fulfil the 4th  objective) and allows you to input which player is being updated, choose which time (academicor training) you wish to update for that player and finally input how much that time has gone up by. After the inputs have been validated, the value is added to the correct (which is worked out by the inputs) area of “Player Times” array. After that, the array gets written to the “Player Times” text file to be stored.
    Sub EnterPlayerTimes()
        loopSection = True

        While loopSection = True
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("You have chosen to enter a Player's Academic/ Training Times")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            'Allows system to loop until valid data has been input | True or False
            Dim typeCheck = False
            'Allows user to choose from a list of players | e.g. value stored = 0
            'Allows user to choose between training and academic times | e.g. value stored = 2
            'Allows user to input how much their time has increased by | e.g. value stored = 10
            Dim chosenPlayer, timeSelect, timeIncrease As String
            'All of these variables will local since it will make my program more efficient due to it requiring less system memory

            While typeCheck = False
                Console.Clear()
                SetUpArrays()

                'shows coach available choices
                placenumber = 0
                Do Until playerDetailsArray(placenumber, 1) = ""
                    Console.WriteLine(playerDetailsArray(placenumber, 0))
                    Console.WriteLine(playerDetailsArray(placenumber, 1))
                    Console.WriteLine()
                    placenumber = placenumber + 1
                Loop

                'allows coach to choose a player
                Console.WriteLine("Enter the placevalue of the player you are choosing")
                Console.ForegroundColor = ConsoleColor.Cyan
                chosenPlayer = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White
                'allows coach to choose which time they want to increase
                Console.WriteLine("Enter 1 to add onto their training time,Enter 2 to add onto their academic time ")
                Console.ForegroundColor = ConsoleColor.Cyan
                timeSelect = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White
                'allows coach to input how much the time has increased
                Console.WriteLine("Enter the value the time has increased by (in min) ")
                Console.ForegroundColor = ConsoleColor.Cyan
                timeIncrease = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White
                'Validation - Data Type / Range / Presence Check - Ensures that only positive integers, the correct type of time and actual players can be input
                If IsNumeric(chosenPlayer) = True And IsNumeric(timeSelect) And IsNumeric(timeIncrease) Then
                    If 0 <= chosenPlayer And chosenPlayer <= numOfPlayerInSystem - 1 Then
                        If 0 < timeSelect And timeSelect < 3 Then
                            If timeIncrease > 0 Then
                                typeCheck = True
                            Else
                                ErrorMessage("Less Than 0")
                            End If
                        Else
                            ErrorMessage("Invalid Time")
                        End If
                    Else
                        ErrorMessage("Invalid Player Choice")
                    End If
                Else
                    ErrorMessage("Input Data Type")
                End If
            End While

            'Calculates the increase in stat by adding onto the original time and sets the new value into the correct position in the array
            PlayerTimesArray(chosenPlayer, timeSelect) = PlayerTimesArray(chosenPlayer, timeSelect) + timeIncrease

            'writes player times array into player times text file
            objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Player Times.txt")
            'loops through how many players are currently in the system
            For x = 0 To placenumber - 1
                'loops through all the columns in the array (place number and all the times)
                For y = 0 To 2
                    'writes all of the values stored in the player times array into the file
                    writeIt = PlayerTimesArray(x, y)
                    objStreamWriter.WriteLine(writeIt)
                Next
            Next
            objStreamWriter.Close()

            'prompts coach of success
            Console.WriteLine("Time has been updated for " & playerDetailsArray(chosenPlayer, 1))

            'allows user to repeat with a different player
            Console.WriteLine("Enter 1 To Reuse Section - Any other input will return to menu")
            Console.ForegroundColor = ConsoleColor.Cyan
            continueLoop = Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White

            Select Case continueLoop
                Case 1
                    loopSection = True
                    Console.Clear()
                Case Else
                    loopSection = False
            End Select
        End While
        CoachMenu()
    End Sub


    'Subroutine = This allows users to receive a sorted list for a specific type of match statistic (to fulfil the 5th objective) allows you to input a statistic to be sorted which gets validated. The correct statistics and their corresponding place numbers (which represents the players) get placed in a sortedList() Array. The statistics get bubble sorted with the place numbers being swapped at the same time to ensure the names can be correctly matched up with the statistic. The array then is used to output the player’s name, position and the match statistics in a sorted list. All of the match statistics are added up then divided by the number of players in the system to output the statistic average.
    Sub SortMatchStats()
        loopSection = True

        While loopSection = True
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("You have chosen to get a Sorted List Of a Chosen Match Stat")
            Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White

            SetUpArrays()

            'The array alongside the variable allows the system to perform a bubble sort on variables which will output the player's match stats | Will store integers which are the player match stat value and their placenumber
            Dim temp, sortedList(20, 1) As Integer
            'Allows the user to choose which statistic they want | e.g. value stored = 3
            Dim statSelect As String
            'Allows system to loop until valid data has been input | True or False
            Dim typeCheck = False
            'All of these variables will be local since it will make my program more efficient due to it requiring less system memory

            'allows player to choose what statistic they want sorted
            While typeCheck = False
                Console.Clear()
                Console.WriteLine("Enter the corresponding number for which stat you want to see sorted")
                Console.WriteLine(" 1 = minutes played, 2 = Goals scored, 3 = Assists made, 4 = Passes made, 5 = Tackles made")
                Console.WriteLine("6 = Fouls made, 7 = Saves made, 8 = Goals conceded , 9 = Yellow cards , 10 = Red Cards")
                Console.ForegroundColor = ConsoleColor.Cyan
                statSelect = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White

                Console.WriteLine("Players with 0 in the chosen stat will not be shown")
                'Validation - Data Type / Range / Presence Checks - Ensures only valid types of match statistics can be input to get sorted
                If IsNumeric(statSelect) = True Then
                    If 0 < statSelect And statSelect < 11 Then
                        typeCheck = True
                    Else
                        ErrorMessage("Invalid Stat")
                    End If
                Else
                    ErrorMessage("Input Data Type")
                End If
            End While

            'sorts chosen statistic value and placevalues from original array in new array that will get sorted
            For x = 0 To 20
                'column 1 is the place value
                sortedList(x, 0) = playerDetailsArray(x, 0)
                'column 2 is the match statistic
                sortedList(x, 1) = PlayerMatchStatisticsArray(x, statSelect)
            Next

            'Bubble Sort = This will be used since it is the easiest sort to code (which will also allow me to sort in two dimensions) and is capable of sorting lists of any size (in this case up to 20). It is also a sufficient speed for me.
            'checks through array to ensure it has been sorted
            For x = 0 To 2
                'loops through entire array
                For y = 0 To 19
                    'checks if the value is larger than the next (it is comparing the match statistic)
                    If sortedList(y, 1) > sortedList(y + 1, 1) Then
                        'stores 2nd value into temporary variable so it can be switched
                        temp = sortedList(y + 1, 1)
                        'first value gets placed second
                        sortedList(y + 1, 1) = sortedList(y, 1)
                        'the value in the temporary value get set as the first value
                        sortedList(y, 1) = temp

                        'the place value gets switched around so that the correct player and match statistics can be matched up correctly with the same method
                        temp = sortedList(y + 1, 0)
                        sortedList(y + 1, 0) = sortedList(y, 0)
                        sortedList(y, 0) = temp
                        'if a switch has taken place the system will start the comparisons from the beginning of the array again
                        y = 0
                    End If
                Next
            Next

            Console.WriteLine()
            'prompts what has been sorted
            Console.ForegroundColor = ConsoleColor.Magenta
            Select Case statSelect
                Case 1
                    Console.WriteLine("Minutes Played")
                Case 2
                    Console.WriteLine("Goals scored")
                Case 3
                    Console.WriteLine("Assists made")
                Case 4
                    Console.WriteLine("Passes made")
                Case 5
                    Console.WriteLine("Tackles made")
                Case 6
                    Console.WriteLine("Fouls made")
                Case 7
                    Console.WriteLine("Saves made ")
                Case 8
                    Console.WriteLine("Goals conceded ")
                Case 9
                    Console.WriteLine("Yellow cards ")
                Case 10
                    Console.WriteLine("Red Cards")
            End Select
            Console.ForegroundColor = ConsoleColor.White

            'displays sorted list with name and match statistic
            For x = 0 To 20
                'if statement stops any players with nothing in the value from appearing
                If sortedList(x, 1) > 0 Then
                    'outputs players name using place value
                    Console.ForegroundColor = ConsoleColor.Magenta
                    Console.WriteLine(playerDetailsArray(sortedList(x, 0), 1))
                    Console.ForegroundColor = ConsoleColor.White
                    'outputs players position using place value
                    Console.WriteLine(playerDetailsArray(sortedList(x, 0), 3))
                    'outputs the match statistic value
                    Console.WriteLine(sortedList(x, 1))
                End If
            Next
            'NEW FEATURE/SECTION
            'Allows system to calculate and output what the average is for a certain statistic
            Dim averageStat As Decimal
            'Calculates the average statistic by adding up all the correct values and dividing by the number of players in the system, then outputs a message stating the average value. The user can then use this value to gain a better comprehension of how the team is doing overall and how each player stacks compared to the average statistic.
            averageStat = 0
            For x = 0 To numOfPlayerInSystem
                averageStat = averageStat + PlayerMatchStatisticsArray(x, statSelect)
            Next
            averageStat = averageStat / numOfPlayerInSystem
            Console.WriteLine("")
            Console.WriteLine("Average Value for All Players is " & averageStat)
            Console.WriteLine("(existing players with 0 in this value are included)")

            'allows user to repeat section with another statistic
            Console.WriteLine("Enter 1 To Reuse Section - Any other input will return to menu")
            Console.ForegroundColor = ConsoleColor.Cyan
            continueLoop = Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White

            Select Case continueLoop
                Case 1
                    loopSection = True
                    Console.Clear()
                Case Else
                    loopSection = False
            End Select
        End While

        CoachMenu()
    End Sub

    'Subroutine = This allows users to receive a sorted list for a specific type of time (to fulfil the 6th objective) allows you to input a time to be sorted which gets validated. The correct times and their corresponding place numbers (which represents the players) get placed in a sortedList() Array. The statistics get bubble sorted with the place numbers being swapped at the same time to ensure the names are correctly matched up with the times. The array then is used to output the player’s name, position and the times in a sorted list. The times are added up then divided by the number of players in the system to output the time average.
    Sub SortTimes()
        loopSection = True

        While loopSection = True
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("You have chosen to get a Sorted List Of a Chosen Time")
            Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White
            SetUpArrays()

            'The array alongside the variable allows the system to perform a bubble sort on variables which will output the player's match stats | Will store integers which are the player match stat value and their placenumber
            Dim temp, sortedList(20, 1)
            'Allows user to choose between training and academic times | e.g.  value stored = 1
            Dim timeSelect As String
            'Allows system to loop until valid data has been input | True or False
            Dim typeCheck = False
            'All of these variables will be local since it will make my program more efficient due to it requiring less system memory

            'allows user to choose what to get sorted
            While typeCheck = False
                Console.Clear()
                Console.WriteLine("Enter the corresponding number for which time you want to see sorted")
                Console.WriteLine("Enter 1 for training times, Enter 2 for academic times")
                Console.ForegroundColor = ConsoleColor.Cyan
                timeSelect = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White

                Console.WriteLine("Players with 0 in the chosen stat will not be shown")
                'Validation - Data Type / Range / Presence Checks - Ensures only valid types of time can be input to get sorted
                If IsNumeric(timeSelect) = True Then
                    If 0 < timeSelect And timeSelect < 3 Then
                        typeCheck = True
                    Else
                        ErrorMessage("Input Time")
                    End If
                Else
                    ErrorMessage("Input Data Type")
                End If
            End While

            'sorts placenumbers and times into new array
            For x = 0 To 20
                'column 1 is the place value
                sortedList(x, 0) = playerDetailsArray(x, 0)
                ''column 2 is the times
                sortedList(x, 1) = PlayerTimesArray(x, timeSelect)
            Next

            'Bubble Sort = This will be used since it is the easiest sort to code (which will also allow me to sort in two dimensions) and is capable of sorting lists of any size (in this case up to 20). It is also a sufficient speed for me.
            'checks through array to ensure it has been sorted
            For x = 0 To 2
                For y = 0 To 19
                    'checks if the value is larger than the next (it is comparing the times)
                    If sortedList(y, 1) > sortedList(y + 1, 1) Then
                        'stores 2nd value into temporary variable so it can be switched
                        temp = sortedList(y + 1, 1)
                        'first value gets placed second
                        sortedList(y + 1, 1) = sortedList(y, 1)
                        'the value in the temporary value get set as the first value
                        sortedList(y, 1) = temp

                        'the place value gets switched around so that the correct player and match statistics can be matched up correctly with the same method
                        temp = sortedList(y + 1, 0)
                        sortedList(y + 1, 0) = sortedList(y, 0)
                        sortedList(y, 0) = temp
                        y = 0
                    End If
                Next
            Next
            Console.WriteLine()
            'prompts what has been sorted
            Console.ForegroundColor = ConsoleColor.Magenta
            Select Case timeSelect
                Case 1
                    Console.WriteLine("Training Time")
                Case 2
                    Console.WriteLine("Academic Time")
            End Select
            Console.WriteLine()

            'displays sorted list with name and match statistic
            For x = 0 To 20
                'if statement stops any players with nothing in the value from appearing
                If sortedList(x, 1) > 0 Then
                    'outputs players name using place value
                    Console.ForegroundColor = ConsoleColor.Magenta
                    Console.WriteLine(playerDetailsArray(sortedList(x, 0), 1))
                    Console.ForegroundColor = ConsoleColor.White
                    'outputs players position using place value
                    Console.WriteLine(playerDetailsArray(sortedList(x, 0), 3))
                    'outputs the match statistic value
                    Console.WriteLine(sortedList(x, 1))
                End If
            Next
            'Allows system to calculate and output what the average is for a certain time
            Dim averageTime As Decimal
            averageTime = 0
            'Calculates the average time by adding up all the correct values and dividing by the number of players in the system, then outputs a message stating the average value. The user can then use this value to gain a better comprehension of how the team is doing overall and how each player stacks compared to the average statistic.
            For x = 0 To numOfPlayerInSystem
                averageTime = averageTime + PlayerTimesArray(x, timeSelect)
            Next
            averageTime = averageTime / numOfPlayerInSystem
            Console.WriteLine("")
            Console.WriteLine("Average Value for All Players is " & averageTime)
            Console.WriteLine("(players with 0 in this value are included)")
            Console.ReadLine()

            Console.ForegroundColor = ConsoleColor.White
            'allows user to try again with a different time
            Console.WriteLine("Enter 1 To Reuse Section - Any other input will return to menu")
            Console.ForegroundColor = ConsoleColor.Cyan
            continueLoop = Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White

            Select Case continueLoop
                Case 1
                    loopSection = True
                    Console.Clear()
                Case Else
                    loopSection = False
            End Select
        End While

        CoachMenu()
    End Sub

    'Subroutine = This allows the user receive a list of all the information (their personal details, match statistics and times to fulfil the 7th objective). The user first chooses a player based on their place number and then the system uses the place number to output all that player’s information using the arrays (PlayerDetailsArray, PlayerMatchStatisticsArray, PlayerTimesArray) to output the correct information.
    Sub OutputOnePlayer()
        loopSection = True

        While loopSection = True
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("You have chosen to Output All the Data on One Player")
            Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White
            SetUpArrays()
            'Allows system to loop until valid data has been input | True or False
            Dim typeCheck = False
            'Allows user to choose from a list of players using a number | e.g. value stored = 3
            Dim chosenPlayer As String
            'All of these variables are local because it will make my program more efficient due to local variables requiring less system memory

            While typeCheck = False
                Console.Clear()
                'shows coach available choices
                placenumber = 0
                Do Until playerDetailsArray(placenumber, 1) = ""
                    'outputs player's place value
                    Console.WriteLine(playerDetailsArray(placenumber, 0))
                    'outputs player's name
                    Console.WriteLine(playerDetailsArray(placenumber, 1))
                    Console.WriteLine()
                    placenumber = placenumber + 1
                Loop

                'allows coach to choose a player
                Console.WriteLine("Enter place value of the player you want")
                Console.ForegroundColor = ConsoleColor.Cyan
                chosenPlayer = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White
                'Validation - Data Type / Range / Presence Checks - Ensures non existent players cannot be input
                If IsNumeric(chosenPlayer) = True Then
                    If 0 <= chosenPlayer And chosenPlayer < numOfPlayerInSystem Then
                        typeCheck = True
                    Else
                        ErrorMessage("Invalid Data Type")
                    End If
                Else
                    ErrorMessage("Input Data Type")
                End If
            End While

            Console.Clear()
            Console.WriteLine()
            Console.ForegroundColor = ConsoleColor.DarkGreen
            'outputs all the player personal details
            For x = 1 To 3
                Console.WriteLine(playerDetailsArray(chosenPlayer, x))
            Next
            Console.WriteLine()
            'outputs all of the player's match statistics with the appropriate headings
            For x = 1 To 10
                Select Case x
                    Case 1
                        Console.WriteLine("Minutes Played")
                    Case 2
                        Console.WriteLine("Goals scored")
                    Case 3
                        Console.WriteLine("Assists made")
                    Case 4
                        Console.WriteLine("Passes made")
                    Case 5
                        Console.WriteLine("Tackles made")
                    Case 6
                        Console.WriteLine("Fouls made")
                    Case 7
                        Console.WriteLine("Saves made ")
                    Case 8
                        Console.WriteLine("Goals conceded ")
                    Case 9
                        Console.WriteLine("Yellow cards ")
                    Case 10
                        Console.WriteLine("Red Cards")
                End Select
                Console.WriteLine(PlayerMatchStatisticsArray(chosenPlayer, x))
            Next

            'outputs all of the player's training time with the correct headings
            Console.WriteLine()
            For x = 1 To 2
                Select Case x
                    Case 1
                        Console.WriteLine("Training Time")
                    Case 2
                        Console.WriteLine("Academic Time")
                End Select
                Console.WriteLine(PlayerTimesArray(chosenPlayer, x))
            Next
            'allows user to try again with a different player
            Console.ReadLine()
            Console.WriteLine("Enter 1 To Reuse Section - Any other input will return to menu")
            continueLoop = Console.ReadLine()

            Select Case continueLoop
                Case 1
                    loopSection = True
                    Console.Clear()
                Case Else
                    loopSection = False
            End Select
        End While
        CoachMenu()
    End Sub

    'Subroutine = This allow users to receive a sorted list for a specific type of match statistic, but which has been searched for a specific value (to fulfil the 8th objective). The user first chooses which statistic is to be searched and sorted through. The correct statistics and their corresponding place numbers (which represents the players) get placed in a sortedList() Array. The statistics get bubble sorted with the place numbers being swapped at the same time to ensure the names are correctly matched up with the statistic. The user then chooses the value the search is based on and whether they want a list lower or greater than that value (which gets validated). Then, only values which fit that criteria get copied into the searchedArray() which uses the place numbers and statistics to output the list of player names, positions and statistics. Then the system looks through the searchedArray() to see how many players fit the criteria. The system allows you choose whether you want to search through that array for a position and only will output the players which had the right position.
    Sub SearchMatchStats()
        loopSection = True

        While loopSection = True
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("You have chosen to Search For a Specific Match Stat")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            SetUpArrays()

            'The array alongside the variable allows the system to perform a bubble sort on variables which will output the player's match stats | Will store integers which are the player match stat value and their placenumber
            Dim temp, sortedList(20, 1) As Integer
            'Allows the user to choose which statistic they want | e.g. value stored = 3
            Dim statSelect As String
            'Allows system to loop until valid data has been input | True or False
            Dim typeCheck = False
            'All of these variables will be local since it will make my program more efficient due to it requiring less system memory

            'allows user to choose what statistic gets sorted and searched
            While typeCheck = False
                Console.Clear()
                Console.WriteLine("Enter the corresponding number for which stat you want to see searched")
                Console.WriteLine(" 1 = minutes played, 2 = Goals scored, 3 = Assists made, 4= Passes made, 5 = Tackles made")
                Console.WriteLine("6 = Fouls made, 7 = Saves made, 8 = Goals conceded , 9 = Yellow cards , 10 = Red Cards")
                Console.ForegroundColor = ConsoleColor.Cyan
                statSelect = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White

                Console.WriteLine("Players with 0 in the chosen stat will not be shown")
                ' 'Validation - Data Type / Range / Presence Checks - Ensures only valid statistics can be input
                If IsNumeric(statSelect) = True Then
                    If 0 < statSelect And statSelect < 11 Then
                        typeCheck = True
                    Else
                        ErrorMessage("Invalid Stat")
                    End If
                Else
                    ErrorMessage("Input Data Type")
                End If
            End While

            'sorts chosen statistic value and placevalues from original array in new array that will get sorted
            For x = 0 To 20
                'column 1 is the place value
                sortedList(x, 0) = playerDetailsArray(x, 0)
                'column 2 is the match statistic
                sortedList(x, 1) = PlayerMatchStatisticsArray(x, statSelect)
            Next

            'Bubble Sort = This will be used since it is the easiest sort to code (which will also allow me to sort in two dimensions) and is capable of sorting lists of any size (in this case up to 20). It is also a sufficient speed for me.
            'checks through array to ensure it has been sorted
            For x = 0 To 2
                'loops through entire array
                For y = 0 To 19
                    'checks if the value is larger than the next (it is comparing the match statistic)
                    If sortedList(y, 1) > sortedList(y + 1, 1) Then
                        'stores 2nd value into temporary variable so it can be switched
                        temp = sortedList(y + 1, 1)
                        'first value gets placed second
                        sortedList(y + 1, 1) = sortedList(y, 1)
                        'the value in the temporary value get set as the first value
                        sortedList(y, 1) = temp

                        'the place value gets switched around so that the correct player and match statistics can be matched up correctly with the same method
                        temp = sortedList(y + 1, 0)
                        sortedList(y + 1, 0) = sortedList(y, 0)
                        sortedList(y, 0) = temp
                        'if a switch has taken place the system will start the comparisons from the beginning of the array again
                        y = 0
                    End If
                Next
            Next

            'This new array gets the values from the sortedList which fit the criteria that has been given by the player
            Dim searchedArray(20, 1) As Integer
            'Allows user to choose whether the values searched are higher or lower than their search value | e.g. value stored = 1
            'Allows the user to choose what value they are searching around/ for | e.g. value stored = 15
            Dim maxOrMin, searchParameter As String
            'Allows system to loop until valid data has been input | True or False
            Dim maxMinCheck = False
            'All of these variables will be local since it will make my program more efficient due to it requiring less system memory

            While maxMinCheck = False
                'allows coach to either choose a list higher or lower than a specific value
                Console.WriteLine("Enter 1 if you want a list higher than your chosen value , Enter 2 if you want a list lower than the chosen value")
                Console.ForegroundColor = ConsoleColor.Cyan
                maxOrMin = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White
                'Validation - Data Type / Range / Presence Checks - Ensures that the user can only choose to get a higher or lower list and nothing else
                If IsNumeric(maxOrMin) = True Then
                    If 0 < maxOrMin And maxOrMin < 3 Then
                        maxMinCheck = True
                    Else
                        ErrorMessage("Invalid Choice")
                    End If
                Else
                    ErrorMessage("Input Data Type")
                End If
            End While
            'Allows system to loop until valid data has been input | True or False
            Dim ParameterCheck = False

            While ParameterCheck = False
                'allows user to choose what the search will be based around
                Console.WriteLine("Enter the chosen value to base your search on")
                Console.ForegroundColor = ConsoleColor.Cyan
                searchParameter = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White
                'Validation - Data Type / Range / Presence Checks - Ensures only positive intergers can be input
                If IsNumeric(searchParameter) = True Then
                    If searchParameter > 0 Then
                        ParameterCheck = True
                    Else
                        ErrorMessage("Less Than 0")
                    End If
                Else
                    ErrorMessage("Input Data Type")
                End If
            End While

            Console.WriteLine()
            'prompts what was searched
            Console.ForegroundColor = ConsoleColor.Magenta
            Select Case statSelect
                Case 1
                    Console.WriteLine("Minutes Played")
                Case 2
                    Console.WriteLine("Goals scored")
                Case 3
                    Console.WriteLine("Assists made")
                Case 4
                    Console.WriteLine("Passes made")
                Case 5
                    Console.WriteLine("Tackles made")
                Case 6
                    Console.WriteLine("Fouls made")
                Case 7
                    Console.WriteLine("Saves made ")
                Case 8
                    Console.WriteLine("Goals conceded ")
                Case 9
                    Console.WriteLine("Yellow cards ")
                Case 10
                    Console.WriteLine("Red Cards")
            End Select
            Console.ForegroundColor = ConsoleColor.White

            'Linear Search = Since a smaller data set will be used the search will be a sufficient speed whilst remaining simpler to code than a binary search
            Select Case maxOrMin
            ' if 1 was pressed a list higher than the search parameter will be made
                Case 1
                    'loops through the sorted list and sets values which fit the search parameter into a new "searched" array
                    For x = 0 To 20
                        If sortedList(x, 1) > searchParameter Then
                            'sets place value
                            searchedArray(x, 0) = sortedList(x, 0)
                            'sets match statistic
                            searchedArray(x, 1) = sortedList(x, 1)
                        End If
                    Next

            ' if 2 was pressed a list lower than the search parameter will be made
                Case 2
                    'loops through the sorted list and sets values which fit the search parameter into a new "searched" array
                    For x = 0 To 20
                        If sortedList(x, 1) < searchParameter Then
                            'sets place value
                            searchedArray(x, 0) = sortedList(x, 0)
                            'sets match statistic
                            searchedArray(x, 1) = sortedList(x, 1)
                        End If
                    Next
            End Select

            'outputs searched List
            For x = 0 To 20
                'if statement stops any players with nothing in the value from appearing
                If searchedArray(x, 1) > 0 Then
                    'outputs players name using place value
                    Console.WriteLine(playerDetailsArray(searchedArray(x, 0), 1))
                    'outputs players position using place value
                    Console.WriteLine(playerDetailsArray(searchedArray(x, 0), 3))
                    'outputs the match statistic value
                    Console.WriteLine((searchedArray(x, 1)))
                End If
            Next

            'Allows system to work out how many players fit the criteria given out of the total players in the system | e.g. value stored = 5
            Dim PlayerWhichFit = 21
            Console.WriteLine("")
            'NEW FEATURE/SECTION
            'Calculates out how many players fit the criteria by taking away the empty spaces in the searched array from the total size of the array. The user can work out how many players are fitting the performance required and how many are slacking behind and require more training.
            For x = 0 To 20
                If searchedArray(x, 1) = 0 Then
                    PlayerWhichFit = PlayerWhichFit - 1
                End If
            Next

            Console.WriteLine(PlayerWhichFit & " out of " & numOfPlayerInSystem & " players have met the criteria")

            Console.WriteLine()
            'NEW FEATURE/SECTION
            'Allows user to decide whether they want to search for a specific position | e.g. value stored = 1
            Dim postionSearch As String
            Console.WriteLine("Enter 1 To refine this search for one positon, Any other input will continue section")
            Console.ForegroundColor = ConsoleColor.Cyan
            postionSearch = Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White

            Select Case postionSearch
                Case 1
                    'Allows user to choose which player to choose a specific position to search for | e.g. value stored = FWD
                    Dim positionChoice As String
                    Console.WriteLine("Enter the position you want FWD,MID,DEF or GKP (Incorrect Inputs Will Result in the Program Continuing)")
                    Console.ForegroundColor = ConsoleColor.Cyan
                    positionChoice = Console.ReadLine()
                    Console.ForegroundColor = ConsoleColor.White
                    Console.WriteLine()
                    'outputs only the players with the right positions from the searchedArray
                    For x = 0 To 20
                        If playerDetailsArray(searchedArray(x, 0), 3) = positionChoice Then
                            If searchedArray(x, 1) > 0 Then
                                Console.WriteLine(playerDetailsArray(searchedArray(x, 0), 1))
                                Console.WriteLine(searchedArray(x, 1))
                                Console.WriteLine(playerDetailsArray(searchedArray(x, 0), 3))
                            End If
                        End If
                    Next
                Case Else
            End Select

            'allows you to search with different criteria
            Console.WriteLine("Enter 1 To Reuse Section - Any other input will return to menu")
            Console.ForegroundColor = ConsoleColor.Cyan
            continueLoop = Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White

            Select Case continueLoop
                Case 1
                    loopSection = True
                    Console.Clear()
                Case Else
                    loopSection = False
            End Select
        End While
        CoachMenu()
    End Sub

    'Subroutine = This allow users to receive a sorted list for a specific type of time, but which has been searched for a specific value (to fulfil the 9th objective). The user first chooses which time is to be searched and sorted through. The correct times and their corresponding place numbers (which represents the players) get placed in a sortedList() Array. The times get bubble sorted with the place numbers being swapped at the same time to ensure the names are correctly matched up with the time. The user then chooses the value the search is based on and whether they want a list lower or greater than that value (which gets validated). Then, only values which fit that criteria get copied into the searchedArray() which uses the place numbers and times to output the list of player names, positions and times. Then the system looks through the searchedArray() to see how many players fit the criteria. The system allows you choose whether you want to search through that array for a position and only will output the players which had the right position.
    Sub SearchPlayerTimes()
        loopSection = True

        While loopSection = True
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("You have chosen to Search For a Specific Time")
            Console.ForegroundColor = ConsoleColor.White
            SetUpArrays()
            Console.ReadLine()

            'The array alongside the variable allows the system to perform a bubble sort on variables which will output the player's match stats | Will store integers which are the player match stat value and their placenumber
            Dim temp, sortedList(20, 1) As Integer
            'Allows user to choose between training and academic times | e.g. value stored = 1
            Dim timeSelect As String
            'Allows system to loop until valid data has been input | True or False
            Dim timeChoiceCheck = False
            'All of these variables will be local since it will make my program more efficient due to it requiring less system memory

            While timeChoiceCheck = False
                Console.Clear()
                'allows user to choose which time to be sorted and searched
                Console.WriteLine("Enter 1 for training time, Enter 2 for academic time")
                Console.ForegroundColor = ConsoleColor.Cyan
                timeSelect = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White
                'Validation - Data Type / Range / Presence Checks - Ensures the user can only choose to have one of the types of time searched
                If IsNumeric(timeSelect) = True Then
                    If 0 < timeSelect And timeSelect < 3 Then
                        timeChoiceCheck = True
                    Else
                        ErrorMessage("Invalid Time")
                    End If
                Else
                    ErrorMessage("Input Data Type")
                End If
            End While
            Console.WriteLine("Players with 0 in the chosen stat will not be shown")

            'sorts chosen statistic value and place numbers from original array in new array that will get sorted
            For x = 0 To 20
                'column 1 is the place value
                sortedList(x, 0) = playerDetailsArray(x, 0)
                'column 2 is the match statistic
                sortedList(x, 1) = PlayerTimesArray(x, timeSelect)
            Next

            'Bubble Sort = This will be used since it is the easiest sort to code (which will also allow me to sort in two dimensions) and is capable of sorting lists of any size (in this case up to 20). It is also a sufficient speed for me.
            'checks through array to ensure it has been sorted
            For x = 0 To 2
                For y = 0 To 19
                    'checks if the value is larger than the next (it is comparing the times)
                    If sortedList(y, 1) > sortedList(y + 1, 1) Then
                        'stores 2nd value into temporary variable so it can be switched
                        temp = sortedList(y + 1, 1)
                        'first value gets placed second
                        sortedList(y + 1, 1) = sortedList(y, 1)
                        'the value in the temporary value get set as the first value
                        sortedList(y, 1) = temp

                        'the place value gets switched around so that the correct player and match statistics can be matched up correctly with the same method
                        temp = sortedList(y + 1, 0)
                        sortedList(y + 1, 0) = sortedList(y, 0)
                        sortedList(y, 0) = temp
                        'if a switch has taken place the system will start the comparisons from the beginning of the array again
                        y = 0
                    End If
                Next
            Next

            'This new array gets the values from the sortedList which fit the criteria that has been given by the player
            Dim searchedArray(20, 1) As Integer
            'Allows user to choose whether the values searched are higher or lower than their search value | e.g. value stored = 1
            'Allows the user to choose what value they are searching around/ for | e.g. value stored = 15
            Dim maxOrMin, searchParameter As String
            'Allows system to loop until valid data has been input | True or False
            Dim maxMinCheck = False
            'All of these variables will be local since it will make my program more efficient due to it requiring less system memory

            While maxMinCheck = False
                'allows coach to either choose a list higher or lower
                Console.WriteLine("Enter 1 if you want a list higher than your chosen value , Enter 2 if you want a list lower than your chosen value")
                Console.ForegroundColor = ConsoleColor.Cyan
                maxOrMin = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White
                'Validation - Data Type / Range / Presence Checks - Ensures that the user can only choose to get a higher or lower list and nothing else
                If IsNumeric(maxOrMin) = True Then
                    If 0 < maxOrMin And maxOrMin < 3 Then
                        maxMinCheck = True
                    Else
                        ErrorMessage("Invalid Choice")
                    End If
                Else
                    ErrorMessage("Input Data Type")
                End If
            End While
            'Allows system to loop until valid data has been input | True or False
            Dim ParameterCheck = False

            While ParameterCheck = False
                'allsows user to choose what the search will be based around
                Console.WriteLine("Enter the chosen value to base your search on")
                Console.ForegroundColor = ConsoleColor.Cyan
                searchParameter = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White
                'Validation - Data Type / Range / Presence Checks - Ensures that the user can enter positive integers
                If IsNumeric(searchParameter) = True Then
                    ParameterCheck = True
                Else
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Invalid Data Type")
                    Console.ReadLine()
                    Console.ForegroundColor = ConsoleColor.White
                End If

            End While

            'Linear Search = Since a smaller data set will be used the search will be a sufficient speed whilst remaining simpler to code than a binary search
            Select Case maxOrMin
            'if 1 was pressed a list higher than the search parameter will be made
                Case 1
                    'loops through the sorted list and sets values which fit the search parameter into a new "searched" array
                    For x = 0 To 20
                        If sortedList(x, 1) > searchParameter Then
                            'sets place value
                            searchedArray(x, 0) = sortedList(x, 0)
                            'set times
                            searchedArray(x, 1) = sortedList(x, 1)
                        End If
                    Next

                ' if 2 was pressed a list lower than the search parameter will be made
                Case 2
                    'loops through the sorted list and sets values which fit the search parameter into a new "searched" array
                    For x = 0 To 20
                        If sortedList(x, 1) < searchParameter Then
                            'sets place value
                            searchedArray(x, 0) = sortedList(x, 0)
                            'sets times
                            searchedArray(x, 1) = sortedList(x, 1)
                        End If
                    Next

            End Select
            Console.WriteLine()

            'prompts what has been sorted
            Console.ForegroundColor = ConsoleColor.Green
            Select Case timeSelect
                Case 1
                    Console.WriteLine("Training Time")
                Case 2
                    Console.WriteLine("Academic Time")
            End Select
            Console.WriteLine()

            'outputs searched List
            Console.ForegroundColor = ConsoleColor.White
            For x = 0 To 20
                'if statement stops any players with nothing in the value from appearing
                If searchedArray(x, 1) > 0 Then
                    'outputs players name using place value
                    Console.WriteLine(playerDetailsArray(searchedArray(x, 0), 1))
                    'outputs players position using place value
                    Console.WriteLine(playerDetailsArray(searchedArray(x, 0), 3))
                    'outputs the match statistic value
                    Console.WriteLine((searchedArray(x, 1)))
                End If
            Next
            Console.ReadLine()

            'Allows system to work out how many players fit the criteria given out of the total players in the system | e.g. value stored = 5
            Dim PlayerWhichFit = 21
            Console.WriteLine("")

            'Calculates out how many players fit the criteria by taking away the empty spaces in the searched array from the total size of the array. The user can work out how many players are fitting the performance required and how many are slacking behind and require more training.
            For x = 0 To 20
                If searchedArray(x, 1) = 0 Then
                    PlayerWhichFit = PlayerWhichFit - 1
                End If
            Next

            Console.WriteLine(PlayerWhichFit & " out of " & numOfPlayerInSystem & " players have met the criteria")
            Console.WriteLine()

            'Allows user to decide whether they want to search for a specific position| e.g. value stored = 1
            Dim postionSearch As String
            Console.WriteLine("Enter 1 To refine this search for one positon, Any other input will continue section")
            Console.ForegroundColor = ConsoleColor.Cyan
            postionSearch = Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White
            'NEW FEATURE/SECTION
            Select Case postionSearch
                Case 1
                    'Allows user to choose which player to choose a specific position to search for | e.g. value stored = FWD
                    Dim positionChoice As String
                    Console.WriteLine("Enter the position you want FWD,MID,DEF or GKP (Incorrect Inputs Will Result in the Program Continuing)")
                    Console.ForegroundColor = ConsoleColor.Cyan
                    positionChoice = Console.ReadLine()
                    Console.ForegroundColor = ConsoleColor.White
                    Console.WriteLine()
                    'outputs only the players with the right positions from the searchedArray
                    For x = 0 To 20
                        If playerDetailsArray(searchedArray(x, 0), 3) = positionChoice Then
                            If searchedArray(x, 1) > 0 Then
                                Console.WriteLine(playerDetailsArray(searchedArray(x, 0), 1))
                                Console.WriteLine(searchedArray(x, 1))
                                Console.WriteLine(playerDetailsArray(searchedArray(x, 0), 3))
                            End If
                        End If
                    Next
                Case Else
            End Select

            'allows you to search with different criteria
            Console.WriteLine("Enter 1 To Reuse Section - Any other input will return to menu")
            Console.ForegroundColor = ConsoleColor.Cyan
            continueLoop = Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White

            Select Case continueLoop
                Case 1
                    loopSection = True
                    Console.Clear()
                Case Else
                    loopSection = False
            End Select
        End While
        CoachMenu()
    End Sub

    'Subroutine = This allow users to get a random recommendation on a player who is doing well (to fulfil the 10th objective). The system uses the random function to select a random player and statistic to choose a value from the PlayerMatchStatisticsArray(). If the value is above the pre-set goal, then a recommendation will be output (unless after 50 attempts a player can’t be found) congratulating the player on their performance.
    Sub SuggestGoodPlayer()
        loopSection = True

        While loopSection = True
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("You have chosen to Suggest a Random Player who is Doing Well")
            Console.ForegroundColor = ConsoleColor.White
            SetUpArrays()

            'Allows system to choose a random match statistic to look at for a player to see whether they've done well
            'Allows system to choose a random player to look at to  see whether they've done well
            'Stops system from being stuck in an infinite loop if no good players can be found
            'Combines both the randStat and randPer to find a player and a statistic and if it beats a preset value they will be congratulated
            Dim randStat, randPer, limit, chosen As Integer
            'Allows system to end loop when a acceptable player is found
            Dim looping = True
            'All of these variables are local because it will make my program more efficient due to local variables requiring less system memory

            'loops until a player who has done well has been found or the system could not find anyone after 50 attempts
            limit = 0
            While (looping = True) And (limit < 50)
                'chooses a random stat to choose for a player (goals/assists/passes/tackles)
                randStat = (Rnd() * 4) + 2
                'chooses a random player using the number of players stored in the system
                randPer = (Rnd() * numOfPlayerInSystem)
                'chooses a value based on the random person and statistic that has been chosen
                chosen = PlayerMatchStatisticsArray(randPer, randStat)

                'if the person is high enough in the chosen value then they will be complimented on their stats
                If chosen > 15 Then
                    Console.Write("Well done to " & playerDetailsArray(randPer, 1) & "who has got " & PlayerMatchStatisticsArray(randPer, randStat))
                    Select Case randStat
                    'ensures the coach is aware in which statistic the player has done well in
                        Case 2
                            Console.Write(" goals")
                        Case 3
                            Console.Write(" assists")
                        Case 4
                            Console.Write(" passes")
                        Case 5
                            Console.Write(" tackles")
                    End Select
                    looping = False
                Else
                    looping = True
                    'allows the loop to terminate if a player cannot be found
                    limit = limit + 1
                End If
            End While

            'tells coach if a good player couldn't be found
            If limit > 48 Then
                Console.WriteLine("No players could be found")
            End If

            Console.WriteLine()
            'allows users to find another recommendation
            Console.WriteLine("Enter 1 To Reuse Section - Any other input will return to menu")
            Console.ForegroundColor = ConsoleColor.Cyan
            continueLoop = Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White

            Select Case continueLoop
                Case 1
                    loopSection = True
                    Console.Clear()
                Case Else
                    loopSection = False
            End Select
        End While
        CoachMenu()
    End Sub

    'Subroutine = This allows users to get a recommendation for what the best position for a player is (to fulfil the 11th objective). First, a player is chosen by either by random or the user can choose who receives the suggestion. The system then checks three statistics (goals for attacking , passes for midfielding and tackles for defending for that player to calculate whether that player is good at that position. The suggestion is then output, and the user can choose to change the player’s position based on the suggestion. The value in the PlayerDetailsArray storing the player’s position can then be changed to a new position and the array gets rewritten to the Player Details.txt file.
    Sub SuggestGoodRole()
        loopSection = True

        While loopSection = True
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("You have chosen to Suggest the Best Role for a Player")
            Console.ForegroundColor = ConsoleColor.White
            SetUpArrays()

            'Allows user to choose from a list of players using a number | e.g. value stored = 3
            Dim chosenPlayer As String
            'All of these variables use either a true or false variable to decide a player is good at a particular position - True being good and false being bad
            Dim goodFWD, goodMID, goodDEF As Boolean
            'Allows user to choose whether they want to choose the player getting the suggested role or whether its random | e.g. value stored = 2
            Dim randOrExactChoice As String
            'Allows system to loop until valid data has been input | True or False
            Dim typeCheck = False
            'All of these variables are local because it will make my program more efficient due to local variables requiring less system memory
            'NEW FEATURE/SECTION
            While typeCheck = False
                'allows user to choose which player is getting a recommendation
                Console.WriteLine("Enter 1 to choose a specific player, Enter 2 to get a random choice")
                Console.ForegroundColor = ConsoleColor.Cyan
                randOrExactChoice = Console.ReadLine
                Console.ForegroundColor = ConsoleColor.White
                'type and range checks the input
                Select Case randOrExactChoice
                    Case 1
                        typeCheck = True
                    Case 2
                        typeCheck = True
                    Case Else
                        ErrorMessage("Invalid Choice")
                End Select
            End While

            typeCheck = False
            While typeCheck = False
                'type and range checks player choices
                Select Case randOrExactChoice
                    Case 1
                        'shows coach the available options to choose from
                        For x = 0 To numOfPlayerInSystem
                            Console.WriteLine(playerDetailsArray(x, 0))
                            Console.WriteLine(playerDetailsArray(x, 1))
                        Next
                        'allows user to choose player
                        Console.WriteLine("Enter the placevalue of your player")
                        Console.ForegroundColor = ConsoleColor.Cyan
                        chosenPlayer = Console.ReadLine()
                        Console.ForegroundColor = ConsoleColor.White
                        'Validation - Data Type / Range / Presence Checks - Ensures that the user cannot choose non existent players
                        If IsNumeric(chosenPlayer) Then
                            If -1 < chosenPlayer And chosenPlayer < numOfPlayerInSystem Then
                                typeCheck = True
                            Else
                                ErrorMessage("Invalid Player Choice")
                                Console.Clear()
                            End If
                        Else
                            ErrorMessage("Input Data Type")
                            Console.Clear()
                        End If
                    Case 2
                        'chooses random player based on number of players in system
                        chosenPlayer = (Rnd() * numOfPlayerInSystem)
                        typeCheck = True
                    Case Else
                        ErrorMessage("Invalid Choice")
                        Console.Clear()
                End Select
            End While

            'checks how well the random player has done in each statistic and sets either True or False for what position correlates to that statistic
            If PlayerMatchStatisticsArray(chosenPlayer, 2) > 7 Then
                goodFWD = True
            Else
                goodFWD = False
            End If

            If PlayerMatchStatisticsArray(chosenPlayer, 4) > 15 Then
                goodMID = True
            Else
                goodMID = False
            End If

            If PlayerMatchStatisticsArray(chosenPlayer, 5) > 12 Then
                goodDEF = True
            Else
                goodDEF = False
            End If

            'based on whether the player got true or false in each positon the correct message will be outputted
            Console.WriteLine(playerDetailsArray(chosenPlayer, 1) & " at current position " & playerDetailsArray(chosenPlayer, 3) & " seems to suit ")
            If goodFWD = True Then
                Console.WriteLine("FWD")
            End If

            If goodMID = True Then
                Console.WriteLine("MID")
            End If

            If goodDEF = True Then
                Console.WriteLine("DEF")
            End If
            'if you get false in all of them the correct message can be output
            If (goodDEF = False) And (goodFWD = False) And (goodMID = False) Then
                Console.WriteLine("none of these positions - Maybe this player needs more time to develop stats")
            Else
                Console.WriteLine("position/s")
            End If

            'Allows user to choose whether they want to change a players position based on the recommendation they recieved
            Dim changePosChoice As String
            Console.WriteLine("Enter 1 to change player position - Any other input will continue")
            Console.ForegroundColor = ConsoleColor.Cyan
            changePosChoice = Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White
            'NEW FEATURE/SECTION
            Select Case changePosChoice
                Case 1
                    'Allows system to loop until valid data has been input | True or False
                    Dim PositionCheck = False
                    'Allows user to choose from a list of different positions to which they can assign to the player | e.g. value stored = 3
                    Dim positionEntry As String
                    While PositionCheck = False
                        Console.WriteLine("Enter 1 if player is a forward, Enter 2 for midfielder, Enter 3 for defender or 4 for goalkeeper - Current Pos = " & playerDetailsArray(chosenPlayer, 3))
                        Console.ForegroundColor = ConsoleColor.Cyan
                        positionEntry = Console.ReadLine()
                        Console.ForegroundColor = ConsoleColor.White
                        'Validation - Data Type / Range / Presence Checks - Ensures that the user can only choose these four positions
                        Select Case positionEntry
                            Case 1
                                positionEntry = "FWD"
                                PositionCheck = True
                            Case 2
                                positionEntry = "MID"
                                PositionCheck = True
                            Case 3
                                positionEntry = "DEF"
                                PositionCheck = True
                            Case 4
                                positionEntry = "GKP"
                                PositionCheck = True
                            Case Else
                                ErrorMessage("Invalid Position")
                        End Select
                    End While
                    'writes player details array back to the text file including the updated position
                    playerDetailsArray(chosenPlayer, 3) = positionEntry
                    objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Personal Details.txt")
                    For x = 0 To placenumber
                        For y = 0 To 3
                            writeIt = playerDetailsArray(x, y)
                            objStreamWriter.WriteLine(writeIt)
                        Next
                    Next
                    objStreamWriter.Close()
                    Console.WriteLine("Task Completed")
                Case Else
            End Select

            'allows user to try again with another player
            Console.WriteLine("Enter 1 To Reuse Section - Any other input will return to menu")
            Console.ForegroundColor = ConsoleColor.Cyan
            continueLoop = Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White

            Select Case continueLoop
                Case 1
                    loopSection = True
                    Console.Clear()
                Case Else
                    loopSection = False
            End Select
        End While
        CoachMenu()
    End Sub

    'Subroutine = This allows the logged in player to update either their own training or academic times (to fulfil 4th objective). The player inputs the type of time to be updated and the value that it has been increased and then using these inputs and whoever the player logged in is, the system uses the PlayerTimesArray() to store the new value for that player. The array then gets written to the Player Times.txt file to be stored.
    Sub SelfTimeUpdate()
        loopSection = True

        While loopSection = True
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("You have chosen to update your training/ academic time")
            Console.ReadLine()
            Console.ForegroundColor = ConsoleColor.White

            'Allows user to choose between training and academic times | e.g. value stored = 2
            'Allows user to input how much their time has increased by  | e.g. value stored = 15
            Dim timeSelect, timeIncrease As String
            Dim typeCheck = False

            While typeCheck = False
                Console.Clear()
                SetUpArrays()

                'allows user to choose which time to increase
                Console.WriteLine("Enter 1 to add onto your training time, Enter 2 to add onto your academic time ")
                Console.ForegroundColor = ConsoleColor.Cyan
                timeSelect = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White
                'allows user to input how much the time has gone up by
                Console.WriteLine("Enter the value the time has increased by (in min) ")
                Console.ForegroundColor = ConsoleColor.Cyan
                timeIncrease = Console.ReadLine()
                Console.ForegroundColor = ConsoleColor.White
                'Validation - Data Type / Range / Presence Checks - Ensures that only correct types of time and positve integers can be input
                If IsNumeric(timeSelect) And IsNumeric(timeIncrease) Then
                    If 0 < timeSelect And timeSelect < 3 Then
                        If timeIncrease > 0 Then
                            typeCheck = True
                        Else
                            ErrorMessage("Less Than 0")
                        End If
                    Else
                        ErrorMessage("Invalid Time")
                    End If
                Else
                    ErrorMessage("Input Data Type")
                End If
            End While

            'Calculates the increase in stat by adding onto the original time and sets the new value into the correct position in the array
            PlayerTimesArray(loggedInPlayer, timeSelect) = PlayerTimesArray(loggedInPlayer, timeSelect) + timeIncrease

            'writes array back to the text file with updated values
            objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Player Times.txt")
            'loops through how many players are currently in the system
            For x = 0 To placenumber - 1
                For y = 0 To 2
                    'writes all of the values stored in the player match statistics array into the file
                    writeIt = PlayerTimesArray(x, y)
                    objStreamWriter.WriteLine(writeIt)
                Next
            Next
            objStreamWriter.Close()

            'prompts player of success
            Console.WriteLine("Time Updated")
            Console.ReadLine()
            'allows user to reuse section with the other time type
            Console.WriteLine("Enter 1 To Reuse Section - Any other input will return to menu")
            continueLoop = Console.ReadLine()

            Select Case continueLoop
                Case 1
                    loopSection = True
                    Console.Clear()
                Case Else
                    loopSection = False
            End Select
        End While
        PlayerMenu()
    End Sub

    'Subroutine = This allows users to create backup files and to restore replace any corrupted files with said backups. This in turn is to enforce objectives 2, 3 and 4 to ensure that all player data is stored safely and is at minimal risk of being lost. If the user chooses to update the backup files, the system will read from the original files into the arrays and then rewrite those arrays into the separate backup files. If the user decides to replace the original with backups, then the vice versa will occur with the array. In both versions, the arrays are cleared beforehand to ensure no data from before is left behind and gets accidently written.
    'NEW FEATURE/SECTION
    Sub Backup()
        Console.ForegroundColor = ConsoleColor.Red
        Console.WriteLine("You have chosen to make a backup of your text files")

        Console.ForegroundColor = ConsoleColor.White
        'Allows user to choose whether to update the backup files or to replace the current files with the backups | e.g. value stored = 2
        Dim backUpChoice As String
        Console.WriteLine("Enter 1 To update the back up files with the current files")
        Console.WriteLine("Enter 2 To replace the current files with the backups - Any other input will return to the menu")
        Console.ForegroundColor = ConsoleColor.Cyan
        backUpChoice = Console.ReadLine()
        Console.ForegroundColor = ConsoleColor.White

        Select Case backUpChoice
            Case 1
                'clears array to make sure no unwanted data is left inside the array
                For x = 0 To 20
                    For y = 0 To 3
                        playerDetailsArray(x, y) = Nothing
                    Next
                Next

                For x = 0 To 20
                    For y = 0 To 1
                        PlayerMatchStatisticsArray(x, y) = Nothing
                    Next
                Next

                For x = 0 To 20
                    For y = 0 To 2
                        PlayerTimesArray(x, y) = Nothing
                    Next
                Next

                SetUpArrays()
                'writes whatever is in the normal files into the backup files using the arrays (e.g. Personal Details.txt file to Personal Details Backup.txt file)
                objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Personal Details Backup.txt")
                For x = 0 To numOfPlayerInSystem
                    For y = 0 To 3
                        writeIt = playerDetailsArray(x, y)
                        objStreamWriter.WriteLine(writeIt)
                    Next
                Next
                objStreamWriter.Close()

                objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Match Stats Backup.txt")
                For x = 0 To numOfPlayerInSystem - 1
                    For y = 0 To 10
                        writeIt = PlayerMatchStatisticsArray(x, y)
                        objStreamWriter.WriteLine(writeIt)
                    Next
                Next
                objStreamWriter.Close()

                objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Player Times Backup.txt")
                For x = 0 To numOfPlayerInSystem - 1
                    For y = 0 To 2
                        writeIt = PlayerTimesArray(x, y)
                        objStreamWriter.WriteLine(writeIt)
                    Next
                Next

                objStreamWriter.Close()
                Console.WriteLine()
                Console.WriteLine("Backups Made")

            Case 2
                'clears array to make sure no unwanted data is left inside the array
                For x = 0 To 20
                    For y = 0 To 3
                        playerDetailsArray(x, y) = Nothing
                    Next
                Next

                For x = 0 To 20
                    For y = 0 To 1
                        PlayerMatchStatisticsArray(x, y) = Nothing
                    Next
                Next

                For x = 0 To 20
                    For y = 0 To 2
                        PlayerTimesArray(x, y) = Nothing
                    Next
                Next

                'sets up player detail array using personal details backup text file so that it can written to the original file
                objStreamReader = New IO.StreamReader("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Personal Details Backup.txt")
                readLine = objStreamReader.ReadLine
                'place number will make sure the row is switched after one player has been read into the array
                placenumber = 0
                'keeps reading the file until there is nothing left
                Do Until readLine = Nothing
                    For y = 0 To 3
                        ' y = 0 - 'sets whatever has been read as the first column for whatever row the loop is on - this is for the place value
                        playerDetailsArray(placenumber, y) = readLine
                        'moves reader onto the next line
                        readLine = objStreamReader.ReadLine
                        ' y = 1 - 'this line sets the name of the player in the array
                        ' y = 2 - player's date of birth
                        ' y = 3   'player's position
                    Next
                    'allows the next player to read into the next row in the array
                    placenumber = placenumber + 1
                Loop
                'closes text file
                objStreamReader.Close()

                'set match statistics array using match stats backup text file using same method as before
                objStreamReader = New IO.StreamReader("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Match Stats Backup.txt")
                readLine = objStreamReader.ReadLine
                placenumber = 0
                Do Until readLine = Nothing
                    For y = 0 To 10
                        'sets whatever has been read as the first column for whatever row the loop is on - this is for the place value
                        PlayerMatchStatisticsArray(placenumber, y) = readLine
                        'moves reader onto the next line
                        readLine = objStreamReader.ReadLine
                        ' y=1 - sets minutes played
                        ' y=2 - sets goals scored
                        ' y=3 - assists
                        ' y=4 - passes
                        ' y=5 - tackles
                        ' y=6 - fouls
                        ' y=7 - saves
                        ' y=8 - goals condceded
                        ' y=9 - yellow cards
                        ' y=10 - red cards
                    Next
                    'moves onto next player to read into the next row in the array
                    placenumber = placenumber + 1
                Loop
                objStreamReader.Close()

                'setting player times with backup text file (same method as before)
                objStreamReader = New IO.StreamReader("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Player Times Backup.txt")
                readLine = objStreamReader.ReadLine
                placenumber = 0
                Do Until readLine = Nothing
                    For y = 0 To 2
                        PlayerTimesArray(placenumber, y) = readLine
                        readLine = objStreamReader.ReadLine
                        ' y = 0 - place number
                        ' y = 1 - training time
                        ' y = 2 - academic time
                    Next
                    placenumber = placenumber + 1
                Loop
                objStreamReader.Close()

                'allows system to continue looping until no player can be read in the array to determine how many players are being stored in the system | True Or False
                Dim checkBlank = False

                'checks how many players have been stored in a system by seeing when the empty space start
                While checkBlank = False
                    If playerDetailsArray(numOfPlayerInSystem, 1) = "" Then
                        checkBlank = True
                    Else
                        numOfPlayerInSystem = numOfPlayerInSystem + 1
                    End If

                End While

                'writes to the original text file using the data from the backup text files which are currently stored in the arrays
                objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Personal Details.txt")
                For x = 0 To numOfPlayerInSystem
                    For y = 0 To 3
                        writeIt = playerDetailsArray(x, y)
                        objStreamWriter.WriteLine(writeIt)
                    Next
                Next
                objStreamWriter.Close()

                objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Match Stats.txt")
                For x = 0 To numOfPlayerInSystem
                    For y = 0 To 10
                        writeIt = PlayerMatchStatisticsArray(x, y)
                        objStreamWriter.WriteLine(writeIt)
                    Next
                Next
                objStreamWriter.Close()

                objStreamWriter = New IO.StreamWriter("C:\Users\Home\Documents\__Nadvi__\A Level\Computing\Coursework Files\Player Times.txt")
                For x = 0 To numOfPlayerInSystem
                    For y = 0 To 2
                        writeIt = PlayerTimesArray(x, y)
                        objStreamWriter.WriteLine(writeIt)
                    Next
                Next
                objStreamWriter.Close()

                Console.WriteLine("The backups have reset the current files")
            Case Else

        End Select
        Console.ReadLine()
        CoachMenu()
    End Sub

    'Subroutine = This allows players who are logged in to view their own stats which fulfils the 7th objective but ensures that players can only access their own data. The system uses the place number for the person who has logged in to output the correct information from each of the arrays and utilises loops output all of the information.
    Sub ViewOwnStats()

        SetUpArrays()
        'outputs all the player personal details based on who logged in
        Console.ForegroundColor = ConsoleColor.DarkGreen
        'NEW FEATURE/SECTION
        For x = 1 To 3
            Console.WriteLine(playerDetailsArray(loggedInPlayer, x))
        Next

        'outputs all of the player's match statistics with the appropriate headings
        Console.WriteLine()
        For x = 1 To 10
            Select Case x
                Case 1
                    Console.WriteLine("Minutes Played")
                Case 2
                    Console.WriteLine("Goals scored")
                Case 3
                    Console.WriteLine("Assists made")
                Case 4
                    Console.WriteLine("Passes made")
                Case 5
                    Console.WriteLine("Tackles made")
                Case 6
                    Console.WriteLine("Fouls made")
                Case 7
                    Console.WriteLine("Saves made ")
                Case 8
                    Console.WriteLine("Goals conceded ")
                Case 9
                    Console.WriteLine("Yellow cards ")
                Case 10
                    Console.WriteLine("Red Cards")
            End Select
            Console.WriteLine(PlayerMatchStatisticsArray(loggedInPlayer, x))
        Next

        'outputs all of the player's training time with the correct headings
        Console.WriteLine()
        For x = 1 To 2
            Select Case x
                Case 1
                    Console.WriteLine("Training Time")
                Case 2
                    Console.WriteLine("Academic Time")
            End Select
            Console.WriteLine(PlayerTimesArray(loggedInPlayer, x))
        Next

        Console.ReadLine()
        PlayerMenu()
    End Sub

End Module
