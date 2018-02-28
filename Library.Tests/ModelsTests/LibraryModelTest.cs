using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System;

namespace Library.Models.Tests
{
  [TestClass]
  public class LibraryModelTest : IDisposable
 {
    public LibraryModelTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    public void Dispose()
    {
      //Delete everything from the database
    }

    [TestMethod]
    public void Test_JustATest_String()
    {
      Assert.AreEqual("this is a string from the model", LibraryModel.GetString());
    }
  }
}
