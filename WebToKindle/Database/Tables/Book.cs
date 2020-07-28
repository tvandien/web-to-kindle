using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebToKindle.Database
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IndexURL { get; set; }
        public string ChapterURL { get; set; }
        public int ChapterCount { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
