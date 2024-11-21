namespace TaskTrackProject.Console.Handlers;
using System;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TaskTrackProject.Console.Services;

public class MenuHandler
{
    private static ApiService _apiService;
    private static Dictionary<string, string> _menuOptions;

    public static void start() 
    {
        _apiService = new ApiService(Menu.GetBaseUrl());
        _menuOptions = new Dictionary<string, string>(){
            {"1", "Exibir tarefas"},
            {"2", "Criar tarefa"},
            {"3", "Editar tarefa"},
            {"4", "Remover tarefa"},
            {"5", "Marcar como completo"}
        };
        DisplayMenu();
    }

    public static async Task<string> GetTasks() 
    {
        return await _apiService.GetTasksAsync();
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
    static void DisplayMenu() 
    {
        Console.Clear();
        Console.WriteLine("Hoje:{} | Última atividade: ");
        foreach(KeyValuePair<string, string> option in _menuOptions)
        {
            Console.WriteLine("{0}. {1}", option.Key, option.Value);
        }

        // Console.WriteLine("\n0. Encerrar sessão");
        Console.Write("\nEscolha o que deseja fazer (1-5): ");
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