using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Book
  {
    private int _id;
    private string _title;
    private string _callNumber;
    private string _tagNumber;
    private DateTime _checkoutDate;
    private DateTime _dueDate;
    private string _status;

// DateTime checkoutDate = DateTime.Today, DateTime dueDate = DateTime.Today,
    public Book(string title, string callNumber, string tagNumber,DateTime checkoutDate , DateTime dueDate ,string status = "foo", int id = 0)
    {
      _title = title;
      _callNumber = callNumber;
      _tagNumber = tagNumber;
      _checkoutDate = checkoutDate;
      _dueDate = dueDate;
      _status = status;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }

    public void SetId(int id)
    {
      _id = id;
    }

    public string GetTitle()
    {
      return _title;
    }

    public void SetTitle(string title)
    {
      _title = title;
    }

    public string GetCallNumber()
    {
      return _callNumber;
    }

    public void SetCallNumber(string callNumber)
    {
      _callNumber = callNumber;
    }

    public string GetTagNumber()
    {
      return _tagNumber;
    }

    public void SetTagNumber(string tagNumber)
    {
      _tagNumber = tagNumber;
    }

    public DateTime GetCheckoutDate()
    {
      return _checkoutDate;
    }

    public void SetCheckoutDate(DateTime checkoutDate)
    {
      _checkoutDate = checkoutDate;
    }

    public DateTime GetDueDate()
    {
      return _dueDate;
    }

    public void SetDueDate(DateTime dueDate)
    {
      _dueDate = dueDate;
    }

    public string GetStatus()
    {
      return _status;
    }

    public void SetStatus(string status)
    {
      _status = status;
    }

    public override bool Equals(System.Object otherBook)
    {
      if (!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool idEquality = (this.GetId() == newBook.GetId());
        bool titleEquality = (this.GetTitle() == newBook.GetTitle());
        bool callNumberEquality = (this.GetCallNumber() == newBook.GetCallNumber());
        bool tagNumberEquality = (this.GetTagNumber() == newBook.GetTagNumber());
        bool checkoutDateEquality = (this.GetCheckoutDate() == newBook.GetCheckoutDate());
        bool dueDateEquality = (this.GetDueDate() == newBook.GetDueDate());
        bool statusEquality = (this.GetStatus() == newBook.GetStatus());
        // return _id == newBook._id && _title == newBook._title && _callNumber == newBook._callNumber && _tagNumber == newBook._tagNumber && _checkoutDate == newBook._checkoutDate && _dueDate == newBook._dueDate && _status == newBook._status;
        return (idEquality && titleEquality && callNumberEquality && tagNumberEquality && checkoutDateEquality && dueDateEquality && statusEquality);
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
      cmd.CommandText = @"INSERT INTO books (title, call_number, tag_number, checkout_date, duedate, status) VALUES (@title, @callNumber, @tagNumber, @checkoutDate, @dueDate, @status);";
      cmd.Parameters.Add(new MySqlParameter("@title", _title));
      cmd.Parameters.Add(new MySqlParameter("@callNumber", _callNumber));
      cmd.Parameters.Add(new MySqlParameter("@tagNumber", _tagNumber));
      cmd.Parameters.Add(new MySqlParameter("@checkoutDate", _checkoutDate));
      cmd.Parameters.Add(new MySqlParameter("@dueDate", _dueDate));
      cmd.Parameters.Add(new MySqlParameter("@status", _status));
      cmd.ExecuteNonQuery();

      _id = (int)cmd.LastInsertedId;

      conn.Close();
      if(conn != null)
        conn.Dispose();

    }

    public static List<Book> GetAll()
    {
      List<Book> allBooks = new List<Book>();
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT * FROM books";
      MySqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookTitle = rdr.GetString(1);
        string bookCallNumber = rdr.GetString(2);
        string bookTagNumber = rdr.GetString(3);
        DateTime bookCheckOutDate = rdr.GetDateTime(4);
        DateTime bookDueDate = rdr.GetDateTime(5);
        string bookStatus = rdr.GetString(6);
        Book newBook = new Book(bookTitle, bookCallNumber, bookTagNumber, bookCheckOutDate, bookDueDate, bookStatus, bookId);
        allBooks.Add(newBook);
      }

      conn.Close();
      if (conn != null)
        conn.Dispose();
      return allBooks;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"TRUNCATE TABLE books; TRUNCATE TABLE patrons_books; TRUNCATE TABLE authors_books;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public static Book Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT * FROM `books` WHERE id = @thisId;";

      cmd.Parameters.Add(new MySqlParameter("@thisId", id));

      MySqlDataReader rdr = cmd.ExecuteReader();

      int bookId = 0;
      string bookTitle = "";
      string bookCallNumber = "";
      string bookTagNumber = "";
      DateTime bookCheckOutDate = DateTime.Today;
      DateTime bookDueDate = DateTime.Today;
      string bookStatus = "";

      while (rdr.Read())
      {
        bookId = rdr.GetInt32(0);
        bookTitle = rdr.GetString(1);
        bookCallNumber = rdr.GetString(2);
        bookTagNumber = rdr.GetString(3);
        bookCheckOutDate = rdr.GetDateTime(4);
        bookDueDate = rdr.GetDateTime(5);
        bookStatus = rdr.GetString(6);
      }

      Book foundBook = new Book(bookTitle, bookCallNumber, bookTagNumber, bookCheckOutDate, bookDueDate, bookStatus, bookId);

      conn.Close();
      if (conn !=null)
      {
        conn.Dispose();
      }

      return foundBook;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM books WHERE id = @thisId; DELETE from authors_books WHERE book_id = @thisId; DELETE from patrons_books WHERE book_id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = this._id;
      cmd.Parameters.Add(thisId);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddAuthor(Author author)
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

    public void AddPatron(Patron patron)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"INSERT INTO patrons_books (patron_id, book_id) VALUES (@PatronId, @BookId)";
      cmd.Parameters.Add(new MySqlParameter("@PatronId", patron.GetId()));
      cmd.Parameters.Add(new MySqlParameter("@BookId", _id));
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public List<Author> GetAuthors()
    {
      List<Author> allAuthors = new List<Author>();

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"
        SELECT authors.* FROM books
        JOIN authors_books ON (books.id = authors_books.book_id)
        JOIN authors ON (authors_books.author_id = authors.id)
        WHERE books.id = @ThisId;";
      cmd.Parameters.Add(new MySqlParameter("@ThisId", _id));
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

    public List<Patron> GetPatrons()
    {
      List<Patron> allPatrons = new List<Patron>();

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"
        SELECT patrons.* FROM books
        JOIN patrons_books ON (books.id = patrons_books.book_id)
        JOIN patrons ON (patrons_books.patron_id = patrons.id)
        WHERE books.id = @ThisId;";
      cmd.Parameters.Add(new MySqlParameter("@ThisId", _id));
      MySqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {

        int patronId = rdr.GetInt32(0);
        string patronFirstName= rdr.GetString(1);
        string patronLastName = rdr.GetString(2);
        string patronEmail = rdr.GetString(3);
        int patronCardNumber = rdr.GetInt32(4);
        Patron newPatron = new Patron(patronFirstName, patronLastName, patronEmail, patronCardNumber, patronId);
        allPatrons.Add(newPatron);
      }

      conn.Close();
      if (conn != null)
        conn.Dispose();

      return allPatrons;

    }

    public void Edit(string newTitle, string newCallNumber, string newTagNumber, DateTime newCheckoutDate, DateTime newDueDate, string newStatus)
    {

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE `books` SET title = @title, call_number = @callNumber, tag_number = @tagNumber, checkout_date = @checkoutDate, duedate = @dueDate, status = @status WHERE id = @searchId;";
      cmd.Parameters.AddWithValue("@searchId", _id);
      cmd.Parameters.AddWithValue("@title", newTitle);
      cmd.Parameters.AddWithValue("@callNumber", newCallNumber);
      cmd.Parameters.AddWithValue("@tagNumber", newTagNumber);
      cmd.Parameters.AddWithValue("@checkoutDate", newCheckoutDate);
      cmd.Parameters.AddWithValue("@dueDate", newDueDate);
      cmd.Parameters.AddWithValue("@status", newStatus);

      cmd.ExecuteNonQuery();
      _title = newTitle;
      _callNumber = newCallNumber;
      _tagNumber = newTagNumber;
      _checkoutDate = newCheckoutDate;
      _dueDate = newDueDate;
      _status = newStatus;

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();

      }

    }

    public bool BookAvails(int bookId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText =@"SELECT * FROM books WHERE id = @thisId;";
      cmd.Parameters.Add(new MySqlParameter("@thisId", bookId));

      MySqlDataReader rdr = cmd.ExecuteReader();

      string bookStatus = "";

      while (rdr.Read())
      {
        bookStatus = rdr.GetString(6);
      }

      conn.Close();
      if (conn !=null)
      {
        conn.Dispose();
      }

      return bookStatus == "available";

    }


    public void Checkout(int bookId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();


    }
  }
}
