namespace TofuBot.AI.OpenAI;

public struct ModelOptions
{
    public string Endpoint { get; set; }
    public string ModelName { get; set; }
    public string SecretKey { get; set; }
    public string Usage { get; set; }
}