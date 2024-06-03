using UnityEngine;
using PixelCrew.Creatures.Hero;

namespace PixelCrew.Components.Collectables
{
    public class ScoreRefillComponent : MonoBehaviour
    {
        [SerializeField] private int _coinValue;

        private HeroScript _hero;

        void Start()
        {
            _hero = FindObjectOfType<HeroScript>();
        }
        public void IncreaseScore()
        {
            _hero.RefillScore(_coinValue);
        }
    }
}
