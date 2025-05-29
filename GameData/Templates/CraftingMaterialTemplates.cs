using GameData;

public static class CraftingMaterialTemplates
{
    public class IronScrap : CraftingMaterial
    {
        static IronScrap()
        {
            Register<IronScrap>(amount => new IronScrap() { Count = amount });
        }

        public IronScrap() : base("Iron Scrap", "A piece of iron scrap.", GoldCoin.FromAmount(10), ItemRarity.Uncommon)
        {
        }

        public static IronScrap FromRange(int min, int max)
        {
            return (IronScrap)FromRange<IronScrap>(min, max);
        }

        public static IronScrap FromAmount(int amount)
        {
            return (IronScrap)FromAmount<IronScrap>(amount);
        }
    }

    public class WoodenShoot : CraftingMaterial
    {
        static WoodenShoot()
        {
            Register<WoodenShoot>(amount => new WoodenShoot() { Count = amount });
        }
        public WoodenShoot() : base("Wooden Shoot", "A piece of wooden shoot.", GoldCoin.FromAmount(10), ItemRarity.Uncommon)
        {
        }

        public static WoodenShoot FromRange(int min, int max)
        {
            return (WoodenShoot)FromRange<WoodenShoot>(min, max);
        }

        public static WoodenShoot FromAmount(int amount)
        {
            return (WoodenShoot)FromAmount<WoodenShoot>(amount);
        }
    }

    public class TatteredCloth : CraftingMaterial
    {
        static TatteredCloth()
        {
            Register<TatteredCloth>(amount => new TatteredCloth() { Count = amount });
        }

        public TatteredCloth() : base("Tattered Cloth", "A piece of tattered cloth.", GoldCoin.FromAmount(10), ItemRarity.Uncommon)
        {
        }

        public static TatteredCloth FromRange(int min, int max)
        {
            return (TatteredCloth)FromRange<TatteredCloth>(min, max);
        }

        public static TatteredCloth FromAmount(int amount)
        {
            return (TatteredCloth)FromAmount<TatteredCloth>(amount);
        }
    }
}
