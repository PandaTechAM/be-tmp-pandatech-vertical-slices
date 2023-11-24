namespace PandaWebApi;

public class ForTempTests
{
    private readonly IConfiguration _configuration;

    public ForTempTests(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void Test()
    {
        var postgresConnectionString = _configuration.GetConnectionString("Postgres");
        var elasticSearchUrl = _configuration["ConnectionStrings:ElasticSearch"];
        var superUsername = _configuration["Security:SuperUser:Username"];
        var something = _configuration["Something"]!.Replace("", "aper");
        
        Console.WriteLine($"Postgres Connection String: {postgresConnectionString}");
        Console.WriteLine($"ElasticSearch URL: {elasticSearchUrl}");
        Console.WriteLine($"Super Username: {superUsername}");
        Console.WriteLine($"Something: {something}");

    }
}