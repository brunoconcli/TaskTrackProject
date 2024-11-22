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
        using HttpResponseMessage response = await httpClient.PostAsync("api/Task", jsonContentTask);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}