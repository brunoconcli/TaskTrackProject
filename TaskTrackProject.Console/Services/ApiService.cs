namespace TaskTrackProject.Console.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

public static class ApiService
{
    public static async Task<string> GetTasksAsync(HttpClient httpClient)
    {
        using HttpResponseMessage response = await httpClient.GetAsync("api/Task");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<string> AddTaskAsync(HttpClient httpClient, string description, bool completed)
    {
        var jsonSerializedTask = JsonConvert.SerializeObject(new 
        {
            Description = description,
            Completed = completed
        });

        using StringContent jsonContentTask = new StringContent(
            jsonSerializedTask, 
            Encoding.UTF8, 
            "application/json"
        );
        using HttpResponseMessage response = await httpClient.PostAsync(
            "api/Task", 
            jsonContentTask
        );
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<string> UpdateTaskAsync(HttpClient httpClient, string id, string newDescription, bool newCompleted)
    {
        var jsonSerializedTask = JsonConvert.SerializedObject(new
        {
            Id = id,
            Description = newDescription,
            Completed = newCompleted
        });

        using StringContent jsonContentTask = new StringContent(
            jsonSerializedTask,
            Encoding.UTF8,
            "application/json"
        );
        using HttpResponseMessage response = await httpClient.PutAsync(
            "api/Task/" + jsonSerializedTask.Id,
            jsonContentTask
        );
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<string> DeleteTaskAsync(HttpClient httpClient, string id)
    {
        using HttpResponseMessage response = await httpClient.DeleteAsync("api/Task/" + id);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}