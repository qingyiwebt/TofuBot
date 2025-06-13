namespace TofuBot.Abilities;

public class NoOperateAbility : IAbility
{
    public string Name => Constants.DoNothingAbility;
    public string Description => Constants.DoNothingAbilityDesc;
}