using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SwordProjectile : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private int _direction;
        private Rigidbody2D _rigidbody;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            _direction = transform.lossyScale.x > 0 ? 1 : -1;
        }

        void FixedUpdate()
        {
            var position = _rigidbody.position;
            position.x += (_speed * _direction);
            _rigidbody.MovePosition(position);
        }
    }
}

