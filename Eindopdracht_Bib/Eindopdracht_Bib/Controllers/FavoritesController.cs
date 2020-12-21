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
        private IBookRepository bookRepository;
        public FavoritesController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        // ACTIONS
        public IActionResult Index()
        {
            Dictionary<int, Favorite> favorite = getBookFromSession();
            List<Favorite> favoritesList = favorite.Values.ToList();

            FavoriteViewModel favoriteViewModel = new FavoriteViewModel
            {
                Favorites = favoritesList
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
            if (favorite.Count <= 5)
            {
                HttpContext.Session.SetString("favorite", JsonConvert.SerializeObject(favorite));
            }
            else
            {
                favorite.Remove(favorite.Keys.First());
                HttpContext.Session.SetString("favorite", JsonConvert.SerializeObject(favorite));
            }
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
