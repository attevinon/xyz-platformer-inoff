using System;
using UnityEngine;
using UnityEngine.Events;
using PixelCrew.Utils;

namespace PixelCrew.Components.ColliderBased
{
    public abstract class BaseEnterComponent : MonoBehaviour
    {
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private string[] _tags;
        [SerializeField] protected EnterEvent _action;

        protected bool IsInConditions(GameObject other)
        {
            if (_layer != ~0)
            {
                if (!other.IsInLayer(_layer)) return false;
                if (_tags?.Length == 0) return true;
            }

            foreach (var tag in _tags)
            {
                if (other.CompareTag(tag)) return true;
            }

            return false;
        }

    }

    [Serializable]
    public class EnterEvent : UnityEvent<GameObject>
    {

    }
}
