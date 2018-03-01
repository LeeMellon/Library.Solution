using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Controllers
{
  public class AuthorsController : Controller
  {

    //LIST ALL AUTHORS
      [HttpGet("/authors")]
      public ActionResult Index()
      {
        List<Author> allAuthors = Author.GetAll();
        return View(allAuthors);
      }


      //ADD BOOK TO AUTHOR
      [HttpPost("/authors/{authorId}/books/new")]
      public ActionResult AddBook(int authorId)
      {
        Author author = Author.Find(authorId);
        Book thisBook = Book.Find(Convert.ToInt32(Request.Form["book-id"]));
        author.AddBook(thisBook);
        return RedirectToAction("Index");
      }


      //DISPLAY AUTHOR DETAILS
      [HttpGet("/authors/{id}")]
      public ActionResult Details(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Author selectedAuthor = Author.Find(id);
        List<Book> authorBooks = selectedAuthor.GetBooks();
        List<Book> allBooks = Book.GetAll();
        model.Add("author", selectedAuthor);
        model.Add("authorBooks", authorBooks);
        model.Add("allBooks", allBooks);
        return View(model);
      }

      //CREATE NEW AUTHOR
      [HttpGet("/authors/new")]
      public ActionResult CreateForm()
      {
        return View();
      }

      //SAVE NEWLY CREATED AUTHOR TO DB
      [HttpPost("/authors/new")]
      public ActionResult Create()
      {
        Author newAuthor = new Author(Request.Form["new-first-name"], Request.Form["new-last-name"]);
        newAuthor.Save();
        return RedirectToAction("Index");
      }
  }
}
