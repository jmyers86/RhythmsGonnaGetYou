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

                        case 4:
                            ViewAllAlbums();
                            break;

                        case 5:
                            ViewSignedBands();
                            break;

                        case 6:
                            ViewNotSignedBands();
                            break;


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

                    var selection = BandMenuPrompt("Please make a selection from below:");
                    switch (selection)
                    {
                        case 0:
                            selectedBand = null;
                            break;
                        // I cut the WaitForKeyOrGoBack because I couldn't figure out how to fit it into this switch statement.
                        // It ended up asking you to "press 0" and then running the WaitFor... method which felt redundant.

                        case 1:
                            AddAlbum();
                            break;

                        case 2:
                            AddSong();
                            break;

                        case 3:
                            CutBand();
                            break;

                        case 4:
                            ResignBand();
                            break;

                        default:
                            Console.WriteLine(" Sorry, that is not a valid option.");
                            break;
                    }

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
        static void SelectBand()
        {
            MenuGreeting("Search for Band");

            while (selectedBand == null)
            {
                var bandNameQuery = PromptForString("Band name to search for:");

                // The way this is written only allows fully lower-case queries.
                selectedBand = db.Bands.Include(band => band.Albums).FirstOrDefault(band => band.Name.ToLower().Contains(bandNameQuery));

                if (selectedBand == null)
                {
                    Console.WriteLine($"No band found that matches \"{bandNameQuery}\".");
                    if (WaitForKeyOrGoBack()) break;
                }
            }
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



        static void ViewAllAlbums()
        {
            MenuGreeting("Viewing all Albums:");
            // var orderedAlbums = db.Albums.OrderBy(Album => Album.ReleaseDate);
            // Console.WriteLine($"{orderedAlbums}");   <---- I don't know why this doesn't work.
            foreach (Album album in db.Albums.OrderBy(album => album.ReleaseDate))
            {
                Console.WriteLine($"{album.Title} , {album.ReleaseDate}");
            }
            Console.Write(">");
            Console.ReadKey();
        }

        static void ViewSignedBands()
        {
            MenuGreeting("Viewing all Signed Bands:");
            foreach (Band band in db.Bands.Where(band => band.IsSigned == true))
            {
                Console.WriteLine($"{band.Name}");
            }
            Console.Write(">");
            Console.ReadKey();
        }

        static void ViewNotSignedBands()
        {
            MenuGreeting("Viewing all Signed Bands:");
            foreach (Band band in db.Bands.Where(band => band.IsSigned == false))
            {
                Console.WriteLine($"{band.Name}");
            }
            Console.Write(">");
            Console.ReadKey();
        }


        static int BandMenuPrompt(string prompt)
        {
            Console.WriteLine(prompt);
            Console.WriteLine("1) Add an Album for this Band");
            Console.WriteLine("2) Add a Song to one of this Band's Albums");
            Console.WriteLine("3) Cut this Band from the Label");
            Console.WriteLine("4) Sign this Band to the Label");
            Console.WriteLine();
            Console.WriteLine(" Type '0' to exit Band Menu");
            string input;
            int value;
            do
            {
                Console.Write(">");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out value));
            return value;
        }

        static void AddAlbum()
        {
            MenuGreeting("Adding a new Album. Please enter the following information about this new Album:");
            Console.WriteLine("Album's title:");
            var newAlbumTitle = Console.ReadLine();

            Console.WriteLine("Is this Album explicit? (true or false)");
            var newIsExplicit = bool.Parse(Console.ReadLine());

            Console.WriteLine("Album's release date (DD/MM/YYYY):");
            var newReleaseDate = DateTime.Parse(Console.ReadLine());   //This input is too particular and crashes the program if the format is not correct. I tried to implement a try/catch
                                                                       // or tryparse but I couldn't make it work.
            var newAlbum = new Album()
            {
                Title = newAlbumTitle,
                IsExplicit = newIsExplicit,
                ReleaseDate = newReleaseDate,
            };

            selectedBand.Albums.Add(newAlbum);
            db.SaveChanges();
        }

        static void AddSong()  // This one is very difficult for me and I had to do a PEDAC for it. Still not getting it.
        {
            MenuGreeting("Please choose the Album you would like to add a song to:");


        }

        static void CutBand()   // This does nothing. It should at least print something. 
        {
            if (selectedBand.IsSigned == true)
            {
                Console.WriteLine($"{selectedBand} was let go from the Label!");

            }
            else Console.WriteLine($"{selectedBand} is not signed to the Label!");
        }

        static void ResignBand()
        {

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