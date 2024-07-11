using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace PixelCrew.Components.Health
{
    public class HealthImpactComponent : MonoBehaviour
    {
        [SerializeField] private int _impactValue;

        public void AffectTheObjectsHealth(GameObject target)
        {
            var targetHealth = target.GetComponent<HealthComponent>();

            if (targetHealth != null)
                targetHealth.ApplyHealthImpact(_impactValue);
        }
        public void AffectHeroHealth()
        {
            var hero = FindObjectOfType<HeroScript>();
            AffectTheObjectsHealth(hero.gameObject);
        }
    }

}

