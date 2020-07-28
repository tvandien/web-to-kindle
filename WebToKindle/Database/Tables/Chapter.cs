using System;
using System.Collections.Generic;
using System.Text;

namespace WebToKindle.Database.Tables
{
    public class Chapter
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public int ChapterNumber { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Version { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
