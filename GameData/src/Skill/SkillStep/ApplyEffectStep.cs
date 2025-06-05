namespace GameData.src.Skill.SkillStep
{
    public sealed record ApplyEffectStep : ISkillStep
    {
        public required string Effect { get; init; }
    }
}