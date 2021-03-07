using System;
using System.Linq;
using RhythmsGonnaGetYou.Models;
using Microsoft.EntityFrameworkCore;

namespace RhythmsGonnaGetYou
{
    class Program
    {
        static RhythmContext db = new RhythmContext();
        static Band selectedBand = null;
        static void MenuGreeting(string message)
        {
            Console.WriteLine();
            Console.Clear();
            Console.WriteLine($"Welcome to the {message}");
            Console.WriteLine(("").PadRight(55, '-'));
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            var isRunning = true;
            while (isRunning)
            {
                if (selectedBand == null)
                {
                    MenuGreeting("Main Menu.");
                    var selection = MainMenuPrompt("Please make a selection from below:");
                    switch (selection)
                    {
                        case 0:
                            Console.WriteLine("Goodbye.");
                            isRunning = false;
                            break;

                        case 1:
                            AddBand();
                            break;

                        case 2:
                            SelectBand();
                            break;

                        case 3:
                            ViewBands();
                            break;

                        // TODO: Handle 4, 5, and 6

                        default:
                            Console.WriteLine(" Sorry, that is not a valid option.");
                            break;
                    }
                }
                else
                {
                    MenuGreeting($"Band Menu: {selectedBand.Name}");
                    foreach (Album album in selectedBand.Albums)
                    {
                        Console.WriteLine($"{album.Title}");
                    }
                    Console.WriteLine();

                    // TODO: Handle these options:
                    //   AddAlbum();
                    //   AddSong();
                    //   CutBand();
                    //   ResignBand();

                    if (WaitForKeyOrGoBack())
                    {
                        selectedBand = null;
                    }
                }
            }
        }
        static int MainMenuPrompt(string prompt)
        {
            Console.WriteLine(prompt);
            Console.WriteLine("1) Add a Band to the Database");
            Console.WriteLine("2) Select a band by name");
            Console.WriteLine("3) View all Bands in the Database");
            Console.WriteLine("4) View all Albums in the Database (ordered by release date)");
            Console.WriteLine("5) View all Bands that are signed to the Label");
            Console.WriteLine("6) View all Bands that are NOT signed to the Label");
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

            db.Bands.Add(newBand);
            db.SaveChanges();
        }

        static void ViewBands()
        {
            MenuGreeting("Viewing all Bands:");
            foreach (Band band in db.Bands)
            {
                Console.WriteLine($"{band.Name}");
            }
            Console.Write(">");
            Console.ReadKey();
        }

        static void SelectBand()
        {
            MenuGreeting("Search for Band");

            while (selectedBand == null)
            {
                var bandNameQuery = PromptForString("Band name to search for:");

                // set selectBand by querying the DB for the band.
                selectedBand = db.Bands.Include(band => band.Albums).FirstOrDefault(band => band.Name.ToLower().Contains(bandNameQuery));

                if (selectedBand == null)
                {
                    Console.WriteLine($"No band found that matches \"{bandNameQuery}\".");
                    if (WaitForKeyOrGoBack()) break;
                }
            }
        }

        static string PromptForString(string prompt)
        {
            Console.WriteLine();
            Console.WriteLine(prompt);
            Console.Write("> ");
            return Console.ReadLine();
        }

        static bool WaitForKeyOrGoBack()
        {
            Console.WriteLine("Press (b) to go back or any key to continue.");
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.B)
            {
                Console.Clear();
                return true;
            }
            return false;
        }
    }
}