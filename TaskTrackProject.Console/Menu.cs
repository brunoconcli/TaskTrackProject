using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using TaskTrackProject.Console.Handlers;

public static class Menu 
{
    static ConfigurationBuilder builder = new ConfigurationBuilder();
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

    IConfiguration config = builder.Build();

    static async Task Main(string[] args) 
    {
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