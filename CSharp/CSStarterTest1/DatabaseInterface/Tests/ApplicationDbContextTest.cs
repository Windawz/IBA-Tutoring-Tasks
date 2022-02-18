using System;
using System.IO;
using System.Linq;

using CSStarterTest1.DataOps;
using CSStarterTest1.TestUtils;

using Microsoft.Data.SqlClient;

namespace DatabaseInterface.Tests
{
    public class ApplicationDbContextTest : Test
    {
        public ApplicationDbContextTest(TextWriter writer) : base(writer) { }

        protected override TestResult PerformImpl()
        {
            var testResult = new TestResult(TestStatus.Success);

            // Connect to automatic instance
            var csb = new SqlConnectionStringBuilder
            {
                DataSource = "(LocalDB)\\MSSQLLocalDB",
                IntegratedSecurity = true
            };

            using var context = new ApplicationDbContext(csb.ConnectionString);
            bool hadToCreate = context.Database.EnsureCreated();

            var data = new Data
            (
                id: default,
                date: DateTime.Now,
                firstName: "FirstName1",
                lastName: "LastName1",
                surName: "SurName1",
                city: "City1",
                country: "Country1"
            );
            context.Add(data);
            context.SaveChanges();

            Data? readData = context.Datas.FirstOrDefault();
            if (readData is null)
            {
                testResult = new TestResult(TestStatus.Failure, $"{nameof(readData)} is null");
            }
            else
            {
                Logger.WriteLine("Successfully retrieved test data after adding.");
                foreach (var prop in typeof(Data).GetProperties())
                {
                    Logger.WriteLine($"{prop.Name}: \"{prop.GetValue(readData)!}\"");
                }
            }

            if (hadToCreate)
            {
                if (!context.Database.EnsureDeleted())
                {
                    testResult = new TestResult(TestStatus.Failure, "Failed to ensure database deletion");
                }
            }

            context.SaveChanges();

            return testResult;
        }
    }
}
