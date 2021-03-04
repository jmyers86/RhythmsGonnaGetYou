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

            var band = new Band()
            {
                Name = "The Cure",
                CountryOfOrigin = "UK",
                NumberOfMembers = 5,
                Website = "https://thecure.com",
                Genre = "Post-Punk",
                IsSigned = true,
                ContactName = "Robert Smith",
                ContactPhoneNumber = "+114230984985702"
            };

            bands.Add(band);
            db.SaveChanges();

            Console.WriteLine($"There are {bands.Count()} bands!");
        }
    }
}