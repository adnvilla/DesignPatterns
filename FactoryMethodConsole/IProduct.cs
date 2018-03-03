namespace FactoryMethodConsole
{
    public interface IDataSource
    {
        Person GetPerson(string name);

        void InsertPerson(string name, int age);
    }

    public class Person
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}