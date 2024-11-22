using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using TaskTrackProject.Console.Handlers;
using TaskTrackProject.Console.Services;


public static class Menu 
{
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