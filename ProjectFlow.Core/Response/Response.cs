using System.Text.Json.Serialization;

namespace ProjectFlow.Core.Response;

public class Response<TData>
{
    [JsonIgnore]
    public int Code { get; }

    [JsonConstructor]
    public Response() => Code = Configuration.DefaultStatusCode;
    public Response(TData? data, int code = Configuration.DefaultStatusCode, string? message = null)
    {
        Data = data;
        Code = code;
        Message = message;
    }
    public TData? Data { get; set; }
    public string? Message { get; set; }
    [JsonIgnore]
    public bool IsSuccess => Code is >= 200 and < 300;
}