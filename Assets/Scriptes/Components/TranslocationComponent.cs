using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components
{
    public class TranslocationComponent : MonoBehaviour
    {
        [SerializeField] private Vector3[] _destinationPoints;
        [SerializeField] private float _smoothing;
        [SerializeField] private bool _isReversed;

        private int _nextPointIndex;
        private Coroutine _coroutine;
        private Vector3 _destinationPoint;

        public void Translocate(int index)
        {
            _destinationPoint = _destinationPoints[index];

            StartTranslocation();
        }

        public void Translocate()
        {
            CountNextPositionIndex();

            _destinationPoint = _destinationPoints[_nextPointIndex];

            StartTranslocation();
        }

        private void StartTranslocation()
        {
            if (_coroutine != null)
                return;

            _coroutine = StartCoroutine(AnimateTranslocation());
        }

        private IEnumerator AnimateTranslocation()
        {
            while (this.transform.position != _destinationPoint)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, _destinationPoint, _smoothing);
                yield return null;
            }

            _coroutine = null;
        }

        private void CountNextPositionIndex()
        {
            if (_nextPointIndex == _destinationPoints.Length - 1 && _isReversed)
            {
                Array.Reverse(_destinationPoints);
            }

            _nextPointIndex = (int)Mathf.Repeat(_nextPointIndex + 1, _destinationPoints.Length);
        }

        [ContextMenu("Translocate")]
        private void TranslocateIn()
        {
            Translocate();
        }
    }

}