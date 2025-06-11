using Newtonsoft.Json;

namespace BotOneOne.MessageFormat;

public class ReplyMessageSegment : MessageSegment<ReplyMessageSegment.Payload>
{
    public override string Type => "reply";
    
    public override string ToString()
    {
        return $"[Reply {Data.MessageId}]";
    }

    public ReplyMessageSegment(long content)
    {
        Data = new Payload { MessageId = content };
    }

    public struct Payload
    {
        [JsonProperty("id")] public long MessageId { get; set; }
    }
}