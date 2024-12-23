using UnityEngine;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Utils;

namespace PixelCrew.Creatures.Mobs
{
    public class ShootingAndMeleeTrapAI : ShootingTrapAI
    {
        [Header("Melee")]
        [SerializeField] private LayerChecker _meleeCanAttack;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private Cooldown _meleeCooldown;

        static readonly int meleeKey = Animator.StringToHash("melee");

        protected override void Update()
        {
            if (_meleeCanAttack.IsTouchingLayer)
            {
                if (_meleeCooldown.IsReady)
                {
                    StartMeleeAttackAnimation();
                }
                return;
            }

            base.Update();
        }

        private void StartMeleeAttackAnimation()
        {
            Animator.SetTrigger(meleeKey);
            _meleeCooldown.Reset();
        }

        public void DoMeleeAttack()
        {
            _meleeAttack.CheckOverlap();
        }
    }
}

