using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private HeroScript _hero;
    private HeroInputActions _inputAction;

    private void Awake()
    {
        _inputAction = new HeroInputActions();

        _inputAction.Hero.MovementVector2D.performed += OnAxesInput;
        _inputAction.Hero.MovementVector2D.canceled += OnAxesInput;
        _inputAction.Hero.Attack.canceled += OnAttackInput;
    }

    private void OnEnable()
    {
        _inputAction.Enable();
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

    public void OnAxesInput(InputAction.CallbackContext callback)
    {
        var direction = callback.ReadValue<Vector2>();
        _hero.SetDirection(direction);
    }
}
