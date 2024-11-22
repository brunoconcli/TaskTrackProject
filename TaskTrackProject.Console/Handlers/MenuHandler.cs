namespace TaskTrackProject.Console.Handlers;
using System;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TaskTrackProject.Console.Services;

public class MenuHandler
{
    private static HttpClient _sharedClient;
    private static Dictionary<string, string> _menuOptions;

    public static void start() 
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
        DisplayMenu();
    }

    public static async Task GetTasks() 
    {        
        Console.WriteLine(await ApiService.GetTasksAsync(_sharedClient));
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
    static void DisplayMenu(string infoMessage = "") 
    {
        Console.Clear();
        Console.WriteLine("Hoje:{} | Última atividade: ");
        foreach(KeyValuePair<string, string> option in _menuOptions)
        {
            Console.WriteLine("{0}. {1}", option.Key, option.Value);
        }

        Console.Write("\n" + infoMessage + "\nEscolha o que deseja fazer (1-5): ");
        string chosenOption = Console.ReadLine();
        HandleOptionChoice(chosenOption);
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
                    await GetTasks();
                    break;
            }
        }
        DisplayMenu(infoMessage);
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