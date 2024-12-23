using PixelCrew.Model.Data;
using System;
using UnityEngine;

namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;

        public int Health;
        public InventoryData Inventory => _inventory;
        public PlayerData Clone()
        {
            var jsonClone = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(jsonClone);
        }
    }
}

