using System.Diagnostics;

Stopwatch timer = new Stopwatch();

int menuItems = 6;

string? readResult = null;

bool keepPlaying = false;

Random random = new Random();

List<string> gameHistory = new List<string>();
List<double> timeHistory = new List<double>();
List<int> pointsHistory = new List<int>(); 
List<int> questionsPerGame = new List<int>(); 

do
{
    int points = 0;
    int range = 0;
    int questions = 5;
    int selection = 0;
    int difficulty = 0;

    bool validInputs1 = false;
    bool validInputs2 = false;
    bool randomGame = false;

    do
    {
        Console.WriteLine("Choose a minigame (input number):");
        Console.WriteLine("1 - Addition");
        Console.WriteLine("2 - Subtraction");
        Console.WriteLine("3 - Multiplication");
        Console.WriteLine("4 - Division");
        Console.WriteLine("5 - History");
        Console.WriteLine("6 - Randomized operators game");
        Console.WriteLine();

        readResult = Console.ReadLine();

        if (readResult != null && int.TryParse(readResult.Trim(), out selection) && selection > 0 && selection <= menuItems)
        {
            Console.WriteLine($"You selected {selection}");
            validInputs1 = true;
        }
        else
        {
            Console.WriteLine("Invalid Input during menu selection.\n");
            validInputs1 = false;
            continue;
        }
        if (selection <= 4 || selection == 6)
        {
            Console.WriteLine("\nChoose a level of difficulty from 1 to 3:");
            readResult = Console.ReadLine();
            if (readResult != null && int.TryParse(readResult.Trim(), out difficulty) && difficulty > 0 && difficulty < 4)
            {
                Console.WriteLine($"You selected diffuclty {difficulty}");
                (range, questions) = rangeSetting(range, difficulty, questions);
                validInputs2 = true;
            }
            else
            {
                Console.WriteLine("Invalid Input during difficulty selection.\n");
                validInputs2 = false;
            }
        }
        else
        {
            validInputs2 = true;
        }
    } while (validInputs1 != true || validInputs2 != true);

    if (selection <= 4 || selection == 6)
    {
        timer.Start();
        if (selection == 6)
            randomGame = true;
        points = mathGame(points, selection, difficulty, questions, range, randomGame);
        timer.Stop();
        Console.WriteLine($"You took {timer.Elapsed.TotalSeconds:F} seconds");

        timeHistory.Add(timer.Elapsed.TotalSeconds);
        timer.Reset(); 
        pointsHistory.Add(points);
        questionsPerGame.Add(questions);
    }

    else if (selection == 5)
    {
        if (gameHistory.Count == 0)
        {
            Console.WriteLine("No games played yet!");
        }
        else
        {
            Console.WriteLine("Here is the history of the calculations you made: \n");
            int counter = 0;
            int fullGameData = 0;
            int questionsInThisGame = questionsPerGame[fullGameData];
            foreach (string calculation in gameHistory)
            {
                counter++;
                Console.WriteLine(calculation);
                if (counter == questionsInThisGame)
                {
                    Console.WriteLine($"You did {pointsHistory[fullGameData]} points out of {questionsInThisGame}");
                    Console.WriteLine($"And took: {timeHistory[fullGameData]:F} seconds");
                    Console.WriteLine("");
                    fullGameData++;
                    counter = 0;
                    if (fullGameData < questionsPerGame.Count)
                        questionsInThisGame = questionsPerGame[fullGameData];
                }
            }
        }
    }

    Console.WriteLine("wanna do something else? Press anything to keep playing, write \"EXIT\" to exit");
    readResult = Console.ReadLine();

    if (readResult != null && readResult.ToLower().Trim() == "exit")
    {
        keepPlaying = false;
    }
    else
    {
        keepPlaying = true;
    }

} while (keepPlaying == true);

Console.WriteLine("Thanks for playing!");

int mathGame(int points, int selection, int difficulty, int questions, int range, bool randomGame)
{
    string operation = "";
    int totalQuestions = questions;
    do
    {
        if (randomGame == true)
        {
            selection = random.Next(1, 5);
        }
        bool rightAnswer = false;

        switch (selection)
        {
            case 1:
                operation = "+";
                break;
            case 2:
                operation = "-";
                break;
            case 3:
                operation = "*";
                break;
            case 4:
                operation = "/";
                break;

        }


        rightAnswer = Calculation(range, operation);

        if (rightAnswer)
            points++;


        Console.WriteLine($"You made {points} points out of {totalQuestions} \n");
        questions--;

    } while (questions > 0);
    return points;
}

bool Calculation(int range, string operation)
{
    int n1 = random.Next(0, range);
    int n2 = random.Next(0, range);
    int givenResult = 0;

    if (operation == "-")
    {
        if (n2 > n1)
        {
            int temp = n1;
            n1 = n2;
            n2 = temp;
        }
    }
    else if (operation == "/")
    {
        n2 = random.Next(1, 15);
        n1 = n2 * random.Next(1, range / n2);
    }

    int result = operation switch
    {
        "+" => n1 + n2,
        "-" => n1 - n2,
        "*" => n1 * n2,
        "/" => n1 / n2,
        _ => 0
    };

    Console.WriteLine(n1 + operation + n2);
    readResult = Console.ReadLine();

    if (readResult != null && int.TryParse(readResult, out givenResult) && givenResult == result)
    {
        Console.WriteLine("You gave the rigt answer!");
        gameHistory.Add($"{n1} {operation} {n2} = {result} - result given: {givenResult} - right");
        return true;
    }
    else if (readResult != null && int.TryParse(readResult, out givenResult) && givenResult != result)
    {
        Console.WriteLine("You gave a wrong answer!");

        gameHistory.Add($"{n1} {operation} {n2} = {result} - result given: {givenResult} - wrong");
        return false;

    }
    else
    {
        Console.WriteLine("You didn't even give a number!");

        gameHistory.Add($"{n1} {operation} {n2} = {result} - result given: // - wrong");
        return false;
    }
}

(int, int) rangeSetting(int range, int difficulty, int questions)
{
    (range, questions) = difficulty switch
    {
        1 => (21, 5),
        2 => (51, 7),
        3 => (101, 10),
        _ => (21, 5)
    };

    return (range, questions);
}