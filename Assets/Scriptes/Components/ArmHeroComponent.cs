using PixelCrew.Creatures;
using UnityEngine;

namespace PixelCrew
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

