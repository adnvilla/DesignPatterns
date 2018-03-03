namespace FactoryMethodConsole
{
    public class DbFactory
    {
        public IDataSource GetDbAdapter(string environment)
        {
            switch (environment)
            {
                case "Prod":
                    return new SqLiteProvider();
                case "Test":
                    return new RamProvider();
                case "OtherProvider":
                    return new OtherProvider();
                default:
                    return new RamProvider();
            }
        }
    }
}