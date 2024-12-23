﻿using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures.Hero
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

        public void OnThrowInput(InputAction.CallbackContext callback)
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

        public void OnUseHealPotionInput(InputAction.CallbackContext callback)
        {
            if (callback.performed)
            {
                _hero.UseHealPotion();
            }
        }

        public void OnAxesInput(InputAction.CallbackContext callback)
        {
            var direction = callback.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }
    }
}
