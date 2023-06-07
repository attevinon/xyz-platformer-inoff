using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthImpactComponent : MonoBehaviour
{
    [SerializeField] private int _impactValue;

    public void AffectTheObjectsHealth(GameObject target)
    {
        var targetHealth = target.GetComponent<HealthComponent>();

        if (targetHealth != null)
            targetHealth.ApplyHealthImpact(_impactValue);
    }
}
