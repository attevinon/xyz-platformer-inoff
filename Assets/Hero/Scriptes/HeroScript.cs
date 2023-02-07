using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _directionX;
    private float _directionY;
    void Update()
    {
        if (_directionX != 0 || _directionY != 0)
        {
            var newXPosition = CountNewPosition(_directionX, transform.position.x);
            var newYPosition = CountNewPosition(_directionY, transform.position.y);

            transform.position = new Vector3(newXPosition, newYPosition);
        }

    }

    public void SetDirection(float direction_x, float direction_y)
    {
        _directionX = direction_x;
        _directionY = direction_y;
    }

    public void Attack()
    {
        Debug.Log("ATTACK!!!!!!!!!!");
    }

    private float CountNewPosition(float direction, float oldPosition)
    {
        float delta = direction * _speed * Time.deltaTime;
        float newPosition = oldPosition + delta;

        return newPosition;
    }
}
