namespace TofuBot.Configuration;

public class Config
{
    public List<Provider> Providers { get; set; } = [];
    public List<Model> Models { get; set; } = [];
    public Prompts Prompts { get; set; } = new();
    public Character Character { get; set; } = new();
    public Events EventConfig { get; set; } = new();
}