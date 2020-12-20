using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindopdracht_Bib.Models
{
    public interface IBookRepository
    {
        IQueryable<Book> GetAll();
        Book Get(int id);

        void Create(Book book);
        void Delete(Book book);
        void Update(Book book);
    }
}
