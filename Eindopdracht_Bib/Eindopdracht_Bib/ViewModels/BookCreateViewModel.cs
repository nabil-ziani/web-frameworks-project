using Eindopdracht_Bib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eindopdracht_Bib.ViewModels
{
    public class BookCreateViewModel
    {
        [Required(ErrorMessage = "Je moet verplicht een ISBN opgeven.")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Je moet verplicht een titel opgeven.")]
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Je moet verplicht een auteur opgeven.")]
        [Display(Name = "Auteur")]
        public string Author { get; set; }

        [Range(1900, 2021, ErrorMessage = "Het jaar moet tussen 1900 en 2021 liggen")]
        [Display(Name = "Publicatie Jaar")]
        [Required(ErrorMessage = "Je moet verplicht een publicatiejaar opgeven.")]
        public int PublicationYear { get; set; }

        public BookType Type { get; set; }
    }
}
