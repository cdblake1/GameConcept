namespace GameDataLayer
{
    static class ItemTemplates
    {
        public static Item HelmOfValor => new Item(
            "Helm of Valor",
            "A sturdy helm that provides excellent protection.",
            new StatTemplate
            {
                AttackPower = 0,
                Defense = 5,
                Health = 0
            },
            ItemKind.HeadArmor);

        public static Item SwordOfMight => new Item(
            "Sword of Might",
            "A powerful sword that increases your attack power.",
            new StatTemplate
            {
                AttackPower = 10,
                Defense = 0,
                Health = 0
            },
            ItemKind.Weapon);

        public static Item LegsOfMight => new Item(
            "Legs of Might",
            "A powerful legs that increases your attack power.",
            new StatTemplate
            {
                AttackPower = 5,
                Defense = 5,
                Health = 0
            },
            ItemKind.LegArmor);
        
        public static Item ChestOfMight => new Item(
            "Chest of Might",
            "A powerful chest that increases your attack power.",
            new StatTemplate
            {
                AttackPower = 5,
                Defense = 5,
                Health = 0
            },
            ItemKind.BodyArmor);
    }
}