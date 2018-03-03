using System.Collections.Generic;
using System.Linq;

namespace FactoryMethodConsole
{
    public class RamProvider : IDataSource
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
}