using System;
using System.Collections.Generic;
using System.Text;

namespace WebToKindle.Database.Tables
{
    public class Target
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public Chapter LastChapter { get; set; }
        public string Name { get; set; }
        public string KindleAddress { get; set; }
        public string MailAddress { get; set; }
    }
}
