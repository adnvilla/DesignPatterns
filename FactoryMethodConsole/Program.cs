using System;
using System.Configuration;

namespace FactoryMethodConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var client = new Client();

            client.InsertPerson("NamePerson", 40);

            Console.WriteLine("Se inserto una persona");

            client.GetPerson("NamePerson");

            Console.WriteLine("Se consulto la persona 'NamePerson' ");

            Console.ReadKey();
        }
    }

    public class Client
    {
        private readonly IDataSource _dataSource;

        public Client()
        {
            var environment = ConfigurationManager.AppSettings["Environment"];
            _dataSource = new DbFactory().GetDbAdapter(environment);
        }

        public Person GetPerson(string name)
        {
            return _dataSource.GetPerson(name);
        }

        public void InsertPerson(string name, int age)
        {
            _dataSource.InsertPerson(name, age);
        }
    }
}