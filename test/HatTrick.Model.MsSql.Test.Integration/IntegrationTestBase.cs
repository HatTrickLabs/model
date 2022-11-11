namespace HatTrick.Model.MsSql.Test.Integration;

public abstract class IntegrationTestBase
{
    protected MsSqlModel Model { get; }

    protected IntegrationTestBase()
    {
        Model = new MsSqlModelBuilder(ConfigurationProvider.ConnectionString).Build();
    }
}
