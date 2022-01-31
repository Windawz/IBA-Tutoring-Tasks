using System;

namespace CSStarterTest1.DataOps.New
{
    /// <summary>
    /// Represents the most complete piece of data.
    /// </summary>
    public record Data(int Id, DateTime Date, string FirstName, string LastName, string SurName, string City, string Country);
}
