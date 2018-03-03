using Microsoft.Data.Sqlite;
using System;

namespace FactoryMethodConsole
{
    public class SqLiteProvider : IDataSource
    {
        private static readonly string DataSource = "abc.db" + Guid.NewGuid();

        static SqLiteProvider()
        {
            CreateConnection();
        }

        public Person GetPerson(string name)
        {
            using (var connection = new SqliteConnection("DataSource=" + DataSource))
            {
                var command = connection.CreateCommand();
                string query = "SELECT Name, Age FROM Person Where Name = $name";
                command.CommandText = query;

                command.Parameters.AddWithValue("$name", name);
                connection.Open();
                var reader = command.ExecuteReader();
                Person person = new Person();
                while (reader.Read())
                {
                    Person p = new Person
                    {
                        Name = reader["Name"].ToString(),
                        Age = Convert.ToInt32(reader["Age"])
                    };
                    person = p;
                }
                return person;
            }
        }

        public void InsertPerson(string name, int age)
        {
            using (var connection = new SqliteConnection("DataSource=" + DataSource))
            {
                var command = connection.CreateCommand();
                string query = "INSERT INTO Person(Name, Age)"
                               + " VALUES('" + name + "',"
                               + age
                               + ");";
                command.CommandText = query;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private static void CreateConnection()
        {
            using (var connection = new SqliteConnection("DataSource=" + DataSource))
            {
                string query = "CREATE TABLE Person("
                               + "Name TEXT PRIMARY KEY NOT NULL,"
                               + "Age INT NOT NULL)";
                var command = connection.CreateCommand();
                command.CommandText = query;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}