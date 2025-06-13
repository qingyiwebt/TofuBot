using OpenAI.Chat;
using TofuBot.Abilities;

namespace TofuBot;

public class AbilitiesManager
{
    public static readonly NoOperateAbility NoOperateAbility = new();
    public List<IAbility> AbilitiesList { get; } = [];
    private List<ChatTool>? _abilityTools;

    public void Add(IAbility ability)
    {
        AbilitiesList.Add(ability);
    }
    
    public void Remove(Type abilityType)
    {
        AbilitiesList.RemoveAll(abilityType.IsInstanceOfType);
    }

    /// <summary>
    /// If you have a <see cref="CharacterContext"/>, call Reset() on it instead of calling this
    /// </summary>
    public void ResetAbilityTools()
    {
        _abilityTools = null;
    }

    public List<ChatTool> GetAbilityTools()
    {
        if (_abilityTools != null)
        {
            return _abilityTools;
        }

        _abilityTools = [];
        
        foreach (var ability in AbilitiesList)
        {
            _abilityTools.Add(ChatTool.CreateFunctionTool(
                functionName: ability.Name,
                functionDescription: ability.Description));
        }
        AddInternalTools(_abilityTools);

        return _abilityTools;
    }

    public IAbility ResolveAbility(string funcName)
    {
        if (funcName == NoOperateAbility.Name)
        {
            return NoOperateAbility;
        }

        return AbilitiesList.First(x => x.Name == funcName);
    }

    private static void AddInternalTools(List<ChatTool> tools)
    {
        tools.Add(ChatTool.CreateFunctionTool(
            functionName: NoOperateAbility.Name,
            functionDescription: NoOperateAbility.Description));
    }
}