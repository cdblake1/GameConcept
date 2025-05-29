using GameData;
using static CraftingMaterialTemplates;

public static class LootTableConcretes
{
    public static LootTable LowLevelLootTable => new([
        new(IronScrap.FromRange(1,2), 33),
        new(WoodenShoot.FromRange(1,2), 33),
        new(IronScrap.FromRange(1,2), 33),
        new(TatteredCloth.FromRange(1,2), 33),
        new(ItemTemplates.ChestOfMight, 1),
        new(ItemTemplates.HelmOfValor, 1),
        new(ItemTemplates.LegsOfMight, 1),
        new(ItemTemplates.SwordOfMight, 1),
    ]);

    public static LootTable LowLevelBossLootTable => new([
        new(IronScrap.FromRange(2,4), 25),
        new(WoodenShoot.FromRange(2,4), 25),
        new(TatteredCloth.FromRange(2,4), 25),
        new(ItemTemplates.ChestOfMight, 10),
        new(ItemTemplates.HelmOfValor, 10),
        new(ItemTemplates.LegsOfMight, 10),
        new(ItemTemplates.SwordOfMight, 10),
    ], alwaysDropLoot: true);
}