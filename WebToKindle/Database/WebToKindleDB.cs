using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebToKindle.Database.Tables;

namespace WebToKindle.Database
{
    public class WebToKindleDB : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BookTemplate> BookTemplates { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<MailHistory> MailHistory { get; set; }
        public DbSet<Regex> Regexes { get; set; }
        public DbSet<RegexType> RegexTypes {get; set;}
        public DbSet<Target> Targets { get; set; }

        public WebToKindleDB(DbContextOptions<WebToKindleDB> options)
            : base(options)
        {
        }

    }
}
