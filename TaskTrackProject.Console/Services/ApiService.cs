namespace TaskTrackProject.Console.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public class ApiService
{
    private static HttpClient _sharedClient;

    public ApiService(string baseUrl) 
    {
        _sharedClient =  new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    public async Task<string> GetTasksAsync()
    {
        using HttpResponseMessage response = await _sharedClient.GetAsync("api/Task");
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
    //     using HttpResponseMessage response = await _sharedClient.PostAsync("api/Task", jsonContent);
    //     response.EnsureSuccessStatusCode();

    //     return await response.Content.ReadAsStringAsync();
    // }
}