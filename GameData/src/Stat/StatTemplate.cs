using GameData.src.Shared.Enums;

namespace GameData.src.Stat;

public record StatTemplate(Dictionary<StatKind, int> Stats);