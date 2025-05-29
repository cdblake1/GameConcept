using GameData;

public static class CraftedEquipmentTemplates
{
    public class IronHelmet : Equipment
    {
        public IronHelmet() : base("IronHelmet", "Crafted iron helmet", GoldCoin.FromAmount(80), new()
        {
            AttackPower = 0,
            Defense = 5,
            Health = 0,
            Speed = 0
        }, EquipmentKind.HeadArmor, ItemRarity.Uncommon)
        {

        }
    }

    public class IronChest : Equipment
    {
        public IronChest() : base("IronChest", "Crafted iron chest", GoldCoin.FromAmount(150), new()
        {
            AttackPower = 0,
            Defense = 5,
            Health = 0,
            Speed = 0
        }, EquipmentKind.BodyArmor, ItemRarity.Uncommon)
        {
        }
    }

    public class IronSword : Equipment
    {
        public IronSword() : base("IronSword", "Crafted iron sword", GoldCoin.FromAmount(160), new()
        {
            AttackPower = 5,
            Defense = 0,
            Health = 0,
            Speed = 0
        }, EquipmentKind.Weapon, ItemRarity.Uncommon)
        {

        }
    }

    public class IronLegs : Equipment
    {
        public IronLegs() : base("IronLegs", "Crafted iron legs", GoldCoin.FromAmount(110), new()
        {
            AttackPower = 0,
            Defense = 5,
            Health = 0,
            Speed = 0
        }, EquipmentKind.LegArmor, ItemRarity.Uncommon)
        {

        }
    }
}
