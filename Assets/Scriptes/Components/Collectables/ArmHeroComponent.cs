using UnityEngine;
using PixelCrew.Creatures.Hero;

namespace PixelCrew.Components.Collectables
{
    public class ArmHeroComponent : MonoBehaviour
    {
        public void ArmHero(GameObject go)
        {
            if (go.TryGetComponent(out HeroScript hero))
            {
                hero.ArmHero();
            }
        }
    }
}

