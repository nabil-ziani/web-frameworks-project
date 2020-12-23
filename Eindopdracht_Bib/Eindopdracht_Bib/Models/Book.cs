using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eindopdracht_Bib.Models
{
    public enum BookType
    {
        [Display(Name = "Fictie")]FICTION,
        [Display(Name = "Non Fictie")]NONFICTION
    }

    public class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int? PublicationYear { get; set; }
        public BookType Type { get; set; }
        public bool AddedToFavorites { get; set; } = false;
    }
}
