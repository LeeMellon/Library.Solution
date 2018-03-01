using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Controllers
{
  public class BooksController : Controller
  {

  //LIST ALL BOOKS
    [HttpGet("/books")]
    public ActionResult Index()
    {
      List<Book> allBooks = Book.GetAll();
      return View(allBooks);
    }


    //ADD AUTHOR TO BOOK
    [HttpPost("/books/{bookId}/authors/new")]
    public ActionResult AddAuthor(int authorId)
    {
      Book book = Book.Find(bookId);
      Author thisAuthor = Author.Find(Convert.ToInt32(Request.Form["author-id"]));
      book.AddAuthor(thisAuthor);
      return RedirectToAction(XXXXXXXXXXXX);
    }

    //ADD PATRON TO BOOK
    [HttpPost("/books/{bookId}/patrons/new")]
    public ActionResult AddPatron(int patronId)
    {
      Book book = Book.Find(bookId);
      Patron thisPatron = Patron.Find(Convert.ToInt32(Request.Form["patron-id"]));
      book.AddPatron(thisPatron);
      return RedirectToAction(XXXXXXXXXXXXX);
    }

    //DISPLAY BOOK DETAILS
    [HttpGet("/books/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Book selectedBook = Book.Find(id);
      List<Author> bookAuthors = selectedBook.GetAuthors();
      List<Author> allAuthors = Author.GetAll();
      model.Add("book", selectedBook);
      model.Add("bookAuthors", bookAuthors);
      model.Add("allAuthors", allAuthors);
      return View(model);
    }

    //CREATE NEW BOOK
    [HttpGet("/books/new")]
    public ActionResult CreateForm()
    {
      return View();
    }

    //SAVE NEWLY CREATED BOOK TO DB
    [HttpPost("/books/new")]
    public ActionResult Create()
    {
      Book newBook = new Book(Request.Form["book-name"]);
      newBook.Save();
      return RedirectToAction("Index");
    }
  }
}
