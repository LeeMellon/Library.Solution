using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Library.Models.Tests
{
  [TestClass]
  public class PatronTest : IDisposable
 {
    public PatronTest()
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
      int result = Patron.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfDescriptionsAreTheSame_True()
    {

      // Arrange

      Patron firstPatron = new Patron("Peter", "Benchley", "pbenchley@me.com", 1001);

      // Act
      firstPatron.Save();
      List<Patron> authorsList = Patron.GetAll();

      // Assert
      Assert.AreEqual(firstPatron, authorsList[0]);
    }

    [TestMethod]
    public void Getters_TestingAllGetters_Various()
    {
      //Arrange
      string firstName = "Peter";
      string lastName = "Benchley";
      string email = "pbenchley@me.com";
      int cardNumber = 1002;
      Patron newPatron = new Patron(firstName, lastName, email, cardNumber);

      //Act
      string firstNameResult = newPatron.GetFirstName();
      string lastNameResult = newPatron.GetLastName();
      string emailResult = newPatron.GetEmail();
      int cardNumberResult = newPatron.GetCardNumber();

      //Assert
      Assert.AreEqual(firstName, firstNameResult);
      Assert.AreEqual(lastName, lastNameResult);
      Assert.AreEqual(email, emailResult);
      Assert.AreEqual(1002, cardNumberResult);

    }

    [TestMethod]
    public void Setters_TestingAllSetters_Various()
    {

      //Arrange
      string firstName = "Peter";
      string lastName = "Benchley";
      string email = "pbenchley@me.com";
      int cardNumber = 1002;
      string firstName2 = "Stephen";
      string lastName2 = "King";
      string email2 = "sKing@MeAsWell.com";
      int cardNumber2 = 1003;
      Patron newPatron = new Patron(firstName, lastName, email, cardNumber);

      //Act
      newPatron.SetFirstName(firstName2);
      newPatron.SetLastName(lastName2);
      newPatron.SetEmail(email2);
      newPatron.SetCardNumber(cardNumber2);
      string firstNameResult = newPatron.GetFirstName();
      string lastNameResult = newPatron.GetLastName();
      string emailResult = newPatron.GetEmail();
      int cardNumberResult = newPatron.GetCardNumber();

      //Assert
      Assert.AreEqual(firstName2, firstNameResult);
      Assert.AreEqual(lastName2, lastNameResult);
      Assert.AreEqual(email2, emailResult);
      Assert.AreEqual(cardNumber2, cardNumberResult);
    }


    [TestMethod]
    public void Save_SavesToDatabase_PatronList()
    {
      //Arrange
      Patron testPatron = new Patron("Peter", "Benchley", "pbenchley@me.com", 1004);
      testPatron.Save();
      Patron testPatron2 = new Patron("Stephen", "King", "Sking@MeAsWell.com", 1005);
      testPatron2.Save();
      //Act
      List<Patron> result = Patron.GetAll();
      List<Patron> testList = new List<Patron>{testPatron, testPatron2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void DeleteAll_DeletesAllPatronsFromDatabase_Patronlist()
    {
      //Arrange
      Patron testPatron = new Patron("Peter", "Benchley", "pbenchley@me.com", 1006);
      testPatron.Save();
      Patron testPatron2 = new Patron("Stephen", "King", "Sking@MeAsWell.com", 1007);
      testPatron2.Save();

      //Act
      Patron.DeleteAll();
      List<Patron> result = Patron.GetAll();

      //Assert
      Assert.AreEqual(0, result.Count);
    }
  }
}
