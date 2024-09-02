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

        public bool TryAdd(string id, int value)
        {
            if (_inventory.Count >= DefsFacade.I.PlayerDef.InventorySize)
            {
                Debug.Log("Inventory is full");
                return false;
            }
            if (IsNoDef(id)) return false;
            if (value <= 0) return false;

            var itemDef = DefsFacade.I.ItemsDef.Get(id);
            if (itemDef.IsStackable)
            {
                AddStackable(id, value);
            }
            else
            {
                AddNonStackable(id, value);
            }

            OnInventoryChanged?.Invoke(id,Count(id));
            return true;
        }

        private void AddStackable(string id, int value)
        {
            var item = GetItem(id);
            if (item == null)
            {
                item = new InventoryItemData(id);
                _inventory.Add(item);
            }
            item.Value += value;
        }
        private void AddNonStackable (string id, int value)
        {
            int slotsLeft = DefsFacade.I.PlayerDef.InventorySize - _inventory.Count;
            int ableToAdd = Mathf.Min(slotsLeft, value);
            for (int i = 0; i < ableToAdd; i++)
            {  
                _inventory.Add(new InventoryItemData(id));
            }

            if(ableToAdd < value)
                Debug.Log("Inventory is full");
        }

        public void Remove(string id, int value)
        {
            if (IsNoDef(id)) return;

            bool isSucces = DefsFacade.I.ItemsDef.Get(id).IsStackable ?
                TryRemoveStackable(id, value) : TryRemoveNonStackable(id, value);
            if(isSucces)
                OnInventoryChanged?.Invoke(id, Count(id));
        }

        private bool TryRemoveStackable(string id, int value)
        {
            var item = GetItem(id);
            if (item == null) return false;
            item.Value -= value;
            if (item.Value <= 0)
                _inventory.Remove(item);

            return true;
        }

        private bool TryRemoveNonStackable(string id, int value)
        {
            var items = GetItems(id);
            if(items.Length == 0) return false;
            int itemsToRemove = Mathf.Min(items.Length, value);
            for (int i = 0; i < itemsToRemove; i++)
            {
                _inventory.Remove(items[i]);
            }
            return true;
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

        private InventoryItemData[] GetItems(string id)
        {
            var items = new List<InventoryItemData>();
            foreach (var item in _inventory)
            {
                if (item.Id == id) items.Add(item);
            }
            return items?.ToArray();
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
