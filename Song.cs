using System;

namespace RhythmsGonnaGetYou.Models
{
    class Song
    {
        public int Id { get; set; }
        public Album Album { get; set; }
        public string Title { get; set; }
        public int TrackNumber { get; set; }
        public int Duration { get; set; }
    }
}