using System;

class Program
{
    static void Main(string[] args)
    {
        Random randomGenerator = new Random();
        bool playAgain = true;

        while (playAgain)
        {
            int magicNumber = randomGenerator.Next(1, 101); // 1 through 100 inclusive
            int guessCount = 0;
            int guess = int.MinValue;

            Console.WriteLine("I'm thinking of a number between 1 and 100.");

            while (guess != magicNumber)
            {
                Console.Write("What is your guess? ");
                string input = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(input, out guess))
                {
                    Console.WriteLine("Please enter a valid whole number.");
                    continue;
                }

                guessCount++;

                if (guess < magicNumber)
                {
                    Console.WriteLine("Higher");
                }
                else if (guess > magicNumber)
                {
                    Console.WriteLine("Lower");
                }
                else
                {
                    Console.WriteLine("You guessed it!");
                    Console.WriteLine($"It took you {guessCount} guesses.");
                }
            }

            Console.Write("Play again? (yes/no) ");
            string replayResponse = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
            playAgain = replayResponse == "yes" || replayResponse == "y";
            Console.WriteLine();
        }
    }
}
