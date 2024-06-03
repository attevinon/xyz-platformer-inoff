using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.GoBased
{
    public class SpawnBurstComponent : MonoBehaviour
    {
        [Header("Spawn bound:")]
        [SerializeField] private float _sectorAngle = 60;
        [SerializeField] private float _sectorRotation;
        [SerializeField] private Transform _transform;


        [Header("Spawn params:")]
        [Space]

        [SerializeField] private float _waitTime = 0.1f;
        [SerializeField] private float _speed = 6;
        [SerializeField] private float _itemPerBurst = 1;
        [SerializeField] private GameObject _particle;
        [SerializeField] private UnityEvent _onSpawned;

        private Coroutine _routine;
        private bool _isEnabled;

        private void Prepeare()
        {
            TryStopRoutine();
            this.transform.position = _transform.position;
        }

        public void StartSpawn(GameObject[] particles)
        {
            Prepeare();
            _routine = StartCoroutine(SpawnBurst(particles));
        }

        public void StartSpawn()
        {
            Prepeare();
            _routine = StartCoroutine(SpawnBurst());
        }

        private IEnumerator SpawnBurst(GameObject[] particles)
        {
            for (var i = 0; i < particles.Length;)
            {
                for (var j = 0; j < _itemPerBurst && i < particles.Length; j++)
                {
                    Spawn(particles[i]);
                    i++;
                }

                yield return new WaitForSeconds(_waitTime);
            }

            _onSpawned?.Invoke();
        }

        private IEnumerator SpawnBurst()
        {
            if (_particle == null)
                TryStopRoutine();

            while (_isEnabled)
            {
                for (var j = 0; j < _itemPerBurst; j++)
                {
                    Spawn(_particle);
                }

                yield return new WaitForSeconds(_waitTime);
            }

            _onSpawned?.Invoke();
        }

        [ContextMenu("Spawn one")]
        private void Spawn(GameObject particle)
        {
            var instance = Instantiate(particle, transform.position, Quaternion.identity);
            var rigidBody = instance.GetComponent<Rigidbody2D>();

            var randomAngle = Random.Range(0, _sectorAngle);
            var forceVector = AngleToVectorInSector(randomAngle);
            rigidBody.AddForce(forceVector * _speed, ForceMode2D.Impulse);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var position = transform.position;

            var middleAngleDelta = (180 - _sectorRotation - _sectorAngle) / 2;
            var rightBound = GetUnitOnCircle(middleAngleDelta);
            Handles.DrawLine(position, position + rightBound);

            var leftBound = GetUnitOnCircle(middleAngleDelta + _sectorAngle);
            Handles.DrawLine(position, position + leftBound);
            Handles.DrawWireArc(position, Vector3.forward, rightBound, _sectorAngle, 1);

            Handles.color = new Color(1f, 1f, 1f, 0.1f);
            Handles.DrawSolidArc(position, Vector3.forward, rightBound, _sectorAngle, 1);
        }
#endif

        private Vector2 AngleToVectorInSector(float angle)
        {
            var angleMiddleDelta = (180 - _sectorRotation - _sectorAngle) / 2;
            return GetUnitOnCircle(angle + angleMiddleDelta);
        }

        private Vector3 GetUnitOnCircle(float angleDegrees)
        {
            var angleRadians = angleDegrees * Mathf.PI / 180.0f;

            var x = Mathf.Cos(angleRadians);
            var y = Mathf.Sin(angleRadians);

            return new Vector3(x, y, 0);
        }

        private void OnDisable()
        {
            TryStopRoutine();
        }

        private void OnDestroy()
        {
            TryStopRoutine();
        }

        private void TryStopRoutine()
        {
            if (_routine != null)
                StopCoroutine(_routine);
        }
    }
}

