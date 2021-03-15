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

                selectedBand = db.Bands.Include(band => band.Albums).FirstOrDefault(band => band.Name.ToLower().Contains(bandNameQuery.ToLower()));

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
            DateTime newReleaseDate = DateTime.Today;
            try
            {
                newReleaseDate = DateTime.Parse(Console.ReadLine());
            }
            catch (System.FormatException)
            {
                Console.WriteLine("Not a valid date format. Returning to Band Menu.");
                return;
            };

            var newAlbum = new Album()
            {
                Title = newAlbumTitle,
                IsExplicit = newIsExplicit,
                ReleaseDate = newReleaseDate,
            };

            selectedBand.Albums.Add(newAlbum);
            db.SaveChanges();
        }

        static void AddSong()
        {
            MenuGreeting("Please choose the Album you would like to add a song to:");
            Album selectedAlbum = null;

            while (selectedAlbum == null)
            {
                var albumQuery = PromptForString("Please choose the Album you would like to add a song to:");
                selectedAlbum = selectedBand.Albums.FirstOrDefault(album => album.Title.ToLower().Contains(albumQuery.ToLower()));
            }
            Console.WriteLine("What is the title of this Song?");
            var newTitle = Console.ReadLine();

            Console.WriteLine("Which track number is this Song?");
            var newTrackNumber = int.Parse(Console.ReadLine());

            Console.WriteLine("How long (in seconds) is this track?");
            var newDuration = int.Parse(Console.ReadLine());

            var newSong = new Song()
            {
                Album = selectedAlbum,   // This saves 'selectedAlbum' to the album ID
                Title = newTitle,
                TrackNumber = newTrackNumber,
                Duration = newDuration,
            };


            db.Songs.Add(newSong);
            db.SaveChanges();


        }

        static void CutBand()
        {
            if (selectedBand.IsSigned == true)
            {
                selectedBand.IsSigned = false;
                Console.WriteLine($"{selectedBand.Name} was let go from the Label!");
                db.SaveChanges();

            }
            else Console.WriteLine($"{selectedBand.Name} is not signed to the Label!");
        }

        static void ResignBand()
        {
            if (selectedBand.IsSigned == false)
            {
                selectedBand.IsSigned = true;
                Console.WriteLine($"{selectedBand.Name} was signed to the Label!");
                db.SaveChanges();

            }
            else Console.WriteLine($"{selectedBand.Name} is already signed to the Label!");
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