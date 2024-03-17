﻿using Model;
using PixelCrew.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures
{
    [RequireComponent(typeof(HealthComponent))]
    public class HeroScript : Creature
    {
        [Header("Hero:")]
        [SerializeField] private float _slamdownVelocity;
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private ParticleSystem _coinsParticles;

        [Header("Animators:")]
        [SerializeField] private RuntimeAnimatorController _unarmed;
        [SerializeField] private RuntimeAnimatorController _armed;

        private bool _allowDoubleJump;

        private GameSession _session;

        static readonly int throwKey = Animator.StringToHash("throw");

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            var health = GetComponent<HealthComponent>();
            health.SetHealth(_session.Data.Health);

            UpdateHeroWeapon();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override float CalculateYVelocity()
        {
            if (_isGrounded)
            {
                _allowDoubleJump = true;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if ( !_isGrounded && _allowDoubleJump)
            {
                yVelocity = _jumpForce;
                _allowDoubleJump = false;
                _particlesSpawners.Spawn("Jump");
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        public void SpawnRunDust()
        {
            _particlesSpawners.Spawn("Run");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(_groundLayer))
            {
                var contact = collision.GetContact(0);
                if (contact.relativeVelocity.y >= _slamdownVelocity)
                {
                    _particlesSpawners.Spawn("SlamDown");
                }
            }
        }

        public void OnHealthChanged(int health)
        {
            _session.Data.Health = health;
        }
        public void TakeHealing()
        {
            //хочу сюда анимацию добавить
        }
        public override void TakeDamage()
        {
            base.TakeDamage();

            if (_session.Data.Coins > 0)
            {
                DropCoins();
            }
        }

        private void DropCoins()
        {
            int coinsToDrop = Mathf.Min(_session.Data.Coins, 5);
            _session.Data.Coins -= coinsToDrop;

            var coinsBurst = _coinsParticles.emission.GetBurst(0);
            coinsBurst.count = coinsToDrop;
            _coinsParticles.emission.SetBurst(0, coinsBurst);

            _coinsParticles.gameObject.SetActive(true);
            _coinsParticles.Play();
        }

        public void RefillScore(int value)
        {
            _session.Data.Coins += value;
            Debug.Log("Total Coins: " + _session.Data.Coins);
        }

        public void Interact()
        {
            _interactionCheck.CheckOverlap();
        }

        //я искренне не поняла зачем лектор сделал отдельный класс для этого метода,
        //мне кажется от этого теряется смысл рефакторинга
        //поэтому я поместила этот метод сюда
        public void DoInteraction(GameObject go)
        {
            var interactable = go.GetComponent<InteractableComponent>();

            if (interactable == null) return;

            interactable.Interact();
        }

        public override void StartAttackAnimation()
        {
            if (!_session.Data.IsArmed) return;

            base.StartAttackAnimation();
        }
        public void StartThrowAnimation()
        {
            //проверка запаса мечей??
            //проверка прыжка??
            if (!_session.Data.IsArmed) return;

            Animator.SetTrigger(throwKey);
        }

        public void DoThrow()
        {
            _particlesSpawners.Spawn("Throw");
        }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
        }

        public void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = (_session.Data.IsArmed) ? _armed : _unarmed;
        }
    }
}
