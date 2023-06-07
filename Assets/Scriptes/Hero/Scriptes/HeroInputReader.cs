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
    public void OnAttackInput(InputAction.CallbackContext callback)
    {
        if (callback.canceled)
        {
            _hero.Attack();
        }
    }

    public void OnInteractionInput(InputAction.CallbackContext callback)
    {
        if (callback.canceled)
        {
            _hero.Interact();
        }
    }

    public void OnAxesInput(InputAction.CallbackContext callback)
    {
        var direction = callback.ReadValue<Vector2>();
        _hero.SetDirection(direction);
    }
}
