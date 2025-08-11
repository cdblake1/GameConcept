using GameData.src.CraftingRecipe;
using GameData.src.Item;
using GameData.src.Item.Equipment;
using Infrastructure.Json.Dto.CraftingRecipe;
using Infrastructure.Json.Dto.Item;

namespace Infrastructure.Json.Mappers
{
    public static class ItemMappers
    {
        public static IItemDefinition ToDomain(this IItemDto dto)
        {
            return dto switch
            {
                CraftingMaterialItemDto craftingMaterial => ToDomain(craftingMaterial),
                EquipmentItemDto equipment => ToDomain(equipment),
                CurrencyDto currency => ToDomain(currency),
                ConsumableDto consumable => ToDomain(consumable),
                _ => throw new ArgumentException($"Invalid item type: {dto}")
            };
        }

        public static CraftingMaterialDefinition ToDomain(this CraftingMaterialItemDto dto)
        {
            return new CraftingMaterialDefinition(
                dto.id,
                dto.presentation.ToDomain(),
                dto.rarity.ToDomain()
            );
        }

        public static EquipmentDefinition ToDomain(this EquipmentItemDto dto)
        {
            return new EquipmentDefinition(
                dto.id,
                dto.presentation.ToDomain(),
                dto.rarity.ToDomain(),
                dto.kind.ToDomain()
            );
        }

        public static CurrencyDefinition ToDomain(this CurrencyDto dto)
        {
            return new CurrencyDefinition(
                dto.id,
                dto.presentation.ToDomain(),
                dto.rarity.ToDomain(),
                dto.min_amount,
                dto.max_amount
            );
        }

        public static ConsumableDefinition ToDomain(this ConsumableDto dto)
        {
            return new ConsumableDefinition(
                dto.id,
                dto.presentation.ToDomain(),
                dto.rarity.ToDomain(),
                [.. dto.modifiers.Select(m => m.ToDomain())]
            );
        }

        public static ItemRarity ToDomain(this ItemRarityDto dto)
        {
            return dto switch
            {
                ItemRarityDto.common => ItemRarity.Common,
                ItemRarityDto.uncommon => ItemRarity.Uncommon,
                ItemRarityDto.rare => ItemRarity.Rare,
                ItemRarityDto.epic => ItemRarity.Epic,
                _ => throw new ArgumentException($"Invalid item rarity: {dto}")
            };
        }

        public static EquipmentKind ToDomain(this EquipmentKindDto dto)
        {
            return dto switch
            {
                EquipmentKindDto.body => EquipmentKind.Body,
                EquipmentKindDto.boots => EquipmentKind.Boots,
                EquipmentKindDto.gloves => EquipmentKind.Gloves,
                EquipmentKindDto.helmet => EquipmentKind.Helmet,
                EquipmentKindDto.legs => EquipmentKind.Legs,
                EquipmentKindDto.necklace => EquipmentKind.Necklace,
                EquipmentKindDto.ring => EquipmentKind.Ring,
                EquipmentKindDto.weapon => EquipmentKind.Weapon,
                _ => throw new ArgumentException($"Invalid equipment kind: {dto}")
            };
        }

        public static CraftingRecipeDefinition ToDomain(this CraftingRecipeDto dto)
        {
            return new CraftingRecipeDefinition(
                dto.id,
                dto.crafted_item_id,
                [.. dto.materials.Select(i => (i.item_id, i.count))]
            );
        }
    }
}