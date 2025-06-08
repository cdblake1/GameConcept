using GameData.src.Effect.Talent;
using GameData.src.Skill.SkillStep;

namespace GameData.src.Talent.TalentActions
{
  public sealed record AddDotDamageAction(
    string SkillId,
    DotDamageStep DotDamage
  ) : ITalentAction;
}