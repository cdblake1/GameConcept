using GameData.src.Effect.Talent;

namespace GameData.src.Talent.TalentActions
{
    public sealed record ApplyEffectAction(
        string EffectId,
        string? FromSkill,
        bool Global = false) : ITalentAction;
}