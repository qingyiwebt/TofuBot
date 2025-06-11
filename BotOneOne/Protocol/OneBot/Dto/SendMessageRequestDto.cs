using BotOneOne.MessageFormat;
using Newtonsoft.Json;

namespace BotOneOne.Protocol.OneBot.Dto;

public struct SendMessageRequestDto
{
    [JsonProperty("user_id")] public long? UserId { get; set; }
    [JsonProperty("group_id")] public long? GroupId { get; set; }
    [JsonProperty("message")] public List<MessageSegment> MessageSegments { get; set; }
}