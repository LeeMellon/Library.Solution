using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Author
  {
    private int _id;
    private string _firstName;
    private string _lastName;


    public Author(string firstName, string lastName, int id=0)
    {
      _firstName = firstName;
      _lastName = lastName;
      _id = id;
    }

    public string GetFirstName()
    {
      return _firstName;
    }

    public void SetFirstName(string firstName)
    {
      _firstName = firstName;
    }

    public string GetLastName()
    {
      return _lastName;
    }

    public void SetLastName(string lastName)
    {
      _lastName = lastName;
    }

    public int GetId()
    {
      return _id;
    }

    public override bool Equals(System.Object otherAuthor)
    {
      if (!(otherAuthor is Author))
      {
        return false;
      }
      else
      {
        Author newAuthor = (Author) otherAuthor;
        bool idEquality = (this.GetId() == newAuthor.GetId());
        bool firstNameEquality = (this.GetFirstName() == newAuthor.GetFirstName());
        bool lastNameEquality = (this.GetLastName() == newAuthor.GetLastName());
        // return _id == newBook._id && _title == newBook._title && _callNumber == newBook._callNumber && _tagNumber == newBook._tagNumber && _checkoutDate == newBook._checkoutDate && _dueDate == newBook._dueDate && _status == newBook._status;
        return (idEquality && firstNameEquality && lastNameEquality);
      }
    }

    public override int GetHashCode()
    {
      return _id.GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"INSERT INTO authors (first_name, last_name) VALUES (@firstName, @lastName);";
      cmd.Parameters.Add(new MySqlParameter("@firstName", _firstName));
      cmd.Parameters.Add(new MySqlParameter("@lastName", _lastName));
      cmd.ExecuteNonQuery();

      _id = (int)cmd.LastInsertedId;

      conn.Close();
      if(conn != null)
        conn.Dispose();

    }

    public static List<Author> GetAll()
    {
      List<Author> allAuthors = new List<Author>();
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT * FROM authors";
      MySqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorFirstName= rdr.GetString(1);
        string authorLastName = rdr.GetString(2);

        Author newAuthor = new Author(authorFirstName, authorLastName, authorId);
        allAuthors.Add(newAuthor);
      }

      conn.Close();
      if (conn != null)
        conn.Dispose();
      return allAuthors;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"TRUNCATE TABLE authors; TRUNCATE TABLE authors_books;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public void AddBook(Book author)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"INSERT INTO authors_books (author_id, book_id) VALUES (@AuthorId, @BookId)";
      cmd.Parameters.Add(new MySqlParameter("@AuthorId", author.GetId()));
      cmd.Parameters.Add(new MySqlParameter("@BookId", _id));
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public List<Book> GetBooks()
    {
      List<Book> books = new List<Book>();

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"
        SELECT books.* FROM authors
        JOIN authors_books ON (authors.id = authors_books.author_id)
        JOIN books ON (authors_books.book_id = books.id)
        WHERE authors.id = @ThisId;";
      cmd.Parameters.Add(new MySqlParameter("@ThisId", _id));
      MySqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookTitle = rdr.GetString(1);
        string bookCallNumber= rdr.GetString(2);
        string bookTagNumber = rdr.GetString(3);
        DateTime bookCheckoutDate = rdr.GetDateTime(4);
        DateTime bookDuedate = rdr.GetDateTime(5);
        string bookStatus = rdr.GetString(6);
        Book newBook = new Book(bookTitle, bookCallNumber, bookTagNumber, bookCheckoutDate, bookDuedate, bookStatus, bookId);
        books.Add(newBook);
      }

      conn.Close();
      if (conn != null)
        conn.Dispose();

      return books;

    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM authors WHERE id = @thisId; DELETE from authors_books WHERE author_id = @thisId;";
      cmd.Parameters.AddWithValue("@thisId", _id);
      // MySqlParameter thisId = new MySqlParameter();
      // thisId.ParameterName = "@thisId";
      // thisId.Value = this._id;
      // cmd.Parameters.Add(thisId);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

  }

}
