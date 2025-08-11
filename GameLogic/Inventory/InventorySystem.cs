using GameData.src.Item;
using GameData.src.Item.Equipment;

namespace GameLogic.Inventory
{
    public class InventorySystem
    {
        private readonly List<EquipmentDefinition> equipment;
        private readonly List<CraftingMaterialDefinition> craftingMaterials;
        private readonly List<ConsumableDefinition> consumables;
        private readonly Dictionary<EquipmentKind, EquipmentDefinition> equippedItems;

        public InventorySystem()
        {
            this.equipment = new();
            this.craftingMaterials = new();
            this.consumables = new();
            this.equippedItems = new();
        }

        public void AddItem(IItemDefinition item)
        {
            if (item is EquipmentDefinition equipment)
            {
                this.equipment.Add(equipment);
            }
            else if (item is CraftingMaterialDefinition craftingMaterial)
            {
                this.craftingMaterials.Add(craftingMaterial);
            }
            else if (item is ConsumableDefinition consumable)
            {
                this.consumables.Add(consumable);
            }
            else
            {
                throw new ArgumentException("Invalid item type");
            }
        }

        public void RemoveItem(IItemDefinition item)
        {
            if (item is EquipmentDefinition equipment)
            {
                this.equipment.Remove(equipment);
            }
            else if (item is CraftingMaterialDefinition craftingMaterial)
            {
                this.craftingMaterials.Remove(craftingMaterial);
            }
            else if (item is ConsumableDefinition consumable)
            {
                this.consumables.Remove(consumable);
            }
            else
            {
                throw new ArgumentException("Invalid item type");
            }
        }

        public void EquipItem(EquipmentDefinition item)
        {
            if (this.equippedItems.TryGetValue(item.Kind, out var existingItem))
            {
                this.AddItem(existingItem);
            }

            this.equippedItems[item.Kind] = item;
        }

        public void UnequipItem(EquipmentKind kind)
        {
            if (this.equippedItems.TryGetValue(kind, out var item))
            {
                this.AddItem(item);
            }

            this.equippedItems.Remove(kind);
        }

        public List<EquipmentDefinition> GetAllEquipment()
        {
            return this.equipment;
        }

        public List<CraftingMaterialDefinition> GetAllCraftingMaterials()
        {
            return this.craftingMaterials;
        }

        public List<CraftingMaterialDefinition> CraftingMaterials => this.craftingMaterials;

        public List<EquipmentDefinition> Equipment => this.equipment;

        public List<ConsumableDefinition> Consumables => this.consumables;

        public Dictionary<EquipmentKind, EquipmentDefinition> GetEquippedItems()
        {
            return this.equippedItems;
        }

        public List<EquipmentDefinition> GetItemsOfKind(EquipmentKind kind)
        {
            return this.equipment.Where(e => e.Kind == kind).ToList();
        }
    }
}