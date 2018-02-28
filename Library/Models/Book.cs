using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public enum Status
  {
    available,
    on-hold,
    checked-out,
    missing
  };

  public class Book
  {
    private int _id;
    private string _title;
    private string _callNumber;
    private string _tagNumber;
    private DateTime _checkoutDate;
    private DateTime _dueDate;
    private Status _status;

    public Book(string title, string callNumber, string tagNumber, DateTime checkoutDate, DateTime dueDate, Status status = available, int id = 0)
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
      return checkoutDate;
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

    public Status GetStatus()
    {
      return _status;
    }

    public void SetStatus(Status status)
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
        Book newBook = (Book) otherBook:
        return _id == newBook._id && _title == newBook._title && _callNumber == newBook._callNumber && _tagNumber == newBook._tagNumber && _checkoutDate == newBook._checkoutDate && _dueDate == newBook._dueDate && _status == newBook._status);
      }
    }

    public override int GetHashCode()
    {
      return _id.GetHashCode();
    }
