using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    [SerializeField] private LayerChecker _groundCheker;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Vector2 _direction;

    static readonly int isRunningKey = Animator.StringToHash("is-running");
    static readonly int isGroundedKey = Animator.StringToHash("is-grounded");
    static readonly int verticalVelocityKey = Animator.StringToHash("vertical-velocity");

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Movement();
        Jumping();
        SetAnimationKeys();
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    /// <summary>
    /// Обеспечивает движение героя
    /// </summary>
    private void Movement()
    {
        _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);
    }

    private void Jumping()
    {
        if (_direction.y > 0)
        {
            if (IsGrounded())
            {
                _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
        }
        else if (_rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
        }
    }

    private void SetAnimationKeys()
    {
        _animator.SetBool(isRunningKey, _direction.x != 0);
        _animator.SetBool(isGroundedKey, IsGrounded());
        _animator.SetFloat(verticalVelocityKey, _rigidbody.velocity.y);
    }

    private bool IsGrounded()
    {
        return _groundCheker.isTouchingLayer;
    }

    public void Attack()
    {
        Debug.Log("ATTACK!!!!!!!!!!");
    }
}
