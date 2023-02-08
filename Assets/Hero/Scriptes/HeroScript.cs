using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Vector2 _direction;

    void Update()
    {
        Movement();
    }

    //оставила эту перегрузку, чтобы при необходимости изменить способ ввода
    //не приходилось изменять код в данном скрипте
    //(это ваще правильно или не надо так делать?....)
    public void SetDirection(float direction_x, float direction_y)
    {
        _direction.x = direction_x;
        _direction.y = direction_y;
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    //что-то внутри подсказало мне добавить возвращаемое значение...
    /// <summary>
    /// Обеспечивает движение героя
    /// </summary>
    /// <returns>
    /// true, если положение героя изменилось;
    /// false, если положение героя не изменилось
    /// </returns>
    private bool Movement()
    {
        if (_direction.magnitude > 0)
        {
            Vector2 delta = _direction * _speed * Time.deltaTime;
            transform.position += new Vector3(delta.x, delta.y);
            return true;
        }
        else
            return false;
    }

    public void Attack()
    {
        Debug.Log("ATTACK!!!!!!!!!!");
    }
}
