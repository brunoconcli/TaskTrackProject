namespace TaskTrackProject.Console.Handlers;
using TaskTrackProject.Console.Services;
using TaskTrackProject.Console.Shared;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

public class MenuHandler
{
  private static HttpClient _sharedClient;
  private static Dictionary<string, string> _menuOptions;
  private static List<JObject> _currentTasksArray;

  public static async Task start()
  {
    _sharedClient = new HttpClient
    {
      BaseAddress = new Uri(Menu.GetBaseUrl())
    };

    _currentTasksArray = new List<JObject>();
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
    if (_currentTasksArray.Count == 0)
    {
      Console.WriteLine(Colors.YELLOW + "Não há atividades para exibir" + Colors.RESET);
    }
    else 
    {
      foreach (var item in _currentTasksArray)
      {
        string optionPrefix = item["completed"].ToObject<bool>() ? "[X]" : "[ ]";
        Console.WriteLine(optionPrefix + " " + item["description"]);
      }
    }
  }

  public static async Task AddTask()
  {
    Console.WriteLine(Colors.WHITE_BOLD + "> Adicionar nova tarefa\nPressione [Enter] para cancelar.\n" + Colors.RESET);
    Console.Write("Descrição: ");
    string inputDescription = Console.ReadLine();

    if (inputDescription.Length != 0)
    {
      await ApiService.AddTaskAsync(_sharedClient, inputDescription, false);
      Console.WriteLine(Colors.GREEN + "\nAtividade adicionada com sucesso." + Colors.RESET);
    }
    else
    {
      Console.WriteLine(Colors.YELLOW + "\nAdição de nova atividade cancelada." + Colors.RESET);
    }
  }

  public static async Task UpdateTask() 
  {
    await UpdateCurrentTasksArray();
    Console.WriteLine("> Editar tarefa\nPressione [Enter] para cancelar.\n");
    int optionIndex = 0;
    foreach (var item in _currentTasksArray)
    {
      optionIndex++;
      Console.WriteLine((optionIndex) + ". " + item["description"]);
    }

    Console.Write("\nTarefa para edição (1-" + optionIndex + "): ");
    string chosenTask = Console.ReadLine();

    if (int.TryParse(chosenTask, out _))
    {
      string chosenTaskId = _currentTasksArray[int.Parse(chosenTask)-1]["id"].ToString();
      string chosenTaskDescription = _currentTasksArray[int.Parse(chosenTask)-1]["description"].ToString();

      Console.Write("\nNova descrição: ");
      string newDescription = Console.ReadLine();

      string response = await ApiService.UpdateTaskAsync(_sharedClient, chosenTaskId, newDescription, true);
      Console.WriteLine(response);
    }
    else 
    {
      Console.WriteLine(Colors.YELLOW + "\nEdição de atividade cancelada." + Colors.RESET);
    }
  }

  public static async Task DeleteTask()
  {
    await UpdateCurrentTasksArray();
    Console.WriteLine(Colors.WHITE_BOLD + "> Remover tarefa\nPressione [Enter] para cancelar.\n" + Colors.RESET);

    if (_currentTasksArray.Count == 0) 
    {
      Console.WriteLine(Colors.YELLOW + "Não há atividades para remover." + Colors.RESET);
    }
    else
    {
      int optionIndex = 0;
      foreach (var item in _currentTasksArray)
      {
        optionIndex++;
        string optionPrefix = item["completed"].ToObject<bool>() ? "[X]" : "[ ]";

        Console.WriteLine((optionIndex) + ". " + optionPrefix + " " + item["description"]);
      }

      Console.Write("\nTarefa para deleção (1-" + optionIndex + "): ");
      string chosenTask = Console.ReadLine(); 

      if (int.TryParse(chosenTask, out _)) 
      {
        string chosenTaskId = _currentTasksArray[int.Parse(chosenTask)-1]["id"].ToString();
        await ApiService.DeleteTaskAsync(_sharedClient, chosenTaskId);
        Console.WriteLine(Colors.GREEN + "\nAtividade removida com sucesso." + Colors.RESET);
      }
      else
      {
        Console.WriteLine(Colors.YELLOW + "\nAdição de nova atividade cancelada." + Colors.RESET);
      }
    }
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
        case "3":
          await UpdateTask();
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
    _currentTasksArray.Clear();
    JArray jsonTaskArray = JArray.Parse(
        (string)
        await ApiService.GetTasksAsync(_sharedClient)
    );
    foreach (var jsonResponseObject in jsonTaskArray)
    {
      JObject currentObject = JObject.Parse(jsonResponseObject.ToString());      
      _currentTasksArray.Add(currentObject);
    }
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