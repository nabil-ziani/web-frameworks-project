using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindopdracht_Bib.Models
{
    public class BookInMemoryRepository : IBookRepository
    {
        private List<Book> books;

        // Lijst boeken aanmaken
        public BookInMemoryRepository()
        {
            this.books = new List<Book>();

            Create(new Book { Id = 1, ISBN = "ISBN025761348", Title = "Lord of the rings", Author = "Tolkien", PublicationYear = 2000, Type = BookType.FICTION });
            Create(new Book { Id = 2, ISBN = "ISBN015496235", Title = "Dune", Author = "Frank Herbert", PublicationYear = 2004, Type = BookType.FICTION });
            Create(new Book { Id = 3, ISBN = "ISBN084562987", Title = "1984", Author = "George Orwell", PublicationYear = 2015, Type = BookType.NONFICTION });
            Create(new Book { Id = 4, ISBN = "ISBN015489632", Title = "Ik ben er niet", Author = "Lize Spit", PublicationYear = 2020, Type = BookType.FICTION });
            Create(new Book { Id = 5, ISBN = "ISBN087456211", Title = "A promised land", Author = "Barack Obama", PublicationYear = 2020, Type = BookType.NONFICTION });
            Create(new Book { Id = 6, ISBN = "ISBN089564213", Title = "Het boek Daniel", Author = "Chris de Stoop", PublicationYear = 2020, Type = BookType.NONFICTION });
            Create(new Book { Id = 7, ISBN = "ISBN058478965", Title = "Most Wanted", Author = "Martin Van Steenbrugge", PublicationYear = 2018, Type = BookType.FICTION });
            Create(new Book { Id = 8, ISBN = "ISBN014895623", Title = "Goed leven", Author = "Dirk De Wachter", PublicationYear = 2002, Type = BookType.FICTION });
            Create(new Book { Id = 9, ISBN = "ISBN054896522", Title = "Fysica", Author = "Lieven Scheire", PublicationYear = 2009, Type = BookType.FICTION });
            Create(new Book { Id = 10, ISBN = "ISBN048756521", Title = "Time machine", Author = "Tanguy Ottomer", PublicationYear = 2014, Type = BookType.NONFICTION });
        }

        public void Create(Book book)
        {
            int max = 0;
            foreach (Book b in books)
            {
                if (b.Id > max)
                {
                    max = b.Id;
                }
            }
            book.Id = max + 1;
            books.Add(book);
        }

        public void Delete(Book book)
        {
            this.books.Remove(book);
        }

        public void Update(Book book)
        {
            // we halen de boek op die wordt geupdate en geven het de nieuwe waarden
            var oldBook = Get(book.Id);

            oldBook.ISBN = book.ISBN;
            oldBook.Title = book.Title;
            oldBook.Author = book.Author;
            oldBook.PublicationYear = book.PublicationYear;
            oldBook.Type = book.Type;
        }

        public Book Get(int id)
        {
            foreach (Book book in books)
            {
                if (book.Id == id)
                {
                    return book;
                }
            }
            return null;
        }

        public IQueryable<Book> GetAll()
        {
            return books.AsQueryable();
        }
    }
}
