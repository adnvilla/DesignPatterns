using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace FactoryMethodConsoleOld
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var client = new Client(ConfigurationManager.AppSettings["Environment"]);

            client.InsertPerson("NamePerson", 40);

            Console.WriteLine("Se inserto una persona");

            client.GetPerson("NamePerson");

            Console.WriteLine("Se consulto la persona 'NamePerson' ");

            Console.ReadKey();
        }
    }

    public class Client
    {
        private readonly string _environment;

        public Client(string environment)
        {
            _environment = environment;
        }

        public Person GetPerson(string name)
        {
            if (_environment == "Prod")
            {
                var sqliteProvider = new SqliteProvider();
                return sqliteProvider.GetPerson(name);
            }
            else if (_environment == "Test")
            {
                var ramProvider = new RamProvider();
                return ramProvider.GetPerson(name);
            }
            else if(_environment == "OtherEnvironment")
            {
                var otherProvider = new OtherProvider();
                return otherProvider.GetPerson(name);
            }

            return null;
        }

        public void InsertPerson(string name, int age)
        {
            if (_environment == "Prod")
            {
                var sqliteProvider = new SqliteProvider();
                sqliteProvider.InsertPerson("NamePerson", 40);
            }
            else
            {
                var ramProvider = new RamProvider();
                ramProvider.InsertPerson("NamePerson", 40);
            }
        }
    }

    public class Person
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }

    public class RamProvider
    {
        public static List<Person> Persons { get; set; }

        static RamProvider()
        {
            Persons = new List<Person>();
        }

        public Person GetPerson(string name)
        {
            return Persons.FirstOrDefault(x => x.Name == name);
        }

        public void InsertPerson(string name, int age)
        {
            Persons.Add(new Person
            {
                Name = name,
                Age = age
            });
        }
    }

    public class SqliteProvider
    {
        private static readonly string DataSource = "abc.db" + Guid.NewGuid();

        static SqliteProvider()
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

    public class OtherProvider
    {
        public Person GetPerson(string name)
        {
            return null;
        }

        public void InsertPerson(string name, int age)
        {
        }
    }
}