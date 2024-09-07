using UnityEngine;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Audio;

namespace PixelCrew.Creatures
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlaySoundsComponent))]
    public class Creature : MonoBehaviour
    {
        [Header("Params:")]
        [SerializeField] protected float _speed;
        [SerializeField] protected float _jumpForce;
        [SerializeField] private float _jumpDamageForce;
        [SerializeField] private bool _isLooksToTheRight;

        [Header("Chekers:")]
        [SerializeField] private LayerChecker _groundCheker;
        [SerializeField] private CheckCircleOverlap _attackRange;

        [Header("Particles:")]
        [SerializeField] protected SpawnListComponent _particlesSpawners;

        protected Vector2 _direction;
        protected bool _isGrounded;
        protected bool _isJumping;
        protected Rigidbody2D _rigidbody;
        protected Animator Animator;
        protected PlaySoundsComponent Sounds;

        static readonly int isRunningKey = Animator.StringToHash("is-running");
        static readonly int isGroundedKey = Animator.StringToHash("is-grounded");
        static readonly int verticalVelocityKey = Animator.StringToHash("vertical-velocity");
        static readonly int hitKey = Animator.StringToHash("hit");
        static readonly int attackKey = Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
        }

        protected virtual void Update()
        {
            _isGrounded = IsGrounded();
        }

        private void FixedUpdate()
        {
            float xVelocity = _direction.x * _speed;
            _rigidbody.velocity = new Vector2(xVelocity, _rigidbody.velocity.y);

            SetAnimations();
        }

        protected void SetAnimations()
        {
            Animator.SetBool(isRunningKey, _direction.x != 0);
            Animator.SetBool(isGroundedKey, _isGrounded);
            Animator.SetFloat(verticalVelocityKey, _rigidbody.velocity.y);
            UpdateSpriteDirection(_direction);
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void UpdateSpriteDirection(Vector2 direction)
        {
            float multiplier = _isLooksToTheRight ? 1 : -1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }

        private bool IsGrounded()
        {
            return _groundCheker.IsTouchingLayer;
        }

        public virtual void TakeDamage()
        {
            _isJumping = false;
            Animator.SetTrigger(hitKey);
            _rigidbody.velocity = Vector2.up * _jumpDamageForce;
        }

        public virtual void StartAttackAnimation()
        {
            Animator.SetTrigger(attackKey);
        }

        private void OnAttackRange()
        {
            _particlesSpawners.Spawn("Attack");
            _attackRange.CheckOverlap();
        }
    }
}

