using System;
using System.Collections.Generic;
using System.Text;

namespace WebToKindle.Database.Tables
{
    public class BookTemplate
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public string Header { get; set; }
        public string Chapter { get; set; }
        public string Footer { get; set; }
    }
}
