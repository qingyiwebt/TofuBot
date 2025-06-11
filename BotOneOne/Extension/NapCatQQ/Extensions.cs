using BotOneOne.Extension.NapCatQQ.Dto;
using BotOneOne.Protocol.OneBot;
using BotOneOne.Protocol.OneBot.Dto;

namespace BotOneOne.Extension.NapCatQQ;

public static class Extensions
{
    /// <summary>
    /// 群签到
    /// </summary>
    /// <param name="context">OneBot上下文</param>
    /// <param name="groupId">群号</param>
    public static Task SetGroupSign(this BaseOneBotContext context, long groupId)
    {
        return context.InvokeAction("set_group_sign", new GroupIdDto { GroupId = groupId });
    }

    /// <summary>
    /// 群聊戳一戳
    /// </summary>
    /// <param name="context">OneBot上下文</param>
    /// <param name="groupId">群号</param>
    /// <param name="userId">对方QQ号</param>
    public static Task GroupPoke(this BaseOneBotContext context, long groupId, long userId)
    {
        return context.InvokeAction("group_poke", new GroupPokeDto { GroupId = groupId, UserId = userId });
    }

    /// <summary>
    /// 私聊戳一戳
    /// </summary>
    /// <param name="context">OneBot上下文</param>
    /// <param name="userId">对方QQ号</param>
    public static Task FriendPoke(this BaseOneBotContext context, long userId)
    {
        return context.InvokeAction("friend_poke", new UserIdDto { UserId = userId });
    }

    /// <summary>
    /// 设置在线状态
    /// </summary>
    /// <param name="context">OneBot上下文</param>
    /// <param name="status">在线状态</param>
    /// <param name="extStatus">扩展状态</param>
    /// <param name="batteryStatus">电量状态</param>
    public static Task SetOnlineStatus(this BaseOneBotContext context, int status, int extStatus = 0, int batteryStatus = 0)
    {
        return context.InvokeAction("set_online_status", new OnlineStatusDto
        {
            Status = status,
            ExtStatus = extStatus,
            BatteryStatus = batteryStatus
        });
    }

    /// <summary>
    /// 设置个人签名
    /// </summary>
    /// <param name="context">OneBot上下文</param>
    /// <param name="longNick">签名内容</param>
    public static Task SetSelfLongNick(this BaseOneBotContext context, string longNick)
    {
        return context.InvokeAction("set_self_longnick", new LongNickDto { LongNick = longNick });
    }

    /// <summary>
    /// 标记所有消息已读
    /// </summary>
    /// <param name="context">OneBot上下文</param>
    public static Task MarkAllAsRead(this BaseOneBotContext context)
    {
        return context.InvokeAction("_mark_all_as_read", null as object);
    }
}