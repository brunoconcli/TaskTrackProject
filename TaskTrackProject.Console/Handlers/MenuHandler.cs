namespace TaskTrackProject.Console.Handlers;
using TaskTrackProject.Console.Services;
using TaskTrackProject.Console.Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

public class MenuHandler
{
  private static HttpClient _sharedClient;
  private static Dictionary<string, string> _menuOptions;
  private static List<JObject> _currentTasksArray;
  private static string _lastActivityTime;

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
    _lastActivityTime = GetCurrentTime();
    await DisplayMenu();
  }

  public static async Task GetTasks()
  {
    await UpdateCurrentTasksArray();

    Console.WriteLine(InterfaceConsts.HeaderGet);
    if (_currentTasksArray.Count == 0)
    {
      Console.WriteLine(InterfaceConsts.EmptyArrayGet);
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
    Console.WriteLine(InterfaceConsts.HeaderPost);
    Console.Write("Descrição: ");
    string inputDescription = Console.ReadLine();

    if (inputDescription.Length != 0)
    {
      await ApiService.AddTaskAsync(_sharedClient, inputDescription, false);
      Console.WriteLine(InterfaceConsts.SuccessPost);
    }
    else
    {
      Console.WriteLine(InterfaceConsts.CancelPost);
    }
  }

  public static async Task UpdateTask() 
  {
    await UpdateCurrentTasksArray();
    Console.WriteLine(InterfaceConsts.HeaderPut);
    if (_currentTasksArray.Count == 0) 
    {
      Console.WriteLine(InterfaceConsts.EmptyArrayPut);
    }
    else {
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

        Console.Write("\nNova descrição: ");
        string newDescription = Console.ReadLine();

        await ApiService.UpdateTaskAsync(_sharedClient, chosenTaskId, newDescription, false);
        Console.WriteLine(InterfaceConsts.SuccessPut); 
      }
      else 
      {
        Console.WriteLine(InterfaceConsts.CancelPut);
      }
    }
  }

  public static async Task DeleteTask()
  {
    await UpdateCurrentTasksArray();
    Console.WriteLine(InterfaceConsts.HeaderDelete);

    if (_currentTasksArray.Count == 0) 
    {
      Console.WriteLine(InterfaceConsts.EmptyArrayDelete);
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
        Console.WriteLine(InterfaceConsts.SuccessDelete);
      }
      else
      {
        Console.WriteLine(InterfaceConsts.CancelDelete);
      }
    }
  }

  public static async Task MarkAsComplete() 
  {
    await UpdateCurrentTasksArray();
    Console.WriteLine(InterfaceConsts.HeaderMarkAsComplete);
    
    if (_currentTasksArray.Count == 0) 
    {
      Console.WriteLine(InterfaceConsts.EmptyArrayMarkAsComplete);
    }
    else
    {
      int optionIndex = 0;
      foreach (var item in _currentTasksArray)
      {
        optionIndex++;
        if (item["completed"].ToObject<bool>())
        Console.WriteLine((optionIndex) + ". [X] " + item["description"]);
      }

      Console.Write("\nTarefa para concluir (1-" + optionIndex + "): ");
      string chosenTask = Console.ReadLine(); 

      if (int.TryParse(chosenTask, out _)) 
      {
        int chosenTaskIndex = int.Parse(chosenTask)-1;
        string chosenTaskId = _currentTasksArray[chosenTaskIndex]["id"].ToString();
        string chosenTaskDescription = _currentTasksArray[chosenTaskIndex]["description"].ToString();

        await ApiService.UpdateTaskAsync(_sharedClient, chosenTaskId, chosenTaskDescription, true);
        Console.WriteLine(InterfaceConsts.SuccessMarkAsComplete);
      }
      else
      {
        Console.WriteLine(InterfaceConsts.CancelMarkAsComplete);
      }
    }
  }

  static async Task DisplayMenu(string infoMessage = "")
  {
    Console.Clear();
    Console.WriteLine(
      Colors.CYAN + 
      "Olá, usuário " + GetUsersOS() + " !\n" + 
      Colors.RESET +
      GetCurrentDay() + " | Última atividade: " + _lastActivityTime + "\n"
    );

    foreach (KeyValuePair<string, string> option in _menuOptions)
    {
      Console.WriteLine("{0}. {1}", option.Key, option.Value);
    }

    Console.Write("\n" + infoMessage + Colors.GREEN + "\nEscolha o que deseja fazer (1-5): " + Colors.RESET);
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
        case "5": 
          await MarkAsComplete();
          break;
      }
      BackToMenu();
      _lastActivityTime = GetCurrentTime();
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
    return System.Runtime.InteropServices.RuntimeInformation.OSDescription;
  }

  static string GetCurrentDay()
  {
    return DateTime.Now.ToString("dd/MM/yyyy");
  }

  static string GetCurrentTime()
  {
    return DateTime.Now.ToString("HH:mm");
  }
}