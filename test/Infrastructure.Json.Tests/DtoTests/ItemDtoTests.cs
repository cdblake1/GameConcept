using GameData.src.Item;
using Infrastructure.Json.Dto.Item;
using Newtonsoft.Json;

namespace Infrastructure.Json.Tests.DtoTests
{
    public class ItemDtoTests
    {
        public static readonly string CraftFilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "test_item.item.json");
        public static readonly string EquipmentFilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "test_equip.item.json");

        [Fact]
        public void CanDeserializeCraftMaterialItemDto()
        {
            var json = File.ReadAllText(CraftFilePath);
            var dto = JsonConvert.DeserializeObject<IItemDto>(json);

            Assert.NotNull(dto);
            var cm = dto as CraftingMaterialItemDto;
            Assert.NotNull(cm);
            Assert.Equal("test_item", cm.id);
            Assert.Equal(ItemRarityDto.uncommon, cm.rarity);
            Assert.Equal("Test Item", cm.presentation.name);
            Assert.Equal("Test Description", cm.presentation.description);
        }

        [Fact]
        public void CanDeserializeEquipmentItemDto()
        {
            var json = File.ReadAllText(EquipmentFilePath);
            var dto = JsonConvert.DeserializeObject<IItemDto>(json);
            Assert.NotNull(dto);

            var equip = dto as EquipmentItemDto;
            Assert.NotNull(equip);

            Assert.Equal("test_equip_item", equip.id);
            Assert.Equal(EquipmentKindDto.body, equip.kind);
            Assert.Equal(ItemRarityDto.epic, equip.rarity);
            Assert.Equal("Test Equip", equip.presentation.name);
            Assert.Equal("Test Equip Description", equip.presentation.description);
        }
    }
}