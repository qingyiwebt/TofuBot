using Newtonsoft.Json;

namespace BotOneOne.Protocol.OneBot.Model;

public class GroupInfo
{
    [JsonProperty("group_id")] public long GroupId { get; set; }
    [JsonProperty("group_name")] public string GroupName { get; set; } = "";
    [JsonProperty("member_count")] public int MemberCount { get; set; }
    [JsonProperty("max_member_count")] public int MaxMemberCount { get; set; }
}
