using Newtonsoft.Json;

namespace BotOneOne.Protocol.OneBot.Dto;

public struct MessageIdDto
{
    [JsonProperty("message_id")] public long MessageId { get; set; }
}