namespace TofuBot.Configuration;

public class Model
{
    public string ModelName { get; set; } = string.Empty;
    public string Usage { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public float? Temperature { get; set; }
    public float? TopP { get; set; }
}
