using PixelCrew.Creatures;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private HeroScript _hero;

        private void Awake()
        {
            _hero = GetComponent<HeroScript>();
        }

        public void OnAttackInput(InputAction.CallbackContext callback)
        {
            if (callback.performed)
            {
                _hero.StartAttackAnimation();
            }
        }

        public void OnTrowInput(InputAction.CallbackContext callback)
        {
            if (callback.performed)
            {
                _hero.StartThrowAnimation();
            }
        }

        public void OnMultithrowInput(InputAction.CallbackContext callback)
        {
            if (callback.performed)
            {
                _hero.StartMultithrow();
            }
        }

        public void OnInteractionInput(InputAction.CallbackContext callback)
        {
            if (callback.performed)
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
}
