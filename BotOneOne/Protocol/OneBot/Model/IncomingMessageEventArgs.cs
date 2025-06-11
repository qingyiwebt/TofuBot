using BotOneOne.MessageFormat;
using Newtonsoft.Json;

namespace BotOneOne.Protocol.OneBot.Model;

public class IncomingMessageEventArgs
{ 
    public UserInfo Sender { get; set; } = new();

    public Message Message { get; set; } = Message.Empty;
    
    public long MessageId { get; set; }
    
    public DateTimeOffset Time { get; set; }
    
    public TargetType Type { get; set; }
}