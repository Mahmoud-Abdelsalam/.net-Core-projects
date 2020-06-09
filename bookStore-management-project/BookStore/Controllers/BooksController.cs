using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Repositories;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookStoreRepository<Book> _bookRepository;
        private readonly IBookStoreRepository<Author> _authorRepository;
        private readonly IHostingEnvironment _hosting;

        public BooksController(IBookStoreRepository<Book> bookRepository,IBookStoreRepository<Author> authorRepository,IHostingEnvironment hosting )
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _hosting = hosting;
        }
        // GET: Books
        public ActionResult Index()
        {
            var books = _bookRepository.List();
            return View(books);
        }

        // GET: Books/Details/5
        public ActionResult Details(int id)
        {
            var books = _bookRepository.Find(id);
            return View(books);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel{

                Authors = FillSelectList()
            };
            return View(model);
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                try
                {
                    var fileName = UploadFile(model.File)?? string.Empty ; 
                   
                    if (model.AuthorId == -1)
                    {
                        ViewBag.Message = "Please select an author from the list";
                       
                        return View(GetAllAuthors());
                    }
                    var author = _authorRepository.Find(model.AuthorId);
                    var book = new Book
                    { 
                        Id = model.BookId,
                        Title = model.Title,
                        Description = model.Description,
                        Author = author,
                        ImageUrl = fileName
                    };
                    _bookRepository.Add(book);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exp)
                {
                    return View(exp.Message);
                }

            }
           
            ModelState.AddModelError("","You have to fill all the required fields correctely");
            return View(GetAllAuthors());

        }

        // GET: Books/Edit/5
        public ActionResult Edit(int id)
        {
            var book = _bookRepository.Find(id);
            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id;
            var ViewModel = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = authorId,
                Authors = FillSelectList(),
                ImageUrl = book.ImageUrl
            };
            return View(ViewModel);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( BookAuthorViewModel ViewModel)
        {
            try
            {

                string fileName = UploadFile(ViewModel.File, ViewModel.ImageUrl);
               

                if (ViewModel.AuthorId == -1)
                {
                    ViewBag.Message = "Please select an author from the list";
                    var vmodel = new BookAuthorViewModel
                    {
                        Authors = FillSelectList()

                    };
                    return View(vmodel);
                }


                var author = _authorRepository.Find(ViewModel.AuthorId);
                var book = new Book
                {
                    Id=ViewModel.BookId,
                    Title = ViewModel.Title,
                    Description = ViewModel.Description,
                    Author = author,
                    ImageUrl = fileName
                };
                _bookRepository.Update(ViewModel.BookId,book);
               
                return RedirectToAction(nameof(Index));
                
            }
            catch(Exception exp)
            {
                return View(exp.Message);
            }
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int id)
        {
            var book = _bookRepository.Find(id);
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id , IFormCollection collection)
        {
            try
            {
               _bookRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<Author> FillSelectList()
        {
            var authors = _authorRepository.List().ToList();
            authors.Insert(0,new Author{Id = -1 ,FullName = "---Please select an author---"});

            return authors;
        }

        BookAuthorViewModel GetAllAuthors()
        {
            var vmodel = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return vmodel;  
        }

        string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                var uploads = Path.Combine(_hosting.WebRootPath, "uploads");
                
                var fullPath = Path.Combine(uploads, Path.GetFileName(file.FileName));
                file.CopyTo(new FileStream(fullPath, FileMode.Create));
                return Path.GetFileName(file.FileName);
            }

            return null;
        }

        string UploadFile(IFormFile file ,string imageUrl)
        {
            if (file != null)
            {
                var uploads = Path.Combine(_hosting.WebRootPath, "uploads");
                
                var newPath = Path.Combine(uploads, Path.GetFileName(file.FileName));
                //Delete the old file
               
                string oldPath = Path.Combine(uploads, imageUrl);


                if (oldPath != newPath)
                {
                    System.IO.File.Delete(oldPath);
                    //Save the new file
                    file.CopyTo(new FileStream(newPath, FileMode.Create));

                }
                else if (oldPath == null)
                {
                    file.CopyTo(new FileStream(newPath, FileMode.Create));
                }

                return Path.GetFileName(file.FileName);

            }

            return imageUrl;
        }

        public ActionResult Search(string term)
        {
            var result = _bookRepository.Search(term);

            return View("Index", result);
        }
    } 

}