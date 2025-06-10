namespace GameData.src.Shared.Enums
{
    public enum DamageType
    {
        Physical = 1 << 0,
        Elemental = 1 << 1,
        Nature = 1 << 2,
        Bleed = (1 << 0) | (1 << 3),
        Poison = (1 << 2) | (1 << 4),
        Burn = (1 << 1) | (1 << 5),
        TrueDamage = 1 << 6
    }
}