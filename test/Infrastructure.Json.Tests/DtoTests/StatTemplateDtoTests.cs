using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Common.Modifiers;
using Newtonsoft.Json;

namespace Infrastructure.Json.Tests.DtoTests
{
    public class StatTemplateDtoTests
    {
        [Fact]
        public void CanDeserializeValidStatTemplateDto()
        {
            // Arrange
            var json = @"{
                ""id"": ""test_stats"",
                ""global"": [{
                    ""stat"": ""health"",
                    ""scalar_op_type"": ""added"",
                    ""value"": 100,
                    ""type"": ""global""
                }],
                ""damage"": [{
                    ""stat"": ""physical"",
                    ""scalar_op_type"": ""added"",
                    ""value"": 100,
                    ""type"":""damage""
                }],
                ""attack"": [],
                ""weapon"": []
            }";

            // Act
            var dto = JsonConvert.DeserializeObject<StatTemplateDto>(json);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal("test_stats", dto.Id);
            Assert.Single(dto.global);
            Assert.Equal(GlobalStatDto.health, dto.global[0].stat);
            Assert.Equal(ScalarOpTypeDto.added, dto.global[0].scalar_op_type);
            Assert.Equal(100, dto.global[0].value);

            Assert.Single(dto.damage);
            Assert.Equal(DamageTypeDto.physical, dto.damage[0].stat);
            Assert.Equal(ScalarOpTypeDto.added, dto.damage[0].scalar_op_type);
            Assert.Equal(100, dto.damage[0].value);

            Assert.Empty(dto.attack);
            Assert.Empty(dto.weapon);
        }

        [Fact]
        public void CanDeserializeStatTemplateDtoWithAllStats()
        {
            // Arrange
            var json = @"{
                ""id"": ""test_stats_full"",
                ""global"": [
                    {""stat"": ""health"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""global""},
                    {""stat"": ""armor"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""global""},
                    {""stat"": ""ward"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""global""},
                    {""stat"": ""avoidance"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""global""},
                    {""stat"": ""crit"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""global""},
                    {""stat"": ""speed"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""global""},
                    {""stat"": ""leech"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""global""}
                ],
                ""damage"": [
                    {""stat"": ""physical"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""damage""},
                    {""stat"": ""elemental"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""damage""},
                    {""stat"": ""nature"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""damage""},
                    {""stat"": ""bleed"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""damage""},
                    {""stat"": ""burn"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""damage""},
                    {""stat"": ""poison"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""damage""},
                    {""stat"": ""true_damage"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""damage""}
                ],
                ""attack"": [
                    {""stat"": ""hit"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""attack""},
                    {""stat"": ""dot"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""attack""}
                ],
                ""weapon"": [
                    {""stat"": ""melee"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""weapon""},
                    {""stat"": ""range"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""weapon""},
                    {""stat"": ""spell"", ""scalar_op_type"": ""added"", ""value"": 100, ""type"": ""weapon""}
                ]
            }";

            // Act
            var dto = JsonConvert.DeserializeObject<StatTemplateDto>(json);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal("test_stats_full", dto.Id);
            Assert.Equal(7, dto.global.Length);
            Assert.Equal(7, dto.damage.Length);
            Assert.Equal(2, dto.attack.Length);
            Assert.Equal(3, dto.weapon.Length);
        }
    }
}