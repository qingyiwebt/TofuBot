
using BotOneOne.Connectivity;
using BotOneOne.Extension.NapCatQQ.Dto;
using BotOneOne.MessageFormat;
using BotOneOne.Protocol.OneBot.Dto;
using BotOneOne.Protocol.OneBot.Model;

namespace BotOneOne.Protocol.OneBot;

public class OneBotV11Context(IConnectionSource connectionSource, BotContextOptions? options = null)
    : BaseOneBotContext(connectionSource, options)
{
    public async Task<long> SendMessage(long targetId, TargetType targetType, Message message)
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

        var response = await InvokeAction<MessageIdDto, SendMessageRequestDto>("send_msg", dto);
        return response.MessageId;
    }

    public Task DeleteMessage(long messageId)
    {
        return InvokeAction("delete_msg", new MessageIdDto { MessageId = messageId });
    }

    public Task<Message?> GetMessage(long messageId)
    {
        return InvokeAction<Message, MessageIdDto>("get_msg", new MessageIdDto { MessageId = messageId });
    }

    public Task<UserInfo[]?> GetFriendList()
    {
        return InvokeAction<UserInfo[], object>("get_friend_list", null);
    }

    public Task<GroupInfo?> GetGroupInfo(long groupId)
    {
        return InvokeAction<GroupInfo, GroupIdDto>("get_group_info", new GroupIdDto { GroupId = groupId });
    }

    public Task<UserInfo?> GetGroupMemberInfo(long groupId, long userId)
    {
        return InvokeAction<UserInfo, UserAndGroupDto>("get_group_member_info", 
            new UserAndGroupDto { GroupId = groupId, UserId = userId });
    }

    public Task<UserInfo[]?> GetGroupMemberList(long groupId)
    {
        return InvokeAction<UserInfo[], GroupIdDto>("get_group_member_list", new GroupIdDto { GroupId = groupId });
    }

    public Task<UserInfo?> GetUserInfo(long userId)
    {
        return InvokeAction<UserInfo, UserIdDto>("get_user_info", new UserIdDto { UserId = userId });
    }
}