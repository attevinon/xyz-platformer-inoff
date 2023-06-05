using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onDie;

    public void ApplyDamage(int damageValue)
    {
        _health -= damageValue;

        if(_health <= 0)
        {
            _onDie?.Invoke();
        }
        else
        {
            _onDamage?.Invoke();
            Debug.Log("your health = " + _health);
        }
    }
}
