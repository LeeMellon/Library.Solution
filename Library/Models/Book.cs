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

    public Book(string title, string callNumber, string tagNumber, DateTime checkoutDate, DateTime dueDate, string status = "available", int id = 0)
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

    }
  }
