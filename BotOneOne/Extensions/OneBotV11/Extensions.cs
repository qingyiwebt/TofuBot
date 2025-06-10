using BotOneOne.MessageFormat;
using Newtonsoft.Json;

namespace BotOneOne.Extensions.OneBotV11;

public static class Extensions
{
    public static OneBotContext UseOneBotV11(this OneBotContext context)
    {
        return context;
    }
    
    public static async Task<long> SendMessage(this OneBotContext context, long targetId, TargetType targetType,
        Message message)
    {
        var dto = new SendMessageRequestDto();
        if (targetType == TargetType.User)
        {
            dto.UserId = targetId;
        }
        else
        {
            dto.GroupId = targetId;
        }

        dto.MessageSegments = message.Segments;

        var response = await context.InvokeAction<MessageIdDto, SendMessageRequestDto>("send_msg", dto);
        return response.MessageId;
    }

    public static Task DeleteMessage(this OneBotContext context, long messageId)
    {
        return context.InvokeAction("delete_msg", new MessageIdDto { MessageId = messageId });
    }

    private struct SendMessageRequestDto
    {
        [JsonProperty("user_id")] public long? UserId { get; set; }
        [JsonProperty("group_id")] public long? GroupId { get; set; }
        [JsonProperty("message")] public List<MessageSegment> MessageSegments { get; set; }
    }

    private struct MessageIdDto
    {
        [JsonProperty("message_id")] public long MessageId { get; set; }
    }
}