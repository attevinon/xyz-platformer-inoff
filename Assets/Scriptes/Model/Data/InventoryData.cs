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
        [SerializeField] private int _capacity = 10;

        public event Action<string, int> OnInventoryChanged;

        public bool TryAdd(string id, int value)
        {
            if (IsNoDef(id)) return false; 
            if (value <= 0) return false;

            var item = GetItem(id);
            if(item == null)
            {
                if (_inventory.Count >= _capacity)
                {
                    Debug.Log("Inventory is full");
                    return false;
                }
                item = new InventoryItemData(id);
                _inventory.Add(item);
            }

            item.Value += value;
            OnInventoryChanged?.Invoke(id,Count(id));
            return true;
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
