using System.Globalization;
using System.Text;
using OpenAI.Chat;
using TofuBot.Abilities;
using TofuBot.AI.OpenAI;

namespace TofuBot;

public class CharacterContext
{
    public AbilitiesManager AbilitiesManager { get; } = new();
    public string Name { get; set; } = Constants.DefaultCharacterName;
    public string Description { get; set; } = Constants.DefaultCharacterDesc;
    public string PurposeGeneratePrompt { get; set; } = string.Empty;
    public RealtimeStatus Status { get; set; } = new();

    private readonly ModelPool _modelPool;
    private ChatCompletionOptions? _purposeGenerateOptions;
    private string? _descriptionOfCharacter;

    public CharacterContext(ModelPool modelPool)
    {
        _modelPool = modelPool;
    }

    public void Reset()
    {
        AbilitiesManager.ResetAbilityTools();
        _purposeGenerateOptions = null;
    }

    public ChatCompletionOptions GetPurposeGenerateOptions()
    {
        if (_purposeGenerateOptions != null)
        {
            return _purposeGenerateOptions;
        }

        var options = _modelPool.GetModelOptions(Constants.ModelUsagePurposeGenerate);
        _purposeGenerateOptions = new ChatCompletionOptions();
        AbilitiesManager.GetAbilityTools()
            .ForEach(_purposeGenerateOptions.Tools.Add);
        _purposeGenerateOptions.Temperature = options.Temperature;
        _purposeGenerateOptions.TopP = options.TopP;

        return _purposeGenerateOptions;
    }

    public string GetCharacterDescPrompt()
    {
        if (_descriptionOfCharacter != null)
        {
            return _descriptionOfCharacter;
        }

        _descriptionOfCharacter =
            $"Name: {Name}\n" +
            $"Description: {Description}";

        return _descriptionOfCharacter;
    }

    public async Task<IAbility> PurposeGenerate()
    {
        var prompt =
            new StringBuilder()
                .AppendLine(PurposeGeneratePrompt)
                .AppendLine(GetCharacterDescPrompt())
                .AppendLine(
                    $"Time: {DateTime.Now.ToString(CultureInfo.InvariantCulture)} (UnixTimestamp: {DateTimeOffset.UtcNow.ToUnixTimeSeconds()})")
                .AppendLine($"Emotion: {Status.Emotion}")
                .ToString();

        var client = _modelPool.NewChatClient(Constants.ModelUsagePurposeGenerate);
        var result = await client.CompleteChatAsync([
            new SystemChatMessage(prompt)
        ], GetPurposeGenerateOptions());

        var returnValue = result.Value;

        if (returnValue.FinishReason == ChatFinishReason.Stop)
        {
            return AbilitiesManager.NoOperateAbility;
        }
        
        return result.Value.FinishReason != ChatFinishReason.ToolCalls
            ? throw new Exception($"Chat finish with {result.Value.FinishReason.ToString()}")
            : AbilitiesManager.ResolveAbility(result.Value.ToolCalls.First().FunctionName);
    }
    
    public class RealtimeStatus
    {
        public string Emotion { get; set; } = "Peace";
    }
}
