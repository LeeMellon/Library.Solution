using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Controllers
{
  public class PatronsController : Controller
  {

    //LIST ALL PATRONS
      [HttpGet("/patrons")]
      public ActionResult Index()
      {
        List<Patron> allPatrons = Patron.GetAll();
        return View(allPatrons);
      }

      //ADD BOOK TO PATRON
      [HttpPost("/patrons/{patronId}/books/new")]
      public ActionResult AddBook(int bookId)
      {
        Patron patron = Patron.Find(patronId);
        Book thisBook = Book.Find(Convert.ToInt32(Request.Form["book-id"]));
        patron.AddBook(thisBook);
        return RedirectToAction(XXXXXXXXXXXXX);
      }

      //DISPLAY PATRON DETAILS
      [HttpGet("/patrons/{id}")]
      public ActionResult Details(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Patron selectedPatron = Patron.Find(id);
        List<Book> patronBooks = selectedPatron.GetBooks();
        List<Book> allBooks = Book.GetAll();
        model.Add("patron", selectedPatron);
        model.Add("patronBooks", patronBooks);
        model.Add("allBooks", allBooks);
        return View(model);
      }

      //CREATE NEW PATRON
      [HttpGet("/patrons/new")]
      public ActionResult CreateForm()
      {
        return View();
      }

      //SAVE NEWLY CREATED PATRON TO DB
      [HttpPost("/patrons/new")]
      public ActionResult Create()
      {
        Patron newPatron = new Patron(Request.Form["patron-name"]);
        newPatron.Save();
        return RedirectToAction("Index");
      }

  }
}
