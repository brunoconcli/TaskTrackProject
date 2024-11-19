namespace TaskListCli.Models;

using MongoDB.Bson.Serialization.Attributes;

public class Task
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonElement("Description")]
    public required string Description { get; set; }
    [BsonElement("Completed")]
    public required bool Completed { get; set; }
}