using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onHeal;
    [SerializeField] private UnityEvent _onDie;

    public void ApplyHealthImpact(int impactValue)
    {
        _health += impactValue;

        if (impactValue > 0)
        {
            _onHeal?.Invoke();
        }
        else if (impactValue < 0)
        {
            if (_health <= 0)
            {
                _onDie?.Invoke();
                return;
            }
            else
            {
                _onDamage?.Invoke();
            }
        }
       
    }
}
