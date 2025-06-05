using GameData.src.Effect.Status;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifiers.Operations;

namespace GameData.src.Shared.Modifiers
{
    public abstract record CollectionModifierBase<T>(CollectionOperation<T> Operation);
    public sealed record DamageTypeCollectionModifier(CollectionOperation<DamageType> Operation) : CollectionModifierBase<DamageType>(Operation);
    public sealed record AttackKindCollectionModifier(CollectionOperation<AttackKind> Operation) : CollectionModifierBase<AttackKind>(Operation);
    public sealed record StatKindCollectionModifier(CollectionOperation<StatKind> Operation) : CollectionModifierBase<StatKind>(Operation);
    public sealed record ModifierCollectionModifier(CollectionOperation<ScalarModifierBase> Operation) : CollectionModifierBase<ScalarModifierBase>(Operation);
    public sealed record StatusCollectionModifier(CollectionOperation<IStatus> Operation) : CollectionModifierBase<IStatus>(Operation);
}