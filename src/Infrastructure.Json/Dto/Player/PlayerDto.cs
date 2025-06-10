using GameData.src.Class;
using GameData.src.Shared.Enums;

namespace Infrastructure.Json.Dto.Player
{
    public sealed record PlayerDto(
        float[] BaseStats,
        ClassDefinition ClassDefinition);
}