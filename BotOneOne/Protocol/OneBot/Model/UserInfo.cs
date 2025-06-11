using Newtonsoft.Json;

namespace BotOneOne.Protocol.OneBot.Model;

public class UserInfo
{
    /// <summary>
    /// 用户所在群号
    /// </summary>
    [JsonProperty("group_id")]
    public long? GroupId { get; set; }

    /// <summary>
    /// 用户号码
    /// </summary>
    [JsonProperty("user_id")]
    public long UserId { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    [JsonProperty("nickname")]
    public string? Nickname { get; set; }

    /// <summary>
    /// 用户群名片
    /// </summary>
    [JsonProperty("card")]
    public string? Card { get; set; }

    /// <summary>
    /// 用户在群中的职责
    /// </summary>
    [JsonProperty("role")]
    public string? Role { get; set; }

    /// <summary>
    /// 对该用户的备注
    /// </summary>
    [JsonProperty("remark")]
    public string? Remark { get; set; }

    /// <summary>
    /// 该用户的性别
    /// </summary>
    [JsonProperty("sex")]
    public string? Sex { get; set; }

    /// <summary>
    /// 该用户的年龄
    /// </summary>
    [JsonProperty("age")]
    public int? Age { get; set; }
    
    /// <summary>
    /// 该用户在群内的头衔
    /// </summary>
    [JsonProperty("title")]
    public string? Title { get; set; }
    
    /// <summary>
    /// 该用户在群中的等级
    /// </summary>
    [JsonProperty("level")]
    public int? Level { get; set; }
}