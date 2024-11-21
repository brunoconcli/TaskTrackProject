using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using TaskTrackProject.Console.Services;
using TaskTrackProject.Console.Handlers;

public static class Menu 
{
    static async Task Main(string[] args) 
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        string baseUrl = configuration["ApiSettings:BaseUrl"];

        MenuHandler.start();
    }
}