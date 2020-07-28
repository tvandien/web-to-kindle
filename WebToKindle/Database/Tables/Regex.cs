using System;
using System.Collections.Generic;
using System.Text;

namespace WebToKindle.Database.Tables
{
    public class Regex
    {
        public int Id { get; set; }
        public RegexType Type { get; set; }
        public Book Book { get; set; }
        public string RegexString { get; set; }

    }
}
