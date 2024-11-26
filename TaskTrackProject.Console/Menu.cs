using Microsoft.Extensions.Configuration;
using TaskTrackProject.Console.Handlers;

public static class Menu 
{
    static async Task Main(string[] args) 
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true);
        IConfiguration config = builder.Build();

        await MenuHandler.start();
    }

    public static string GetBaseUrl() 
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        return configuration["ApiSettings:BaseUrl"];
    }
}