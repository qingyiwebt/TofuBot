using Newtonsoft.Json;

namespace BotOneOne.Protocol.OneBot.Dto;

public struct UserAndGroupDto
{
    [JsonProperty("user_id")] public long UserId { get; set; }
    [JsonProperty("group_id")] public long GroupId { get; set; }
}