using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    [SerializeField] private LayerChecker _groundCheker;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Movement();
        Jumping();
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

    private bool IsGrounded()
    {
        return _groundCheker.isTouchingLayer;
    }

    public void Attack()
    {
        Debug.Log("ATTACK!!!!!!!!!!");
    }
}
