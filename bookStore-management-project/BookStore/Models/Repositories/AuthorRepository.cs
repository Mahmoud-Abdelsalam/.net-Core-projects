using System.Collections.Generic;
using System.Linq;

namespace BookStore.Models.Repositories
{
    public class AuthorRepository : IBookStoreRepository<Author>
    {
        IList<Author> authors;

        public AuthorRepository()
        {
            authors = new List<Author>()
            {
                new Author{Id=1 , FullName = "name1"},
                new Author{Id=2 , FullName = "name2"},
                new Author{Id=3 , FullName = "name3"}

            };
        }
        public IList<Author> List()
        {
            return authors;
        }

        public Author Find(int id)
        {
            var author = authors.SingleOrDefault(a => a.Id == id);
            return author;
        }

        public void Add(Author entity)
        {
            entity.Id = authors.Max(a => a.Id) + 1; 
            authors.Add(entity);
        }

        public void Update(int id, Author newAuthor)
        {
            var author = Find(id);

            
            author.FullName = newAuthor.FullName;
        }

        public void Delete(int id)
        {
            var author = Find(id);

            authors.Remove(author);
        }

        public List<Author> Search(string term)
        {
            return authors.Where(a => a.FullName.Contains(term)).ToList();
        }
    }
}
