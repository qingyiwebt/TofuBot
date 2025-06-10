using Newtonsoft.Json;

namespace BotOneOne.MessageFormat;

public abstract class MessageSegment
{
    [JsonProperty("type")] public abstract string Type { get; }
}

public abstract class MessageSegment<T> : MessageSegment
{
    [JsonProperty("data")] public T? Data { get; set; }
}