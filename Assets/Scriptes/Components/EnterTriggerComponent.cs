using UnityEngine;
using PixelCrew.Utils;

public class EnterTriggerComponent : MonoBehaviour
{
    [SerializeField] private LayerMask _layer = ~0;
    [SerializeField] private string[] _tags;
    [SerializeField] private EnterEvent _action;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_layer != ~0)
        {
            if (!collision.gameObject.IsInLayer(_layer)) return;
        }

        foreach (var tag in _tags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                _action?.Invoke(collision.gameObject);
                return;
            }
        }
    }
}
