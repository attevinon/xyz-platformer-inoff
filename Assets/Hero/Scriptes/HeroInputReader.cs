using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private HeroScript _hero;

    private void Awake()
    {
        _hero = GetComponent<HeroScript>();
    }
    void Update()
    {
        //нет необходимости дёргать с вопросом "а чё а нажали уже кнопку??"
        //всё работает по ивентам
        //так что тут пусто......
    }
    private void OnAttack(InputValue callback)
    {
        _hero.Attack();
    }

    private void OnMovement(InputValue callback)
    {
        var direction = callback.Get<Vector2>();
        _hero.SetDirection(direction);
    }
}
