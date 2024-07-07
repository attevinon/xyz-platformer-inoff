using System;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Model.Definitions;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public void Add(string id, int value)
        {
            if (IsNoDef(id)) return; 
            if (value <= 0) return;

            var item = GetItem(id);
            if(item == null)
            {
                var newItem = new InventoryItemData(id);
                _inventory.Add(newItem);
            }
            item.Value += value;
        }

        public void Remove(string id, int value)
        {
            if (IsNoDef(id)) return;
            var item = GetItem(id);
            if (item == null) return;

            item.Value -= value;

            if(item.Value <= 0)
                _inventory.Remove(item);
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
                Debug.Log($"Definition for item with id={id} is not found");

            return itemDef.IsVoid;
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        public string Id;
        public int Value;

        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}
