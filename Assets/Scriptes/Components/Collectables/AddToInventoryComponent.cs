using UnityEngine;
using PixelCrew.Model.Definitions;
using UnityEngine.Events;
using PixelCrew.Utils;

namespace PixelCrew.Components.Collectables
{
    public class AddToInventoryComponent : MonoBehaviour
    {
        [SerializeField] [InventoryId] private string _id;
        [SerializeField] private int _count;
        [SerializeField] private UnityEvent _onSuccess;
        [SerializeField] private UnityEvent _onFail;
        public void AddToInventory(GameObject gameObject)
        {
            ICanAddInInventory collector = gameObject.GetInterface<ICanAddInInventory>();
            bool? isSuccess = collector?.TryAddInInventory(_id, _count);
            if (isSuccess == true)
            {
                _onSuccess?.Invoke();
            }
            else
            {
                _onFail?.Invoke();
            }
        }
    }
}
