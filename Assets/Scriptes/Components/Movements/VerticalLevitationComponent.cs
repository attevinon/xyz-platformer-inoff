using System.ComponentModel;
using UnityEngine;

namespace PixelCrew.Components.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class VerticalLevitationComponent : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;

        [Description("Изменяет высоту")]
        [SerializeField] private float _amplitude = 1f;

        [SerializeField] private bool _isRandomize;

        private Rigidbody2D _rigidbody;
        private float _originalY;
        private float _seed;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _originalY = _rigidbody.position.y;
            if (_isRandomize)
                _speed = Random.value * Mathf.PI * 2;
        }

        private void Update()
        {
            Vector2 position = _rigidbody.position;
            position.y = _originalY + Mathf.Sin(_seed + Time.time * _speed) * _amplitude;
            _rigidbody.MovePosition(position);
        }
    }
}

