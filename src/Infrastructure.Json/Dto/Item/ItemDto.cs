using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Common.Modifiers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Json.Dto.Item
{
    public sealed record EquipmentItemDto(
        [JsonProperty] string id,
        [JsonProperty] ItemRarityDto rarity,
        [JsonProperty] EquipmentKindDto kind,
        [JsonProperty] PresentationDto presentation
    ) : IItemDto
    {
        public ItemTypeDto type => ItemTypeDto.equipment;
    }

    public sealed record CraftingMaterialItemDto(
          [JsonProperty] string id,
          [JsonProperty] ItemRarityDto rarity,
          [JsonProperty] PresentationDto presentation
      ) : IItemDto
    {
        public ItemTypeDto type => ItemTypeDto.crafting_material;
    }

    public sealed record CurrencyDto(
        [JsonProperty] string id,
        [JsonProperty] ItemRarityDto rarity,
        [JsonProperty] PresentationDto presentation,
        [JsonProperty] int min_amount,
        [JsonProperty] int max_amount
    ) : IItemDto
    {
        public ItemTypeDto type => ItemTypeDto.currency;
    }

    public sealed record ConsumableDto(
        [JsonProperty] string id,
        [JsonProperty] ItemRarityDto rarity,
        [JsonProperty] PresentationDto presentation,
        [JsonProperty] IModifierDto[] modifiers
    ) : IItemDto
    {
        public ItemTypeDto type => ItemTypeDto.consumable;
    }

    [JsonConverter(typeof(ItemDtoConverter))]
    public interface IItemDto
    {
        ItemTypeDto type { get; }
    }

    public enum ItemTypeDto
    {
        equipment,
        currency,
        crafting_material,
        consumable,
    }

    public enum ItemRarityDto
    {
        common, uncommon, rare, epic
    }

    public enum EquipmentKindDto
    {
        helmet, ring, necklace, body, legs, boots, gloves, weapon
    }

    public class ItemDtoConverter : JsonConverter<IItemDto>
    {
        public override IItemDto ReadJson(JsonReader reader, Type objectType, IItemDto? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var typeStr = jo["item"]?["type"]?.ToString() ?? throw new JsonSerializationException("Item type is required");

            return typeStr switch
            {
                "equipment" => new EquipmentItemDto(
                    jo["id"]?.ToString() ?? throw new JsonSerializationException("Item id is required"),
                    (ItemRarityDto)Enum.Parse(typeof(ItemRarityDto), jo["rarity"]?.ToString() ?? throw new JsonSerializationException("Item rarity is required")),
                    (EquipmentKindDto)Enum.Parse(typeof(EquipmentKindDto), jo["item"]?["kind"]?.ToString() ?? throw new JsonSerializationException("Equipment kind is required")),
                    jo["presentation"]?.ToObject<PresentationDto>(serializer) ?? throw new JsonSerializationException("Item presentation is required")
                ),
                "currency" => new CurrencyDto(
                    jo["id"]?.ToString() ?? throw new JsonSerializationException("Item id is required"),
                    (ItemRarityDto)Enum.Parse(typeof(ItemRarityDto), jo["rarity"]?.ToString() ?? throw new JsonSerializationException("Item rarity is required")),
                    jo["presentation"]?.ToObject<PresentationDto>(serializer) ?? throw new JsonSerializationException("Item presentation is required"),
                    jo["item"]?["min_amount"]?.Value<int>() ?? throw new JsonSerializationException("Currency min_amount is required"),
                    jo["item"]?["max_amount"]?.Value<int>() ?? throw new JsonSerializationException("Currency max_amount is required")
                ),
                "crafting_material" => new CraftingMaterialItemDto(
                    jo["id"]?.ToString() ?? throw new JsonSerializationException("Item id is required"),
                    (ItemRarityDto)Enum.Parse(typeof(ItemRarityDto), jo["rarity"]?.ToString() ?? throw new JsonSerializationException("Item rarity is required")),
                    jo["presentation"]?.ToObject<PresentationDto>(serializer) ?? throw new JsonSerializationException("Item presentation is required")
                ),
                "consumable" => new ConsumableDto(
                    jo["id"]?.ToString() ?? throw new JsonSerializationException("Item id is required"),
                    (ItemRarityDto)Enum.Parse(typeof(ItemRarityDto), jo["rarity"]?.ToString() ?? throw new JsonSerializationException("Item rarity is required")),
                    jo["presentation"]?.ToObject<PresentationDto>(serializer) ?? throw new JsonSerializationException("Item presentation is required"),
                    jo["item"]?["modifiers"]?.ToObject<IModifierDto[]>(serializer) ?? throw new JsonSerializationException("Consumable modifiers are required")
                ),
                _ => throw new JsonSerializationException($"Unknown item type: {typeStr}")
            };
        }

        public override void WriteJson(JsonWriter writer, IItemDto? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}