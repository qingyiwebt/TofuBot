using Newtonsoft.Json;

namespace BotOneOne.Connectivity.Models;

public class BaseResponseDto
{
    [JsonProperty("post_type")] public string? PostType { get; set; }
    [JsonIgnore] public bool IsEventPacket => PostType != null;
}