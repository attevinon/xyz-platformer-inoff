using System;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Model.Definitions;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public event Action<string, int> OnInventoryChanged;

        public void Add(string id, int value)
        {
            if (IsNoDef(id)) return; 
            if (value <= 0) return;

            var item = GetItem(id);
            if(item == null)
            {
                item = new InventoryItemData(id);
                _inventory.Add(item);
            }
            item.Value += value;

            OnInventoryChanged?.Invoke(id,Count(id));
        }

        public void Remove(string id, int value)
        {
            if (IsNoDef(id)) return;
            var item = GetItem(id);
            if (item == null) return;

            item.Value -= value;

            if(item.Value <= 0)
                _inventory.Remove(item);

            OnInventoryChanged?.Invoke(id, Count(id));
        }
        
        public int Count(string id)
        {
            int count = 0;
            foreach (var item in _inventory)
            {
                if(item.Id == id)
                    count += item.Value;
            }
            return count;  
        }

        public bool TryUseItem(string id)
        {
            if (IsNoDef(id)) return false;
            var itemDef = DefsFacade.I.ItemsDef.Get(id);

            if(itemDef.OnUse == null)
            {
                Debug.LogWarning($"{id} use is unspecified");
                return false;
            }

            itemDef.OnUse.Invoke();
            return true;
        }

        private InventoryItemData GetItem(string id)
        {
            foreach (var item in _inventory)
            { 
                if(item.Id == id) return item;
            }

            return null;
        }

        private bool IsNoDef(string id)
        {
            var itemDef = DefsFacade.I.ItemsDef.Get(id);
            if (itemDef.IsVoid)
                Debug.LogWarning($"Definition for item with id={id} is not found");

            return itemDef.IsVoid;
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        [InventoryId] public string Id;
        public int Value;

        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}
