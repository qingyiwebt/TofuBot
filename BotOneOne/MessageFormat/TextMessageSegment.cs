using Newtonsoft.Json;

namespace BotOneOne.MessageFormat;

public class TextMessageSegment : MessageSegment<TextMessageSegment.Payload>
{
    public override string Type => "text";
    public override string ToString()
    {
        return Data.Text;
    }

    public TextMessageSegment(string content)
    {
        Data = new Payload { Text = content };
    }

    public struct Payload
    {
        [JsonProperty("text")] public string Text { get; set; }
    }
}