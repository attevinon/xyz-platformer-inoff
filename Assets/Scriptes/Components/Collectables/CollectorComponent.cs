using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using UnityEngine.Events;

namespace PixelCrew.Components.Collectables
{
    public class CollectorComponent : MonoBehaviour, ICanAddInInventory
    {
        [SerializeField] private List<InventoryItemData> _items = new List<InventoryItemData>();
        [SerializeField] private UnityEvent _onFirstFilled;
        [SerializeField] private UnityEvent _onEmpty;
        private GameSession _gameSession;

        public bool TryAddInInventory(string id, int value)
        {
            var item = _items.Find(i => i.Id == id);
            if (item == null)
            {
                if (_items.Count == 0) _onFirstFilled?.Invoke();
                item = new InventoryItemData(id);
                _items.Add(item);
            }
            item.Value += value;
            //имеет ли значение стакаемый ли предмет когда речь идёт о бочке? я думаю нет
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
            _onEmpty?.Invoke();
        }
    }
}
