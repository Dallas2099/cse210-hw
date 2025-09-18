using System;

class Program
{
    static void Main(string[] args)
    {
        DisplayWelcome();

        string userName = PromptUserName();
        int favoriteNumber = PromptUserNumber();
        int squaredNumber = SquareNumber(favoriteNumber);
        PromtUserBirthYear(out int birthYear);

        DisplayResult(userName, squaredNumber, birthYear);
    }

    static void DisplayWelcome()
    {
        Console.WriteLine("Welcome to the Program!");
    }

    static string PromptUserName()
    {
        Console.Write("Please enter your name: ");
        return Console.ReadLine() ?? string.Empty;
    }

    static int PromptUserNumber()
    {
        Console.Write("Please enter your favorite number: ");
        string? input = Console.ReadLine();
        return int.Parse(input ?? "0");
    }

    static void PromtUserBirthYear(out int birthYear)
    {
        Console.Write("Please enter the year you were born: ");
        string? input = Console.ReadLine();
        birthYear = int.Parse(input ?? "0");
    }

    static int SquareNumber(int number)
    {
        return number * number;
    }

    static void DisplayResult(string name, int squaredNumber, int birthYear)
    {
        Console.WriteLine($"{name}, the square of your number is {squaredNumber}");

        int currentYear = DateTime.Now.Year;
        int ageThisYear = currentYear - birthYear;
        Console.WriteLine($"{name}, you will turn {ageThisYear} this year.");
    }
}
