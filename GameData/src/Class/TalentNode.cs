namespace GameData.src.Class
{
    public sealed record TalentNode(string Id, TalentTier Tier, IReadOnlyList<string> Prerequisites);
}