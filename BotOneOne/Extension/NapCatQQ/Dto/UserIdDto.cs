using Newtonsoft.Json;

namespace BotOneOne.Extension.NapCatQQ.Dto;

public class UserIdDto
{
    [JsonProperty("user_id")] public long UserId { get; set; }
}