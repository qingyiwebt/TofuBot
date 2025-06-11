using Newtonsoft.Json;

namespace BotOneOne.Protocol.OneBot.Dto;

public class BaseEventDto
{
    [JsonProperty("time")] public long Time { get; set; }
    
    [JsonProperty("post_type")] public string PostType { get; set; } = string.Empty;
}