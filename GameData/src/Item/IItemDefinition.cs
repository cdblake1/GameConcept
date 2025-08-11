using GameData.src.Shared;

namespace GameData.src.Item
{
    public interface IItemDefinition
    {
        public string Id { get; }
        public PresentationDefinition Presentation { get; }
        public ItemRarity Rarity { get; }
    }
}