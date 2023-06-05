using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    [SerializeField] private int _damageValue;

    public void DamageAnObject(GameObject target)
    {
        var targetHealth = target.GetComponent<HealthComponent>();

        if (targetHealth != null)
            targetHealth.ApplyDamage(_damageValue);
    }
}
