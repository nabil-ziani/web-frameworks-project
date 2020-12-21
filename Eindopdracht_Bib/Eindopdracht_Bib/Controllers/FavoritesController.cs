using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eindopdracht_Bib.Models;
using Eindopdracht_Bib.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Eindopdracht_Bib.Controllers
{
    public class FavoritesController : Controller
    {
        public static int PAGE_SIZE = 5;

        private IBookRepository bookRepository;
        public FavoritesController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        // ACTIONS
        public IActionResult Index([FromQuery] SortField sort = SortField.Type, [FromQuery] SortDirection sortDirection = SortDirection.ASC, [FromQuery] int page = 1)
        {
            Dictionary<int, Favorite> favorite = getBookFromSession();
            List<Favorite> favoritesList = favorite.Values.ToList();

            var allFavorites = favoritesList.AsQueryable();

            switch (sort)
            {
                case SortField.ISBN:
                    allFavorites = (sortDirection == SortDirection.ASC) ? allFavorites.OrderBy(b => b.Book.ISBN) : allFavorites.OrderByDescending(b => b.Book.ISBN);
                    break;
                case SortField.Title:
                    allFavorites = (sortDirection == SortDirection.ASC) ? allFavorites.OrderBy(b => b.Book.Title) : allFavorites.OrderByDescending(b => b.Book.Title);
                    break;
                case SortField.Author:
                    allFavorites = (sortDirection == SortDirection.ASC) ? allFavorites.OrderBy(b => b.Book.Author) : allFavorites.OrderByDescending(b => b.Book.Author);
                    break;
                case SortField.PublicationYear:
                    allFavorites = (sortDirection == SortDirection.ASC) ? allFavorites.OrderBy(b => b.Book.PublicationYear) : allFavorites.OrderByDescending(b => b.Book.PublicationYear);
                    break;
                case SortField.Type:
                    allFavorites = (sortDirection == SortDirection.ASC) ? allFavorites.OrderBy(b => b.Book.Type) : allFavorites.OrderByDescending(b => b.Book.Type);
                    break;
            }

            FavoriteViewModel favoriteViewModel = new FavoriteViewModel
            {
                SortDirection = sortDirection,
                SortField = sort,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)allFavorites.Count() / PAGE_SIZE),
                Favorites = allFavorites.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE)
            };

            return View(favoriteViewModel);
        }

        public IActionResult Add(int id)
        {
            Book book = this.bookRepository.Get(id);

            Dictionary<int, Favorite> favorites = getBookFromSession();
            favorites[id] = favorites.GetValueOrDefault(id, new Favorite { Book = book });
            saveBookToFavorites(favorites);
           
            return RedirectToAction("Index", "Favorites", new { id });
        }

        // Methode om een boek in sessie op te slaan.
        private void saveBookToFavorites(Dictionary<int, Favorite> favorite)
        {
            // geen limiet
            HttpContext.Session.SetString("favorite", JsonConvert.SerializeObject(favorite));
        }
        // Methode om een boek uit een sessie te lezen.
        private Dictionary<int, Favorite> getBookFromSession()
        {
            string sessionString = HttpContext.Session.GetString("favorite");
            Dictionary<int, Favorite> favorite = sessionString != null ? JsonConvert.DeserializeObject<Dictionary<int, Favorite>>(HttpContext.Session.GetString("favorite")) : new Dictionary<int, Favorite>();
            return favorite;
        }
    }
}
