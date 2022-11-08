namespace HatTrick.Model.MsSql.Test.Integration;

internal static class ConfigurationProvider
{
    private static readonly string ConnectionStringKey = "hattrick_model_mssql_test";
    private static IConfiguration? _configuration;

    public static IConfiguration Configuration => _configuration ??= new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                    .AddEnvironmentVariables()
                    .Build();

    public static string ConnectionString => Configuration.GetConnectionString(ConnectionStringKey)!;
}
