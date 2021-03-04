using System;
using System.Linq;
using RhythmsGonnaGetYou.Models;

namespace RhythmsGonnaGetYou
{
    class Program
    {

        static void MenuGreeting(string message)
        {
            Console.WriteLine();
            Console.WriteLine($"Welcome to the {message}");
            Console.WriteLine(("").PadRight(55, '-'));
        }
        static void Main(string[] args)
        {
            var db = new RhythmContext();
            var bands = db.Bands;

            var isRunning = true;
            while (isRunning)
            {

                MenuGreeting("Main Menu.");

                var selection = MenuPrompt("Please make a selection from below:");

                switch (selection)
                {
                    case 0:
                        Console.WriteLine("Goodbye.");
                        isRunning = false;
                        break;

                    case 1:
                        AddBand();
                        bands.Add(newBand);
                        break;

                    case 2:
                        ViewBands();
                        break;

                    case 3:
                        AddAlbum();
                        break;

                    case 4:
                        AddSong();
                        break;

                    case 5:
                        CutBand();
                        break;

                    case 6:
                        ResignBand();
                        break;

                    case 7:
                        ViewAlbums();
                        break;

                    case 8:
                        ViewAllAlbums();
                        break;

                    case 9:
                        ViewBandsSigned();
                        break;

                    case 10:
                        ViewCutBands();
                        break;

                    default:
                        Console.WriteLine(" Sorry, that is not a valid option.");
                        break;

                }
            }

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
            Console.WriteLine("Type 0 to Exit the program.");
            string input;
            int value;
            do
            {
                Console.Write(">");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out value));
            return value;
        }

        static void AddBand()
        {
            MenuGreeting("Adding a new Band. Please enter the following information about this new Band:");
            Console.WriteLine("Band's Name:");
            var newBandName = Console.ReadLine();

            Console.WriteLine("Band's country of origin:");
            var newCountry = Console.ReadLine();

            Console.WriteLine("Number of members:");
            var newNumberOfMembers = int.Parse(Console.ReadLine());

            Console.WriteLine("Band's website:");
            var newBandWebsite = Console.ReadLine();

            Console.WriteLine("Band's Genre:");
            var newGenre = Console.ReadLine();

            Console.WriteLine("Is this Band signed to the Label? (true or false)");
            var newSigned = bool.Parse(Console.ReadLine());

            Console.WriteLine("Name of Band's contact:");
            var newContact = Console.ReadLine();

            Console.WriteLine("Band's contact phone number:");
            var newContactNumber = Console.ReadLine();

            var newBand = new Band()
            {
                Name = newBandName,
                CountryOfOrigin = newCountry,
                NumberOfMembers = newNumberOfMembers,
                Website = newBandWebsite,
                Genre = newGenre,
                IsSigned = newSigned,
                ContactName = newContact,
                ContactPhoneNumber = newContactNumber,
            };

        }
    }
}