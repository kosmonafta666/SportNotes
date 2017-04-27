namespace SportNotes.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class EditNoteModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string Content { get; set; }

        public string Comment { get; set; }
    }
}