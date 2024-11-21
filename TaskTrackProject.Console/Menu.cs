using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using TaskTrackProject.Console.Handlers;
using TaskTrackProject.Console.Services;


public static class Menu 
{
    static async Task Main(string[] args) 
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        string baseUrl = configuration["ApiSettings:BaseUrl"];

        MenuHandler.start();
        // string menuHandlerTasks = await MenuHandler.GetTasks();
        // Console.WriteLine(menuHandlerTasks);
    }

    public static string GetBaseUrl() 
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        return configuration["ApiSettings:BaseUrl"];
    }
}