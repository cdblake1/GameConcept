using GameData.src.Skill.SkillStep;

namespace GameData.src.Talent.TalentActions
{
  public sealed record AddHitDamageAction(
        string SkillId,
        HitDamageStep HitDamage
      ) : ITalentAction;
}