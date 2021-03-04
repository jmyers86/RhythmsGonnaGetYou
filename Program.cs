using System;
using System.Linq;
using RhythmsGonnaGetYou.Models;

namespace RhythmsGonnaGetYou
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new RhythmContext();
            var bands = db.Bands;

            MenuGreeting("Main Menu.");
            MenuPrompt("Please make a selection from below:");

        }
        static void MenuGreeting(string message)
        {
            Console.WriteLine();
            Console.WriteLine($"Welcome to the {message}");
            Console.WriteLine(("").PadRight(55, '-'));
        }
        static int MenuPrompt(string prompt)
        {
            Console.WriteLine();
            Console.WriteLine(prompt);
            Console.WriteLine("1) Add a Band to the Database");
            Console.WriteLine("2) View all Bands in the Database");
            Console.WriteLine("3) Add an Album for a Band in the Database");
            Console.WriteLine("4) Add a Song to an Album in the Database");
            Console.WriteLine("5) Cut a Band in the Database FROM the Label");
            Console.WriteLine("6) Sign a Band in the Database TO the Label");
            Console.WriteLine("7) View all of a specific Band's Albums");
            Console.WriteLine("8) View all Albums in the Database (ordered by release date)");
            Console.WriteLine("9) View all Bands that are signed to the Label");
            Console.WriteLine("10) View all Bands that are NOT signed to the Label");
            Console.WriteLine("Type (Q)uit to Exit the program.");
            string input;
            int value;
            do
            {
                Console.Write(">");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out value));
            return value;
        }
    }
}