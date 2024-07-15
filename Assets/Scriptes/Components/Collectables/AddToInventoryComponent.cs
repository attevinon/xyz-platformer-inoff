using UnityEngine;
using PixelCrew.Creatures.Hero;
using PixelCrew.Model.Definitions;
using UnityEngine.Events;

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
            if (gameObject.TryGetComponent(out HeroScript hero))
            {
                var isSuccess = hero.TryAddToInventory(_id, _count);
                if(isSuccess)
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
}
