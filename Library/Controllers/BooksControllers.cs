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

    [HttpGet("/books/{bookId}/authors/new")]
    public ActionResult AddAuthorForm(int bookId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Book selectedBook = Book.Find(bookId);
      List<Author> bookAuthors = selectedBook.GetAuthors();
      List<Author> allAuthors = Author.GetAll();
      model.Add("book", selectedBook);
      model.Add("bookAuthors", bookAuthors);
      model.Add("allAuthors", allAuthors);
      return View(model);
    }

    //ADD AUTHOR TO BOOK
    [HttpPost("/books/{bookId}/authors/new")]
    public ActionResult AddAuthor(int bookId)
    {
      Book book = Book.Find(bookId);
      Author thisAuthor = Author.Find(Convert.ToInt32(Request.Form["author-id"]));
      book.AddAuthor(thisAuthor);
      return RedirectToAction("Index");
    }

    //ADD PATRON TO BOOK
    [HttpPost("/books/{bookId}/patrons/new")]
    public ActionResult AddPatron(int bookId)
    {
      Book book = Book.Find(bookId);
      Patron thisPatron = Patron.Find(Convert.ToInt32(Request.Form["patron-id"]));
      book.AddPatron(thisPatron);
      return RedirectToAction("Index");
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
      Book newBook = new Book(Request.Form["new-title"], Request.Form["new-call-number"], Request.Form["new-tag-number"], Convert.ToDateTime(Request.Form["new-checkout-date"]), Convert.ToDateTime(Request.Form["new-duedate"]), Request.Form["new-status"]);
      newBook.Save();
      return RedirectToAction("Index");
    }

    //DELETE BOOK FROM DB
    [HttpGet("/books/{id}/delete")]
    public ActionResult Delete(int id)
    {
      Book thisBook = Book.Find(id);
      thisBook.Delete();
      List<Book> allBooks = Book.GetAll();
      return View("Index", allBooks);
    }

    //EDIT BOOK
    [HttpGet("/books/{id}/update")]
    public ActionResult UpdateForm(int id)
    {
      Book thisBook = Book.Find(id);
      List<Author> allAuthors = Author.GetAll();
      Dictionary<string, object> bookDetails = new Dictionary <string, object>();
      bookDetails.Add("book", thisBook);
      bookDetails.Add("authors", allAuthors);

      return View(bookDetails);
    }

    [HttpPost("/books/{id}/update")]
    public ActionResult Update(int id)
    {
      Book thisBook = Book.Find(id);
      string newTitle = Request.Form["new-title"];
      string newCallNumber = Request.Form["new-call-number"];
      string newTagNumber = Request.Form["new-tag-number"];
      DateTime newCheckoutDate = Convert.ToDateTime(Request.Form["new-checkout-date"]);
      DateTime newDuedate= Convert.ToDateTime(Request.Form["new-duedate"]);
      string newStatus = Request.Form["new-status"];

      return RedirectToAction("Index");
    }

  }
}
