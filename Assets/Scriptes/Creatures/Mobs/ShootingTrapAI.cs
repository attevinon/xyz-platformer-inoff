using UnityEngine;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Utils;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [Header("Range")]
        [SerializeField] private LayerChecker _vision;
        [SerializeField] private SpawnComponent _rangeAttackSpawner;
        [SerializeField] private Cooldown _rangeCooldown;

        public UnityEvent OnTargetInVision;  
        protected Animator Animator;
        static readonly int rangeKey = Animator.StringToHash("range");
        public LayerChecker Vision => _vision;

        private void Awake()
        {
            if (TryGetComponent(out Animator animator))
            {
                Animator = animator;
            }
        }

        protected virtual void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_rangeCooldown.IsReady)
                {
                    if(Animator != null)
                    {
                        StartRangeAttackAnimation();
                    }
                    else
                    {
                        OnTargetInVision?.Invoke();
                        _rangeCooldown.Reset();
                    }
                }
            }
        }

        private void StartRangeAttackAnimation()
        {
            Animator.SetTrigger(rangeKey);
            _rangeCooldown.Reset();
        }

        public void DoRangeAttack()
        {
            _rangeAttackSpawner.Spawn();
        }
    }
}
