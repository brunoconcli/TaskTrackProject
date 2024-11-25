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
  private static JArray _currentTasksArray;

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
    await UpdateCurrentTasksArray();

    Console.WriteLine(Colors.WHITE_BOLD + "> Minhas tarefas\n" + Colors.RESET);
    foreach (var jsonResponseObject in _currentTasksArray)
    {
      JObject currentObject = JObject.Parse(jsonResponseObject.ToString());
      string checkboxPrefix = currentObject["completed"].ToObject<bool>() ? "[X] " : "[ ] ";

      Console.WriteLine(checkboxPrefix + currentObject["description"]);
    }
  }

  public static async Task AddTask()
  {
    Console.WriteLine(Colors.WHITE_BOLD + "> Adicionar nova tarefa\nEnvie '-' para cancelar.\n" + Colors.RESET);
    Console.Write("Descrição: ");
    string inputDescription = Console.ReadLine();

    if (inputDescription != "-")
    {
      await ApiService.AddTaskAsync(_sharedClient, inputDescription, false);
      Console.WriteLine(Colors.GREEN + "\nAtividade adicionada com sucesso." + Colors.RESET);
    }
    else
    {
      Console.WriteLine(Colors.YELLOW + "\nAdição de nova atividade cancelada." + Colors.RESET);
    }
  }

  // public  void UpdateTask() 
  // {

  // }

  public static async Task DeleteTask()
  {
    await UpdateCurrentTasksArray();
    Console.WriteLine(Colors.WHITE_BOLD + "> Remover tarefa\nPressione [Enter] para cancelar.\n" + Colors.RESET);

    let counter = 0; // stawped heah [15:44]
    foreach (var jsonResponseObject in _currentTasksArray)
    {
      JObject currentObject = JObject.Parse(jsonResponseObject.ToString());
      string checkboxPrefix = currentObject["completed"].ToObject<bool>() ? "[X] " : "[ ] ";

      Console.WriteLine(checkboxPrefix + currentObject["description"]);
    }
    string response = await ApiService.DeleteTaskAsync();
    Console.WriteLine(response);
  }

  // public  void MarkAsComplete() 
  // {

  // }
  static async Task DisplayMenu(string infoMessage = "")
  {
    Console.Clear();
    Console.WriteLine("Hoje:{} | Última atividade: ");

    foreach (KeyValuePair<string, string> option in _menuOptions)
    {
      Console.WriteLine("{0}. {1}", option.Key, option.Value);
    }

    Console.Write("\n" + infoMessage + "\nEscolha o que deseja fazer (1-5): ");
    string chosenOption = Console.ReadLine();
    await HandleOptionChoice(chosenOption);
  }

  static async Task HandleOptionChoice(string chosenOption)
  {
    string infoMessage = "";
    if (!int.TryParse(chosenOption, out _))
    {
      infoMessage = Colors.RED + "A opção escolhida deve ser um número" + Colors.RESET;
    }
    else if (int.Parse(chosenOption) > 5 || int.Parse(chosenOption) < 0)
    {
      infoMessage = Colors.RED + "A opção deve ser um número positivo menor ou igual a 5" + Colors.RESET;
    }
    else
    {
      Console.Clear();
      switch (chosenOption)
      {
        case "1":
          await GetTasks();
          break;
        case "2":
          await AddTask();
          break;
        case "4":
          await DeleteTask();
          break;
      }
      BackToMenu();
    }
    await DisplayMenu(infoMessage);
  }

  static async Task UpdateCurrentTasksArray()
  {
    _currentTasksArray = JArray.Parse(
        (string)
        await ApiService.GetTasksAsync(_sharedClient)
    );
  }

  static void BackToMenu()
  {
    Console.WriteLine("\nRetornar para o menu...");
    Console.ReadLine();
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