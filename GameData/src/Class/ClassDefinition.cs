namespace GameData.src.Class
{
    public record ClassDefinition(
        string Id,
        IReadOnlyList<TalentNode> Talents,
        IReadOnlyList<SkillEntry> SkillEntries);
}