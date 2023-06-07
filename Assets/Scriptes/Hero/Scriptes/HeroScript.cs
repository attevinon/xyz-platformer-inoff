using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpDamageForce;

    [SerializeField] private LayerChecker _groundCheker;

    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private float _interactionRadius;

    private Collider2D[] _interactionResult = new Collider2D[1];
    private Vector2 _direction;

    private bool _isGrounded;
    private bool _allowDoubleJump;
    private bool _isJumping;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _sprite;

    static readonly int isRunningKey = Animator.StringToHash("is-running");
    static readonly int isGroundedKey = Animator.StringToHash("is-grounded");
    static readonly int verticalVelocityKey = Animator.StringToHash("vertical-velocity");
    static readonly int hitKey = Animator.StringToHash("hit");

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _isGrounded = IsGrounded();
    }

    void FixedUpdate()
    {
        float xVelocity = _direction.x * _speed;
        float yVelocity = CalculateYVelocity();

        _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

        SetAnimationKeys();
        UpdateSpriteDirection();
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private float CalculateYVelocity()
    {
        float yVelocity = _rigidbody.velocity.y;
        bool isJumpPressing = _direction.y > 0;

        if (_isGrounded)
        {
            _allowDoubleJump = true;
            _isJumping = false;
        }

        if (isJumpPressing)
        {
            _isJumping = true;
            yVelocity = CalculateJumpVelocity(yVelocity);
        }
        else if (_rigidbody.velocity.y > 0 && _isJumping) //падение
        {
            yVelocity *= 0.5f;
        }

        return yVelocity;
    }

    private float CalculateJumpVelocity(float yVelocity)
    {
        bool isFalling = _rigidbody.velocity.y <= 0.001f;
        if (!isFalling) return yVelocity;

        if (_isGrounded)
        {
            //_rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            yVelocity += _jumpForce;
        }

        else if (_allowDoubleJump)
        {
            yVelocity = _jumpForce;
            _allowDoubleJump = false;
        }

        return yVelocity;
    }

    private void SetAnimationKeys()
    {
        _animator.SetBool(isRunningKey, _direction.x != 0);
        _animator.SetBool(isGroundedKey, _isGrounded);
        _animator.SetFloat(verticalVelocityKey, _rigidbody.velocity.y);
    }

    private void UpdateSpriteDirection()
    {
        if (_direction.x == 0)
            return;

        if(_direction.x > 0)
        {
            _sprite.flipX = false;
        }
        else if (_direction.x < 0)
        {
            _sprite.flipX = true;
        }
    }

    private bool IsGrounded()
    {
        return _groundCheker.isTouchingLayer;
    }

    public void Attack()
    {
        Debug.Log("ATTACK!!!!!!!!!!");
    }

    public void TakeDamage()
    {
        _animator.SetTrigger(hitKey);
        _rigidbody.velocity = Vector2.up * _jumpDamageForce;
    }

    public void TakeHealing()
    {

    }

    public void Interact()
    {
        int size = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            _interactionRadius,
            _interactionResult,
            _interactionLayer);

        for (int i = 0; i < size; i++)
        {
            var interactable = _interactionResult[i].GetComponent<InteractableComponent>();

            if(interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
