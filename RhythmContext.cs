using Microsoft.EntityFrameworkCore;
using RhythmsGonnaGetYou.Models;

namespace RhythmsGonnaGetYou
{
    class RhythmContext : DbContext
    {
        // Define a movies property that is a DbSet.
        public DbSet<Album> Albums { get; set; }
        public DbSet<Band> Bands { get; set; }
        public DbSet<Song> Songs { get; set; }

        // Define a method required by EF that will configure our connection
        // to the database.
        //
        // DbContextOptionsBuilder is provided to us. We then tell that object
        // we want to connect to a postgres database named suncoast_movies on
        // our local machine.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("server=localhost;database=RhythmsGonnaGetYou");
        }
    }
}