using GameData.src.Shared;
using Newtonsoft.Json;

namespace GameData.src.Item
{
    public interface IItem
    {
        public string Id { get; }
        public PresentationDefinition Presentation { get; }
    }
}