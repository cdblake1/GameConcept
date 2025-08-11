using GameData.src.Shared.Enums;
using Infrastructure.Json.Dto.Common.Modifiers;
using Infrastructure.Json.Dto.Effect;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Common.Operations
{
    public abstract record CollectionOperationBaseDto<T>
    {
        [JsonProperty(nameof(type), Required = Required.Always)]
        public Kind type { get; init; }

        [JsonProperty(nameof(operation), Required = Required.Always)]
        public CollectionOperationDto operation { get; }

        [JsonProperty(nameof(items), Required = Required.Always)]
        public T[] items { get; }

        public CollectionOperationBaseDto(Kind type, CollectionOperationDto operation, T[] items)
        {
            this.type = type;
            this.operation = operation;
            this.items = items;
        }

        public enum Kind { damage_type, attack_type, stat, modifier, id, apply_status }
    }

    public enum CollectionOperationDto { add, set, remove, clear }

    public sealed record StatCollectionOperationDto : CollectionOperationBaseDto<GlobalStatDto>
    {
        public StatCollectionOperationDto(CollectionOperationDto operation, GlobalStatDto[] items) : base(Kind.stat, operation, items)
        {
        }
    }

    public sealed record DamageTypeCollectionOperationDto : CollectionOperationBaseDto<DamageTypeDto>
    {
        public DamageTypeCollectionOperationDto(CollectionOperationDto operation, DamageTypeDto[] items) : base(Kind.damage_type, operation, items)
        {
        }
    }

    public sealed record AttackCollectionOperationDto : CollectionOperationBaseDto<AttackTypeDto>
    {
        public AttackCollectionOperationDto(CollectionOperationDto operation, AttackTypeDto[] items) : base(Kind.attack_type, operation, items)
        {
        }
    }

    public sealed record ModifierCollectionOperationDto : CollectionOperationBaseDto<IModifierDto>
    {
        public ModifierCollectionOperationDto(CollectionOperationDto operation, IModifierDto[] items) : base(Kind.modifier, operation, items)
        {
        }
    }

    public sealed record StatusCollectionOperationDto : CollectionOperationBaseDto<StatusBaseDto>
    {
        public StatusCollectionOperationDto(CollectionOperationDto operation, StatusBaseDto[] items) : base(Kind.apply_status, operation, items)
        {
        }
    }
}