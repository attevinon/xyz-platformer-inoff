using UnityEngine;
using PixelCrew.Utils;

namespace PixelCrew.Components.ColliderBased
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private string[] _tags;
        [SerializeField] private EnterEvent _action;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_layer != ~0)
            {
                if (!collision.gameObject.IsInLayer(_layer)) return;
                if(_tags?.Length == 0)
                {
                    _action?.Invoke(collision.gameObject);
                    return;
                }
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
}
