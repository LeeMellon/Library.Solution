using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Library.Models.Tests
{
  [TestClass]
  public class AuthorTest : IDisposable
 {
    public AuthorTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    public void Dispose()
    {
      Author.DeleteAll();
      Book.DeleteAll();
      Patron.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Author.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfDescriptionsAreTheSame_True()
    {

      // Arrange

      Author firstAuthor = new Author("Peter", "Benchley");

      // Act
      firstAuthor.Save();
      List<Author> authorsList = Author.GetAll();

      // Assert
      Assert.AreEqual(firstAuthor, authorsList[0]);
    }

    [TestMethod]
    public void Getters_TestingAllGetters_Various()
    {
      //Arrange
      string firstName = "Peter";
      string lastName = "Benchley";

      Author newAuthor = new Author(firstName, lastName);

      //Act
      string firstNameResult = newAuthor.GetFirstName();
      string lastNameResult = newAuthor.GetLastName();

      //Assert
      Assert.AreEqual(firstName, firstNameResult);
      Assert.AreEqual(lastName, lastNameResult);

    }

    [TestMethod]
    public void Setters_TestingAllSetters_Various()
    {

      //Arrange
      string firstName = "Peter";
      string lastName = "Benchley";
      string firstName2 = "Stephen";
      string lastName2 = "King";
      Author newAuthor = new Author(firstName, lastName);

      //Act
      newAuthor.SetFirstName(firstName2);
      newAuthor.SetLastName(lastName2);
      string firstNameResult = newAuthor.GetFirstName();
      string lastNameResult = newAuthor.GetLastName();

      //Assert
      Assert.AreEqual(firstName2, firstNameResult);
      Assert.AreEqual(lastName2, lastNameResult);

    }


    [TestMethod]
    public void Save_SavesToDatabase_AuthorList()
    {
      //Arrange
      Author testAuthor = new Author("Peter", "Benchley");
      testAuthor.Save();
      Author testAuthor2 = new Author("Stephen", "King");
      testAuthor2.Save();

      //Act
      List<Author> result = Author.GetAll();
      List<Author> testList = new List<Author>{testAuthor, testAuthor2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void DeleteAll_DeletesAllAuthorsFromDatabase_Authorlist()
    {
      //Arrange
      Author testAuthor = new Author("Peter", "Benchley");
      testAuthor.Save();
      Author testAuthor2 = new Author("Stephen", "King");
      testAuthor2.Save();

      //Act
      Author.DeleteAll();
      List<Author> result = Author.GetAll();

      //Assert
      Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void Delete_DeletesAuthorFromDatabase_Authorlist()
    {
      //Arrange
      Author testAuthor = new Author("Peter", "Benchley");
      testAuthor.Save();
      Author testAuthor2 = new Author("Stephen", "King");
      testAuthor2.Save();

      //Act
      testAuthor.Delete();
      List<Author> result = Author.GetAll();

      //Assert
      Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void Find_FindsAuthorInDatabase_Author()
    {
      //Arrange
      Author testAuthor = new Author("Peter", "Benchley");
      testAuthor.Save();

      //Act
      Author foundAuthor = Author.Find(testAuthor.GetId());

      //Assert
      Assert.AreEqual(testAuthor, foundAuthor);
    }
  }
}
