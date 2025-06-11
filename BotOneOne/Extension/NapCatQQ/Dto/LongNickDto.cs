using Newtonsoft.Json;

namespace BotOneOne.Extension.NapCatQQ.Dto;

public class LongNickDto
{
    [JsonProperty("longNick")] public string LongNick { get; set; } = "";
}