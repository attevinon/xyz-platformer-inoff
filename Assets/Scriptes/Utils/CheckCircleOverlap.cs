using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Utils
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius;
        private Collider2D[] _interactionResult = new Collider2D[4];

        //добавить обощённую перегрузку чтобы сразу тут фильтровать наличие компонента?
        public GameObject[] GetGameObjectsInRange()
        {
            int size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _interactionResult);

            var overlaps = new List<GameObject>();

            for (int i = 0; i < size; i++)
            {
                overlaps.Add(_interactionResult[i].gameObject);
            }

            return overlaps.ToArray();
        }
        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransperentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
    }
}

