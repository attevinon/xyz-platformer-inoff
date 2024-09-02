using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Model;
using PixelCrew.Model.Data;

public class CollectorComponent : MonoBehaviour, ICanAddInInventory
{
    [SerializeField] private List<InventoryItemData> _items = new List<InventoryItemData>();
    private GameSession _gameSession;

    public bool TryAddInInventory(string id, int value)
    {
        _items.Add(new InventoryItemData(id) { Value = value });
        return true;
    }


    public void DropInPlayerInventory()
    {
        if (_gameSession == null) _gameSession = FindObjectOfType<GameSession>();

        var itemsToDrop = _items.ToArray();
        foreach (InventoryItemData item in itemsToDrop)
        {
            bool isSucces = _gameSession.Data.Inventory.TryAdd(item.Id, item.Value);
            if (!isSucces) return;
            _items.Remove(item);
        }
        _items.Clear();
    }
}
