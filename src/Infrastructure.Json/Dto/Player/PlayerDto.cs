using GameData.src.Class;

namespace Infrastructure.Json.Player
{
    public sealed record PlayerDto
    {
        public required StatTemplateDto BaseStats { get; init; }

        public required ClassDefinition ClassDefinition { get; init; }
    }
}