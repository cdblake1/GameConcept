namespace GameData.src.Talent.TalentActions
{
    public sealed record ApplyEffectAction(
        string EffectId,
        string? SkillId,
        bool Global = false) : ITalentAction;
}