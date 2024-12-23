using UnityEngine;
using PixelCrew.Components;

public class DropContainerComponent : MonoBehaviour
{
    [SerializeField] private GameObject[] _itemsToDrop;
    [SerializeField] private LootEvent _onDrop;

    public void Drop()
    {
        _onDrop?.Invoke(_itemsToDrop);
    }
}
