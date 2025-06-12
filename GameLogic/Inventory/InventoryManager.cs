using GameData;
using GameData.src.Item;
using static GameData.CraftingMaterial;

namespace GameLogic.Inventory
{
    public class InventoryManager : IStateSerializable<InventoryManager.InventoryManagerDto, InventoryManager>
    {
        private readonly List<Equipment> _equipment = new();
        private readonly List<CraftingMaterial> _materials = new();

        public IReadOnlyList<Equipment> Equipment => _equipment;
        public IReadOnlyList<CraftingMaterial> CraftingMaterials => _materials;

        public IReadOnlyList<Equipment> GetItemsOfKind(EquipmentKind kind) =>
            _equipment.Where(item => item.Kind == kind).ToList();

        public bool AddItem(IItem item)
        {
            switch (item)
            {
                case Equipment eq:
                    _equipment.Add(eq);
                    return true;
                case CraftingMaterial mat:
                    return AddOrMergeMaterial(mat);
                default:
                    return false;
            }
        }

        private bool AddOrMergeMaterial(CraftingMaterial newMat)
        {
            var existing = _materials.FirstOrDefault(m => m.Name == newMat.Name);
            if (existing is not null)
            {
                existing.Count += newMat.Count;
            }
            else
            {
                _materials.Add(newMat);
            }
            return true;
        }

        public bool RemoveItem(IItem item)
        {
            return item switch
            {
                Equipment eq => _equipment.Remove(eq),
                CraftingMaterial mat => TryRemoveMaterial(mat),
                _ => false
            };
        }

        private bool TryRemoveMaterial(CraftingMaterial mat)
        {
            var existing = _materials.FirstOrDefault(m => m.Name == mat.Name);
            if (existing is null || existing.Count < mat.Count)
                return false;

            existing.Count -= mat.Count;
            if (existing.Count == 0)
            {
                _materials.Remove(existing);
            }

            return true;
        }

        public bool HasAmount(IItem item) =>
            item switch
            {
                Equipment eq => _equipment.Contains(eq),
                CraftingMaterial mat => _materials.Any(m => m.Name == mat.Name && m.Count >= mat.Count),
                _ => false
            };

        public int GetAmount(IItem item) =>
            item switch
            {
                Equipment eq => _equipment.Count(e => e.Name == eq.Name),
                CraftingMaterial mat => _materials.FirstOrDefault(m => m.Name == mat.Name)?.Count ?? 0,
                _ => 0
            };

        public readonly struct InventoryManagerDto
        {
            public List<EquipmentDto> Equipment { get; init; }
            public List<CraftingMaterialDto> CraftingMaterials { get; init; }
        }

        public InventoryManagerDto Serialize()
        {
            return new InventoryManagerDto
            {
                Equipment = [.. _equipment.Select(e => e.Serialize())],
                CraftingMaterials = [.. _materials.Select(m => m.Serialize())]
            };
        }

        public static InventoryManager Restore(InventoryManagerDto dto)
        {
            var manager = new InventoryManager();
            foreach (var item in dto.Equipment)
            {
                manager._equipment.Add(GameData.Equipment.Restore(item));
            }

            foreach (var item in dto.CraftingMaterials)
            {
                manager._materials.Add(CraftingMaterial.Restore(item));
            }

            return manager;
        }
    }
}