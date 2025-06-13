namespace TofuBot.AI.OpenAI;

public class ModelOptions
{
    public string Endpoint { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string Usage { get; set; } = string.Empty;
    public float? Temperature { get; set; }
    public float? TopP { get; set; }
}