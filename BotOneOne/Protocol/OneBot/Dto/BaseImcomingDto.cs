using Newtonsoft.Json;

namespace BotOneOne.Protocol.OneBot.Dto;

public class BaseImcomingDto
{
    [JsonProperty("post_type")] public string? PostType { get; set; }
    [JsonIgnore] public bool IsEventPacket => PostType != null;
}