using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Patron
  {
    private int _id;
    private string _firstName;
    private string _lastName;
    private string _email;
    private int _cardNumber;


    public Patron(string firstName, string lastName, string email, int cardNumber = 1000, int id=0)
    {
      _firstName = firstName;
      _lastName = lastName;
      _email = email;
      _cardNumber = cardNumber;
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

    public string GetEmail()
    {
      return _email;
    }

    public void SetEmail(string email)
    {
      _email = email;
    }

    public int GetCardNumber()
    {
      return _cardNumber;
    }

    public void SetCardNumber(int cardNumber)
    {
      _cardNumber = cardNumber;
    }

    public int GetId()
    {
      return _id;
    }

    public override bool Equals(System.Object otherPatron)
    {
      if (!(otherPatron is Patron))
      {
        return false;
      }
      else
      {
        Patron newPatron = (Patron) otherPatron;
        bool idEquality = (this.GetId() == newPatron.GetId());
        bool firstNameEquality = (this.GetFirstName() == newPatron.GetFirstName());
        bool lastNameEquality = (this.GetLastName() == newPatron.GetLastName());
        bool emailEquality = (this.GetEmail() == newPatron.GetEmail());
        bool cardNumberEquality = (this.GetCardNumber() == newPatron.GetCardNumber());
        // return _id == newBook._id && _title == newBook._title && _callNumber == newBook._callNumber && _tagNumber == newBook._tagNumber && _checkoutDate == newBook._checkoutDate && _dueDate == newBook._dueDate && _status == newBook._status;
        return (idEquality && firstNameEquality && lastNameEquality && emailEquality && cardNumberEquality);
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
      cmd.CommandText = @"INSERT INTO patrons (first_name, last_name, email, card_number) VALUES (@firstName, @lastName, @email, @cardNumber);";
      cmd.Parameters.AddWithValue("@firstName", _firstName);
      cmd.Parameters.AddWithValue("@lastName", _lastName);
      cmd.Parameters.AddWithValue("@email", _email);
      cmd.Parameters.AddWithValue("@cardNumber", _cardNumber);
      // cmd.Parameters.Add(new MySqlParameter("@firstName", _firstName));
      // cmd.Parameters.Add(new MySqlParameter("@lastName", _lastName));
      // cmd.Parameters.Add(new MySqlParameter("@email", _email));
      cmd.ExecuteNonQuery();

      _id = (int)cmd.LastInsertedId;

      conn.Close();
      if(conn != null)
        conn.Dispose();

    }

    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron>();
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT * FROM patrons";
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

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"TRUNCATE TABLE patrons; TRUNCATE TABLE patrons_books;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public void AddBook(Book patron)
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

    public List<Book> GetBooks()
    {
      List<Book> books = new List<Book>();

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"
        SELECT books.* FROM patrons
        JOIN patrons_books ON (patrons.id = patrons_books.patron_id)
        JOIN books ON (patrons_books.book_id = books.id)
        WHERE patrons.id = @ThisId;";
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
      cmd.CommandText = @"DELETE FROM patrons WHERE id = @thisId; DELETE from patrons_books WHERE patron_id = @thisId;";
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

    public static Patron Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT * FROM `patrons` WHERE id = @thisId;";

      cmd.Parameters.Add(new MySqlParameter("@thisId", id));

      MySqlDataReader rdr = cmd.ExecuteReader();

      int patronId = 0;
      string patronFirstName = "";
      string patronLastName = "";
      string patronEmail = "";
      int patronCardNumber = 0;

      while (rdr.Read())
      {
        patronId = rdr.GetInt32(0);
        patronFirstName = rdr.GetString(1);
        patronLastName = rdr.GetString(2);
        patronEmail = rdr.GetString(3);
        patronCardNumber = rdr.GetInt32(4);
      }

      Patron foundPatron = new Patron(patronFirstName, patronLastName, patronEmail, patronCardNumber, patronId);

      conn.Close();
      if (conn !=null)
      {
        conn.Dispose();
      }

      return foundPatron;
    }

  }

}
