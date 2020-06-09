using System.Collections.Generic;
using System.Linq;

namespace BookStore.Models.Repositories
{
    public class AuthorDbRepository : IBookStoreRepository<Author>
    {
       private readonly BookstoreDbContext _db;


        public AuthorDbRepository(BookstoreDbContext db)
        {
            this._db = db;
        }


        public IList<Author> List()
        {
            return _db.Authors.ToList();
        }

        public Author Find(int id)
        {
            var author = _db.Authors.SingleOrDefault(a => a.Id == id);
            return author;
        }

        public void Add(Author entity)
        {
            
            _db.Authors.Add(entity);
            _db.SaveChanges();
        }

        public void Update(int id, Author newAuthor)
        {
            _db.Update(newAuthor);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var author = Find(id);

            _db.Authors.Remove(author);
            _db.SaveChanges();
        }

        public List<Author> Search(string term)
        {
            return _db.Authors.Where(a => a.FullName.Contains(term)).ToList();
        }
    }
   
}
