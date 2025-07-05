using System.Text.Json.Serialization;

namespace ProjectFlow.Core.Request;

public class EntityIdRequest
{
    [JsonIgnore]
    public int Id { get; set; }
}