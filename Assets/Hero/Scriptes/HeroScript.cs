using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Movement();
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
        _rigidbody.velocity = new Vector2(_direction.x, _rigidbody.velocity.y);
    }

    public void Attack()
    {
        Debug.Log("ATTACK!!!!!!!!!!");
    }
}
