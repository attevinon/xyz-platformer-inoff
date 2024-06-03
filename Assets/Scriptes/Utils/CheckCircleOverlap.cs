using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Utils
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private string[] _tags;
        [SerializeField] private bool _areTagsNeeded;

        [SerializeField]  private OnOverlapEvent _onOverlap;

        private Collider2D[] _overlapResult = new Collider2D[10];

        public void CheckOverlap()
        {
            int size = Physics2D.OverlapCircleNonAlloc(
                transform.position, 
                _radius, 
                _overlapResult, 
                _layerMask);

            for (int i = 0; i < size; i++)
            {
                var overlap = _overlapResult[i];

                if (_areTagsNeeded)
                {
                    bool isInTags = _tags.Any(tag => overlap.CompareTag(tag));

                    if (!isInTags)
                    {
                        continue;
                    }
                }

                _onOverlap?.Invoke(overlap.gameObject);
            }
        }

        public void GetOverlaps()
        {
            int size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _radius,
                _overlapResult,
                _layerMask);

            for (int i = 0; i < size; i++)
            {
                var overlap = _overlapResult[i];
                bool isInTags = _tags.Any(tag => overlap.CompareTag(tag));
                if (isInTags)
                {
                    _onOverlap?.Invoke(overlap.gameObject);
                }
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransperentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
#endif
        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject>
        {
        }
    }
}

