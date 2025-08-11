using GameData.src.Item;
using GameData.src.Item.Equipment;
using GameData.src.Shared.Modifiers;
using GameData.src.Shared.Modifiers.Operations;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Common.Modifiers;
using Infrastructure.Json.Dto.Item;
using Infrastructure.Json.Mappers;

namespace Infrastructure.Json.Tests.MappingTests
{
    public class ItemMapperTests
    {
        [Fact]
        public void Map_CraftingMaterialItem_To_CraftingMaterialItemDto()
        {
            // Arrange
            var item = new CraftingMaterialItemDto(
                id: "test",
                rarity: ItemRarityDto.common,
                presentation: new PresentationDto
                {
                    name = "test",
                    description = "test",
                    icon = "test"
                }
            );

            // Act
            var result = item.ToDomain();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test", result.Id);
            Assert.Equal(ItemRarity.Common, result.Rarity);
            Assert.Equal("test", result.Presentation.Name);
            Assert.Equal("test", result.Presentation.Description);
            Assert.Equal("test", result.Presentation.Icon);
        }

        [Fact]
        public void Map_EquipmentItem_To_EquipmentItemDto()
        {
            // Arrange
            var item = new EquipmentItemDto(
                id: "test",
                rarity: ItemRarityDto.common,
                kind: EquipmentKindDto.weapon,
                presentation: new PresentationDto
                {
                    name = "test",
                    description = "test",
                    icon = "test"
                }
            );

            // Act
            var result = item.ToDomain();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test", result.Id);
            Assert.Equal(ItemRarity.Common, result.Rarity);
            Assert.Equal(EquipmentKind.Weapon, result.Kind);
            Assert.Equal("test", result.Presentation.Name);
            Assert.Equal("test", result.Presentation.Description);
            Assert.Equal("test", result.Presentation.Icon);
        }

        [Fact]
        public void Map_ConsumableItem_To_ConsumableItemDto()
        {
            // Arrange
            var item = new ConsumableDto(
                id: "test",
                rarity: ItemRarityDto.common,
                presentation: new PresentationDto
                {
                    name = "test",
                    description = "test",
                    icon = "test"
                },
                modifiers: [ new SkillModifierDto(
                skill_id: "test",
                scalar_op_type: ScalarOpTypeDto.added,
                value: 10)
                ]
            );

            // Act
            var result = item.ToDomain();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test", result.Id);
            Assert.Equal(ItemRarity.Common, result.Rarity);
            Assert.Equal("test", result.Presentation.Name);
            Assert.Equal("test", result.Presentation.Description);
            Assert.Equal("test", result.Presentation.Icon);
            Assert.Single(result.Modifiers);
            var skillMod = (SkillModifier)result.Modifiers[0];
            Assert.IsType<SkillModifier>(skillMod);
            Assert.Equal("test", skillMod.SkillId);
            Assert.Equal(ScalarOpType.Additive, skillMod.Operation);
            Assert.Equal(10, skillMod.Value);
        }
    }
}