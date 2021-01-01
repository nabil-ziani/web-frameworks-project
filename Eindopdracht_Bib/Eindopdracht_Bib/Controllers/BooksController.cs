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

    public enum SortDirection
    {
        DESC,
        ASC
    }
    public enum SortField
    {
        ISBN,
        Title,
        Author,
        PublicationYear,
        Type
    }

    public class BooksController : Controller
    {
        public static int PAGE_SIZE = 5;

        private IBookRepository bookRepository;
        public BooksController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public IActionResult Index([FromQuery] SortField sort = SortField.Type, [FromQuery] SortDirection sortDirection = SortDirection.ASC, [FromQuery] int page = 1, [FromQuery] bool filter = false)
        {
            // lijst van boeken ophalen + "resetten" van favorieten 
            var books = this.bookRepository.GetAll();
            foreach (var book in books)
            {
                book.AddedToFavorites = false; 
            }

            switch (sort)
            {
                case SortField.ISBN:
                    books = (sortDirection == SortDirection.ASC) ? books.OrderBy(b => b.ISBN) : books.OrderByDescending(b => b.ISBN);
                    break;
                case SortField.Title:
                    books = (sortDirection == SortDirection.ASC) ? books.OrderBy(b => b.Title) : books.OrderByDescending(b => b.Title);
                    break;
                case SortField.Author:
                    books = (sortDirection == SortDirection.ASC) ? books.OrderBy(b => b.Author) : books.OrderByDescending(b => b.Author);
                    break;
                case SortField.PublicationYear:
                    books = (sortDirection == SortDirection.ASC) ? books.OrderBy(b => b.PublicationYear) : books.OrderByDescending(b => b.PublicationYear);
                    break;
                case SortField.Type:
                    books = (sortDirection == SortDirection.ASC) ? books.OrderBy(b => b.Type) : books.OrderByDescending(b => b.Type);
                    break;
            }

            // Lijst van favorieten ophalen en afhankelijk daarvan de property "AddedToFavorites" aanpassen
            var favorites = GetBookFromSession();
            foreach (var favorite in favorites)
            {
                foreach (var book in books)
                {
                    if (book.Id == favorite.Value.Id)
                    {
                        book.AddedToFavorites = true;
                    }
                }
            }

            // Filteren van de lijst voor enkel favorieten te tonen
            if (filter != false)
            {
                books = books.Where(b => b.AddedToFavorites == true);
            }

            // ViewModel aanmaken die we kunnen meesturen naar de pagina
            BookListViewModel bookListViewModel = new BookListViewModel
            {
                SortDirection = sortDirection,
                SortField = sort,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)books.Count() / PAGE_SIZE),
                Books = books.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE),
                Filter = filter
            };

            return View(bookListViewModel);
        }

        // Action voor het verwijderen van een boek.
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var book = bookRepository.Get(id);
            if (book != null)
            {
                this.bookRepository.Delete(book);
                TempData["Message"] = $"{book.Title} is verwijderd.";
                return RedirectToAction("Index", "Books");
            }
            return NotFound();
        }

        // Actions voor het aanmaken van een nieuw boek.
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookCreateViewModel bookCreateViewModel) 
        {
            if (ModelState.IsValid)
            {
                Book book = new Book
                {
                    ISBN = bookCreateViewModel.ISBN,
                    Title = bookCreateViewModel.Title,
                    Author = bookCreateViewModel.Author,
                    PublicationYear = bookCreateViewModel.PublicationYear,
                    Type = bookCreateViewModel.Type
                };

                bookRepository.Create(book);

                TempData["Message"] = $"{book.Title} is aangemaakt.";
                return RedirectToAction("Index", "Books");
            }
            else
            {
                return View();
            }
        }

        // Actions voor het updaten van een boek.
        [HttpGet]
        public IActionResult Update(int id)
        {
            var book = this.bookRepository.Get(id);
            if (book != null)
            {
                BookUpdateViewModel bookUpdateViewModel = new BookUpdateViewModel
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Author = book.Author,
                    PublicationYear = book.PublicationYear,
                    Type = book.Type

                };
                return View(bookUpdateViewModel);
            }
            else
            {
                return NotFound();
            }
        }
        
        [HttpPost]
        public IActionResult Update(int id, BookUpdateViewModel bookUpdateViewModel)
        {
            var book = this.bookRepository.Get(id);
            if (book != null)
            {
                if (ModelState.IsValid)
                {
                    book.ISBN = bookUpdateViewModel.ISBN;
                    book.Title = bookUpdateViewModel.Title;
                    book.Author = bookUpdateViewModel.Author;
                    book.PublicationYear = bookUpdateViewModel.PublicationYear;
                    book.Type = bookUpdateViewModel.Type;

                    this.bookRepository.Update(book);
                    TempData["Message"] = $"{book.Title} is aangepast.";
                    return RedirectToAction("Index", "Books");
                }
                else
                {
                    return View(bookUpdateViewModel);
                }
            }
            else
            {
                return NotFound();
            }
        }

        // Actions voor het toevoegen/verwijderen van favorieten.
        public IActionResult Add(int id, [FromQuery] int page = 1, [FromQuery] bool filter = false)
        {
            Book book = this.bookRepository.Get(id);

            Dictionary<int, Book> favorites = GetBookFromSession();
            favorites[id] = favorites.GetValueOrDefault(id, book);
            SaveBookToSession(favorites);

            return RedirectToAction("Index", "Books", new { page, filter });
        }
        public IActionResult Remove(int id, [FromQuery] int page = 1, [FromQuery] bool filter = false)
        {
            // aangeven dat dit boek geen favoriet meer is.
            //Book book = this.bookRepository.Get(id);
            //book.AddedToFavorites = false;

            // favorieten-lijst ophalen, juiste verwijderen en nieuwe lijst opslaan.
            Dictionary<int, Book> favorites = GetBookFromSession();
            
            favorites.Remove(id); 
            
            SaveBookToSession(favorites);

            return RedirectToAction("Index", "Books", new { page, filter });
        }


        // Methode om een boek in sessie op te slaan.
        private void SaveBookToSession(Dictionary<int, Book> favorite)
        {
            // geen limiet
            HttpContext.Session.SetString("favorite", JsonConvert.SerializeObject(favorite));
        }
        // Methode om een boek uit een sessie te lezen.
        private Dictionary<int, Book> GetBookFromSession()
        {
            string sessionString = HttpContext.Session.GetString("favorite");
            Dictionary<int, Book> favorite = sessionString != null ? JsonConvert.DeserializeObject<Dictionary<int, Book>>(HttpContext.Session.GetString("favorite")) : new Dictionary<int, Book>();
            return favorite;
        }
    }
}
