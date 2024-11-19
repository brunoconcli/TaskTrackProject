using Microsoft.AspNetCore.Mvc;
using TaskListCli.Services;
namespace TaskListCli.Controllers;

[Controller]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly MongoDBService _mongoDBService;
    public TaskController(MongoDBService mongoDBService)
    {
        _mongoDBService = mongoDBService;
    }

    [HttpGet]
    public async Task<List<Models.Task>> Get()
    {
        return await _mongoDBService.GetAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Models.Task task)
    {
        await _mongoDBService.CreateAsync(task);
        return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, string description, bool completed)
    {
        await _mongoDBService.UpdateAsync(id, description, completed);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _mongoDBService.DeleteAsync(id);
        return NoContent();
    }
}