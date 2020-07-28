using System;
using System.Collections.Generic;
using System.Text;

namespace WebToKindle.Database.Tables
{
    public class MailHistory
    {
        public int Id { get; set; }
        public Target Target { get; set; }
        public Chapter Chapter { get; set; }
        public DateTime MailTime { get; set; }
        public int Attempts { get; set; }
        public int Result { get; set; }
    }
}
