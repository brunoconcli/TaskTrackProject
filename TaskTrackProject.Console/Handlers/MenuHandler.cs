namespace TaskTrackProject.Console.Handlers;
using TaskTrackProject.Console.Services;
using TaskTrackProject.Console.Shared;
using System;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

public class MenuHandler
{
    private static HttpClient _sharedClient;
    private static Dictionary<string, string> _menuOptions;

    public static async Task start() 
    {
        _sharedClient = new HttpClient
        {
            BaseAddress = new Uri(Menu.GetBaseUrl())
        };

        _menuOptions = new Dictionary<string, string>(){
            {"1", "Exibir tarefas"},
            {"2", "Criar tarefa"},
            {"3", "Editar tarefa"},
            {"4", "Remover tarefa"},
            {"5", "Marcar como completo"}
        };
        await DisplayMenu();
    }

    public static async Task GetTasks() 
    {   
        JArray jsonResponseArray = JArray.Parse(
            (string)
            await ApiService.GetTasksAsync(_sharedClient)
        );

        Console.WriteLine("Minhas tarefas");
        foreach (var jsonResponseObject in jsonResponseArray)
        {
            JObject currentObject = JObject.Parse(jsonResponseObject.ToString());
            string checkboxPrefix = currentObject["completed"].ToObject<bool>() ? "[X]" : "[ ]";
        
            Console.WriteLine(checkboxPrefix + currentObject["description"]);
        }
    }

    // public  void AddTask()
    // {

    // }

    // public  void UpdateTask() 
    // {

    // }

    // public  void RemoveTask()
    // {

    // }

    // public  void MarkAsComplete() 
    // {

    // }
    static async Task DisplayMenu(string infoMessage = "") 
    {
        Console.Clear();
        Console.WriteLine("Hoje:{} | Última atividade: ");
        
        foreach(KeyValuePair<string, string> option in _menuOptions)
        {
            Console.WriteLine("{0}. {1}", option.Key, option.Value);
        }

        Console.Write("\n" + infoMessage + "\nEscolha o que deseja fazer (1-5): ");
        string chosenOption = Console.ReadLine();
        await HandleOptionChoice(chosenOption);
    }

    static async Task HandleOptionChoice (string chosenOption)
    {
        string infoMessage = "";
        if (!int.TryParse(chosenOption, out _)) {
            infoMessage = Colors.RED + "A opção escolhida deve ser um número" + Colors.RESET;
        }
        else if (int.Parse(chosenOption) > 5 || int.Parse(chosenOption) < 0)
        {
            infoMessage = Colors.RED + "A opção deve ser um número positivo menor ou igual a 5" + Colors.RESET;
        }
        else
        {
            switch(chosenOption) 
            {
                case "1": 
                    Console.Clear();
                    await GetTasks();
                    Console.WriteLine("\nRetornar para o menu...");
                    Console.ReadLine();
                    break;
            }
        }
        await DisplayMenu(infoMessage);
    }

    static void Exit() 
    {
        // RegisterActivity();
        Console.WriteLine("Encerrando aplicação...");
        Environment.Exit(0);
    }

    static string GetUsersOS() 
    {
        // TO-DO
        return "";
    }
}