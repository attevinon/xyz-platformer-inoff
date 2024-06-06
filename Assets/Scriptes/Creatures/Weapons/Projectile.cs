using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private bool _flip;

        private int _direction;
        private Rigidbody2D _rigidbody;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            _direction = _flip || transform.lossyScale.x < 0 ? -1 : 1;
        }

        void FixedUpdate()
        {
            var position = _rigidbody.position;
            position.x += (_speed * _direction);
            _rigidbody.MovePosition(position);
        }
    }
}

