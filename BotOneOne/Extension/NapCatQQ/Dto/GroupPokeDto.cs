using Newtonsoft.Json;

namespace BotOneOne.Extension.NapCatQQ.Dto;

public class GroupPokeDto
{
    [JsonProperty("group_id")] public long GroupId { get; set; }
    [JsonProperty("user_id")] public long UserId { get; set; }
}