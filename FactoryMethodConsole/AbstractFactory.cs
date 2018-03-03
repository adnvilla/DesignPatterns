namespace FactoryMethodConsole
{
    public abstract class AbstractDbFactory
    {
        public abstract IDataSource GetDbAdapter(string environment);
    }
}





