using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Models.Repositories
{
    public class BookDbRepository : IBookStoreRepository<Book>
    {
        readonly BookstoreDbContext _db;


        public BookDbRepository(BookstoreDbContext db)
        {
            _db = db;
        }


        public IList<Book> List()
        {
            return _db.Books.Include(a=>a.Author).ToList();
        }

        public Book Find(int id)
        {
            var book = _db.Books.Include(a => a.Author).SingleOrDefault(b => b.Id == id);

            return book;
        }

        public void Add(Book entity)
        {
           
            _db.Books.Add(entity);
            _db.SaveChanges();
        }

        public void Update(int id, Book newBook)
        {
            _db.Update(newBook);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = Find(id);

            _db.Books.Remove(book);
            _db.SaveChanges();
        }

        public List<Book> Search(string term)
        {
            var result = _db.Books.Include(a => a.Author)
                .Where(b => b.Title.Contains(term)
                            || b.Description.Contains(term)
                            || b.Author.FullName.Contains(term)).ToList();

            return result;
        }
    }
}
