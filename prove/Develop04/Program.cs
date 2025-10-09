using System;
using System.Threading;

// Enhancement: Reflection and listing prompts cycle through all options before repeating to encourage variety.
class Program
{
    static void Main(string[] args)
    {
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("Menu Options:");
            Console.WriteLine("  1. Start breathing activity");
            Console.WriteLine("  2. Start reflection activity");
            Console.WriteLine("  3. Start listing activity");
            Console.WriteLine("  4. Quit");
            Console.Write("Select a choice from the menu: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    new BreathingActivity().Run();
                    break;
                case "2":
                    new ReflectionActivity().Run();
                    break;
                case "3":
                    new ListingActivity().Run();
                    break;
                case "4":
                    running = false;
                    Console.WriteLine();
                    Console.WriteLine("Thanks for taking time to be mindful today!");
                    Thread.Sleep(1500);
                    break;
                default:
                    Console.WriteLine("Please choose a valid option (1-4).");
                    Thread.Sleep(1500);
                    break;
            }
        }
    }
}
