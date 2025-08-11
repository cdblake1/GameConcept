using GameData.src.Class;
using GameData.src.Shared;

namespace GameData.src.Player
{
    public sealed record PlayerDefinition(ClassDefinition ClassDefinition, PresentationDefinition Presentation);
}