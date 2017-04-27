namespace SportNotes.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class UserNote
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Summary { get; set; }
    }
}