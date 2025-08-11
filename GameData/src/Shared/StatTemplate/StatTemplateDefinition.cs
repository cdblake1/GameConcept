using GameData.src.Shared.Modifiers;

namespace GameData.src.Shared.StatTemplate
{
    public sealed record StatTemplateDefinition(
        string Id,
        IReadOnlyList<GlobalModifier> Global,
        IReadOnlyList<DamageModifier> Damage,
        IReadOnlyList<AttackModifier> Attack,
        IReadOnlyList<WeaponModifier> Weapon
    );
}