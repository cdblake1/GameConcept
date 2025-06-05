using GameData.src.Effect.Talent;

namespace GameData.src.Talent.TalentActions
{
    public class ApplyEffectAction : ITalentAction
    {
        public required string FromSkill { get; init; }

        public required string Effect { get; init; }
    }
}