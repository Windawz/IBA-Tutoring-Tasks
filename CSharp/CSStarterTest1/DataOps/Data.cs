using System;

namespace CSStarterTest1.DataOps
{
    /// <summary>
    /// Represents the most complete piece of data.
    /// </summary>
    public class Data
    {
        public static readonly int FieldCount = typeof(Data).GetProperties().Length;

        public Data(int id, DateTime date, string firstName, string lastName, string surName, string city, string country)
        {
            Id = id;
            Date = date;
            FirstName = firstName;
            LastName = lastName;
            SurName = surName;
            City = city;
            Country = country;
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
