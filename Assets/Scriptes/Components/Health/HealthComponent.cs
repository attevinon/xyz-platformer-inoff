using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private HealthChangedEvent _onChanged;
        [SerializeField] public UnityEvent OnDie;

        public void SetHealth(int health)
        {
            _health = health;
        }

        public void ApplyHealthImpact(int impactValue)
        {
            _health += impactValue;
            _onChanged?.Invoke(_health);

            if (impactValue > 0)
            {
                _onHeal?.Invoke();
            }
            else if (impactValue < 0)
            {
                if (_health <= 0)
                {
                    OnDie?.Invoke();
                    return;
                }
                else
                {
                    _onDamage?.Invoke();
                }
            }
        }

        private void OnDestroy()
        {
            OnDie.RemoveAllListeners();
        }

        [Serializable]
        public class HealthChangedEvent : UnityEvent<int>
        {
        }
    }
}

