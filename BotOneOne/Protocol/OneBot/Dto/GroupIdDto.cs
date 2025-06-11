using Newtonsoft.Json;

namespace BotOneOne.Protocol.OneBot.Dto;

public struct GroupIdDto
{
    [JsonProperty("group_id")] public long GroupId { get; set; }
}