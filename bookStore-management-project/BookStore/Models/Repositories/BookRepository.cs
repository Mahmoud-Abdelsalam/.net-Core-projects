using System.Collections.Generic;
using System.Linq;

namespace BookStore.Models.Repositories
{
    public class BookRepository : IBookStoreRepository<Book>
    {
         List<Book> books;

        public BookRepository()
        {

            books = new List<Book>()
            {
                new Book{Id=1 ,
                    Title = "C# Programming" ,
                    Description = "No Description" ,
                    Author = new Author(),
                    ImageUrl = "CSharp.png"
                },
                new Book{Id=2 , 
                    Title = "Java Programming" ,
                    Description = "Nothing ",
                    Author = new Author(),
                    ImageUrl = "Java.png"
                },
                new Book{Id=3 , 
                    Title = "Python Programming" ,
                    Description = "Fake Description",
                    Author = new Author(),
                    ImageUrl = "Python.jpg"
                }
            };

          

        }

        public IList<Book> List()
        {
            return books;
        }

        public Book Find(int id)    
        {
            var book = books.SingleOrDefault(b => b.Id == id);

            return book;
        }

        public void Add(Book entity)
        {
            entity.Id = books.Max(b => b.Id) + 1;
            books.Add(entity);

        }

        public void Update(int id ,Book newBook)
        {
            var book = Find(id);

            book.Title = newBook.Title;
            book.Author = newBook.Author;
            book.Description = newBook.Description;
            book.ImageUrl = newBook.ImageUrl;
        }

        public void Delete(int id)
        {
            var book = Find(id);

            books.Remove(book);
        }

        public List<Book> Search(string term)
        {
            return books.Where(a => a.Title.Contains(term)).ToList();
        }
    }
}
