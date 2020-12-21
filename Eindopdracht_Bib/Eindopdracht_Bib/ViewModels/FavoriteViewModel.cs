﻿using Eindopdracht_Bib.Controllers;
using Eindopdracht_Bib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindopdracht_Bib.ViewModels
{
    public class FavoriteViewModel
    {
        public SortDirection SortDirection { get; set; }
        public SortField SortField { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<Favorite> Favorites { get; set; }
    }
}
