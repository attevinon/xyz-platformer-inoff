﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/ItemsDefinition", fileName = "ItemsDefinition")]
    public class InventoryItemsDef : ScriptableObject
    {
        [SerializeField] private ItemDef[] _items;

        public ItemDef Get(string id)
        {
            foreach(var itemDef in _items)
            {
                if(itemDef.Id == id)
                    return itemDef;
            }
            return default;
        }

#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _items;
#endif

    }

    [Serializable]
    public struct ItemDef
    {
        [SerializeField] private string _id;
        [SerializeField] private bool _isStackable;
        [SerializeField] public UnityEvent OnUse;
        public string Id => _id;
        public bool IsStackable => _isStackable;
        public bool IsVoid => string.IsNullOrEmpty(_id);
    }
}