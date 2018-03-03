namespace FactoryMethodConsole
{
    public class OtherProvider : IDataSource
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