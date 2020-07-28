using System;
using System.Collections.Generic;
using System.Text;

namespace WebToKindle.Database.Tables
{

    public enum RegexTypes
    {
        ChapterCount,
        ChapterTitle,
        ChapterContent
    }

    public class RegexType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
