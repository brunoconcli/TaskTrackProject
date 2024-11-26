namespace TaskTrackProject.Console.Models;
using MongoDB.Bson.Serialization.Attributes;

public class Task
{
  [BsonId]
  [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
  public string? Id { get; set; }
  [BsonElement("description")]
  public required string Description { get; set; }
  [BsonElement("completed")]
  public required bool Completed { get; set; }
}