using Newtonsoft.Json;

namespace BotOneOne.Extension.NapCatQQ.Dto;

public class OnlineStatusDto
{
    [JsonProperty("status")] public int Status { get; set; }
    [JsonProperty("ext_status")] public int ExtStatus { get; set; }
    [JsonProperty("battery_status")] public int BatteryStatus { get; set; }
}