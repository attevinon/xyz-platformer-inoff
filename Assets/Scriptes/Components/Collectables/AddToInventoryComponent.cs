using UnityEngine;
using PixelCrew.Creatures.Hero;
using PixelCrew.Model.Definitions;

namespace PixelCrew.Components.Collectables
{
    public class AddToInventoryComponent : MonoBehaviour
    {
        [SerializeField] [InventoryId] private string _id;
        [SerializeField] private int _count;
        public void AddToInventory(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out HeroScript hero))
            {
                hero.AddToInventory(_id, _count);
            }
        }
    }
}
