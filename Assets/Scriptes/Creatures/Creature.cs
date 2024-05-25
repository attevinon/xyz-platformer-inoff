using PixelCrew.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class Creature : MonoBehaviour
    {
        [Header("Params:")]
        [SerializeField] private float _speed;
        [SerializeField] protected float _jumpForce;
        [SerializeField] private float _jumpDamageForce;
        [SerializeField] private bool _isLooksToTheRight;

        [Header("Chekers:")]
        [SerializeField] private LayerChecker _groundCheker;
        [SerializeField] private CheckCircleOverlap _attackRange;

        [Header("Particles:")]
        [SerializeField] protected SpawnListComponent _particlesSpawners;

        private Vector2 _direction;
        protected bool _isGrounded;
        protected bool _isJumping;
        private Rigidbody2D _rigidbody;
        protected Animator Animator;

        static readonly int isRunningKey = Animator.StringToHash("is-running");
        static readonly int isGroundedKey = Animator.StringToHash("is-grounded");
        static readonly int verticalVelocityKey = Animator.StringToHash("vertical-velocity");
        static readonly int hitKey = Animator.StringToHash("hit");
        static readonly int attackKey = Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            _isGrounded = IsGrounded();
        }

        void FixedUpdate()
        {
            float xVelocity = _direction.x * _speed;
            float yVelocity = CalculateYVelocity();

            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            SetAnimationKeys();
            UpdateSpriteDirection(_direction);
        }

        private void SetAnimationKeys()
        {
            Animator.SetBool(isRunningKey, _direction.x != 0);
            Animator.SetBool(isGroundedKey, _isGrounded);
            Animator.SetFloat(verticalVelocityKey, _rigidbody.velocity.y);
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

        protected virtual float CalculateYVelocity()
        {
            float yVelocity = _rigidbody.velocity.y;
            bool isJumpPressing = _direction.y > 0;

            if (_isGrounded)
            {
                _isJumping = false;
            }

            if (isJumpPressing)
            {
                //говніна
                _isJumping = true;
                bool isNotGoingUp = _rigidbody.velocity.y <= 0.001f;
                yVelocity = isNotGoingUp ? CalculateJumpVelocity(yVelocity) : yVelocity;

            }
            else if (_rigidbody.velocity.y > 0 && _isJumping) //падение
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (_isGrounded)
            {
                //_rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                yVelocity += _jumpForce;
                _particlesSpawners.Spawn("Jump");
            }

            return yVelocity;
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

