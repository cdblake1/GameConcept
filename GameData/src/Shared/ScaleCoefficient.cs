using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifier;

namespace GameData.src.Shared
{
    public sealed record ScaleCoefficient(StatKind Stat, ScalarOperation Operation);
}