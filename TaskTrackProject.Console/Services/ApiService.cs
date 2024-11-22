namespace TaskTrackProject.Console.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public static class ApiService
{
    public static async Task<string> GetTasksAsync(HttpClient httpClient)
    {
        using HttpResponseMessage response = await httpClient.GetAsync("api/Task");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    // public async Task<string> AddTaskAsync(string description, bool completed)
    // {
    //     using StringContent jsonContent = new(
    //         JsonSerializer.Serialize(new {
    //             Description = description,
    //             Completed = completed
    //         }), Encoding.UTF8, "application/json");
    //     using HttpResponseMessage response = await httpClient.PostAsync("api/Task", jsonContent);
    //     response.EnsureSuccessStatusCode();

    //     return await response.Content.ReadAsStringAsync();
    // }
}