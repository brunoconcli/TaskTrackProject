using TaskListCli.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace TaskListCli.Services;

public class MongoDBService
{
    private readonly IMongoCollection<Models.Task> _taskCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _taskCollection = database.GetCollection<Models.Task>(mongoDBSettings.Value.CollectionName);
    }

    public async Task<List<Models.Task>> GetAsync()
    {
        return await _taskCollection.Find(new BsonDocument()).ToListAsync();
    }
    public async System.Threading.Tasks.Task CreateAsync(Models.Task task)
    {
        await _taskCollection.InsertOneAsync(task);
        return;
    }
    public async System.Threading.Tasks.Task UpdateAsync(string id, string description, bool completed)
    {
        FilterDefinition<Models.Task> filter = Builders<Models.Task>.Filter.Eq("Id", id);
        UpdateDefinition<Models.Task> update = Builders<Models.Task>.Update.Set("Description", description).Set("Completed", completed);
        await _taskCollection.UpdateOneAsync(filter, update);
        return;
    }
    public async System.Threading.Tasks.Task DeleteAsync(string id)
    {
        FilterDefinition<Models.Task> filter = Builders<Models.Task>.Filter.Eq("Id", id);
        await _taskCollection.DeleteOneAsync(filter);
        return;
    }
}