namespace HatTrick.Model.MySql.Test.Integration;

public abstract class IntegrationTestBase
{
    protected MySqlModel Model { get; }

    protected IntegrationTestBase(string database, params string[] databases)
    {
        Model = new MySqlModelBuilder(ConfigurationProvider.ConnectionString, database, databases).Build();
    }
}
