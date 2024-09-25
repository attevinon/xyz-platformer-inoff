using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Mobs
{
    public class SnailShellAI : MonoBehaviour
    {
        [Header("Params:")]
        [SerializeField] protected float _speed;
        [SerializeField] private float _jumpHitForce;
        [SerializeField] private float _jumpDeathForce;
        [SerializeField] private float _rotationDeathForce;
        [SerializeField] private float _stayAfterWallHitForSec = 1f;
        [SerializeField] private UnityEvent _onDie;

        protected Vector2 _direction;
        private int _directionX;
        protected Rigidbody2D _rigidbody;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _directionX = transform.localScale.x > 0 ? -1 : 1;
            _direction.x = _directionX;
            UpdateSpriteDirection(_direction);
        }

        private void FixedUpdate()
        {
            float xVelocity = _direction.x * _speed;
            _rigidbody.velocity = new Vector2(xVelocity, _rigidbody.velocity.y);

            UpdateSpriteDirection(_direction);
        }

        public void CheckHit(GameObject go)
        {
            if (go.TryGetComponent(out Rigidbody2D rigidbody))
            {
                if (rigidbody.velocity.y < -0.1f)
                    OnDie();
            }
        }

        public void UpdateSpriteDirection(Vector2 direction)
        {
            if (direction.x == 0) return;
            transform.localScale = new Vector3(-direction.x, 1, 1);
        }

        public void OnWallHit()
        {
            _direction = Vector2.zero;
            _directionX = -_directionX;
            _rigidbody.velocity = Vector2.up * _jumpHitForce;
            Invoke(nameof(AfterWallHit), _stayAfterWallHitForSec);
        }

        private void AfterWallHit()
        {
            _direction = new Vector2(_directionX, 0);
        }

        public void OnDie()
        {
            _onDie?.Invoke();
            _direction = Vector2.zero;
            _rigidbody.freezeRotation = false;
            _rigidbody.AddForce(new Vector2(0, _jumpDeathForce), ForceMode2D.Impulse);
            _rigidbody.AddTorque(_rotationDeathForce, ForceMode2D.Impulse);
            this.gameObject.layer = LayerMask.NameToLayer("Trash");
        }
    }

}