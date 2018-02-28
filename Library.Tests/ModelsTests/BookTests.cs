using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Library.Models.Tests
{
  [TestClass]
  public class BookTest : IDisposable
 {
    public BookTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    public void Dispose()
    {
      Book.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Book.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfDescriptionsAreTheSame_True()
    {

      // Arrange
      DateTime newDate1 = new DateTime (2018, 2, 28);

      Book firstBook = new Book("Jaws", "FICTION BENCHLEY 1991", "Jaw1", DateTime.Today, newDate1);
      // Book secondBook = new Book("Jaws", "FICTION BENCHLEY 1991", "Jaw1", DateTime.Now, DateTime.Now, 0);

      // Act
      firstBook.Save();
      List<Book> booksList = Book.GetAll();
    
      // Assert
      Assert.AreEqual(firstBook, booksList[0]);
    }

    [TestMethod]
    public void Getters_TestingAllGetters_Various()
    {
      //Arrange
      string title = "Jaws";
      string callNumber = "FICTION BENCHLEY 1991";
      string tagNumber = "Jaw1";
      DateTime checkoutDate = DateTime.Now;
      DateTime dueDate = DateTime.Now;
      Book newBook = new Book(title, callNumber, tagNumber, checkoutDate, dueDate);

      //Act
      string titleResult = newBook.GetTitle();
      string callNumberResult = newBook.GetCallNumber();
      string tagNumberResult = newBook.GetTagNumber();
      DateTime checkoutDateResult = newBook.GetCheckoutDate();
      DateTime dueDateResult = newBook.GetDueDate();

      //Assert
      Assert.AreEqual(title, titleResult);
      Assert.AreEqual(callNumber, callNumberResult);
      Assert.AreEqual(tagNumber, tagNumberResult);
      Assert.AreEqual(checkoutDate, checkoutDateResult);
      Assert.AreEqual(dueDate, dueDateResult);
    }

    [TestMethod]
    public void Setters_TestingAllSetters_Various()
    {

      //Arrange
      string title = "Jaws";
      string title2 = "It";
      string callNumber = "FICTION BENCHLEY 1991";
      string callNumber2 = "FICTION KING";
      string tagNumber = "Jaw1";
      string tagNumber2 = "It1";
      DateTime checkoutDate = DateTime.Now;
      DateTime checkoutDate2 = DateTime.Now;
      DateTime dueDate = DateTime.Now;
      DateTime dueDate2 = DateTime.Now;
      Book newBook = new Book(title, callNumber, tagNumber, checkoutDate, dueDate);

      //Act
      newBook.SetTitle(title2);
      newBook.SetCallNumber(callNumber2);
      newBook.SetTagNumber(tagNumber2);
      newBook.SetCheckoutDate(checkoutDate2);
      newBook.SetDueDate(dueDate2);
      string titleResult = newBook.GetTitle();
      string callNumberResult = newBook.GetCallNumber();
      string tagNumberResult = newBook.GetTagNumber();
      DateTime checkoutDateResult = newBook.GetCheckoutDate();
      DateTime dueDateResult = newBook.GetDueDate();

      //Assert
      Assert.AreEqual(title2, titleResult);
      Assert.AreEqual(callNumber2, callNumberResult);
      Assert.AreEqual(tagNumber2, tagNumberResult);
      Assert.AreEqual(checkoutDate2, checkoutDateResult);
      Assert.AreEqual(dueDate2, dueDateResult);
    }


    [TestMethod]
    public void Save_SavesToDatabase_BookList()
    {
      //Arrange
      DateTime newDate1 = new DateTime (2018, 2, 28);
      DateTime newDate2 = new DateTime (2018, 2, 28);

      Book testBook = new Book("Jaws II", "FICTION BENCHLEY 1991", "Jaw1", newDate2, newDate2);
      testBook.Save();
      Book testBook2 = new Book("It", "FICTION KING", "It1", newDate1, newDate1);
      testBook2.Save();

      //Act
      List<Book> result = Book.GetAll();
      List<Book> testList = new List<Book>{testBook, testBook2};
      System.Console.WriteLine(result[0].GetId());
      System.Console.WriteLine(testList[0].GetId());


      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void DeleteAll_DeletesAllBooksFromDatabase_Booklist()
    {
      //Arrange
      Book testBook = new Book("Jaws", "FICTION BENCHLEY 1991", "Jaw1", DateTime.Now, DateTime.Now);
      testBook.Save();
      Book testBook2 = new Book("It", "FICTION KING", "It1", DateTime.Now, DateTime.Now);
      testBook2.Save();

      //Act
      Book.DeleteAll();
      List<Book> result = Book.GetAll();

      //Assert
      Assert.AreEqual(0, result.Count);
    }
  }
}
