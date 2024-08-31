using System.Collections;
using UnityEngine;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.Interactions;
using PixelCrew.Components.Health;
using PixelCrew.Model;
using PixelCrew.Utils;
using System;
using PixelCrew.Components.Audio;

namespace PixelCrew.Creatures.Hero
{
    [RequireComponent(typeof(HealthComponent))]
    public class HeroScript : Creature, ICanAddInInventory
    {
        [Header("Hero:")]
        [SerializeField] private float _slamdownVelocity;
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private ParticleSystem _coinsParticles;
        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private int _projectilesPerMultithrow;
        [SerializeField] private float _secBetweenProjectilesInMultithrow;

        [Header("Animators:")]
        [SerializeField] private RuntimeAnimatorController _unarmed;
        [SerializeField] private RuntimeAnimatorController _armed;

        private bool _allowDoubleJump;
        private int SwordsCount => _session.Data.Inventory.Count(Constants.ItemsId.SWORD);
        private int CoinsCount => _session.Data.Inventory.Count(Constants.ItemsId.COIN);

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

            _session.Data.Inventory.OnInventoryChanged += OnInventoryChanged;

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
                Sounds.PlaySound("jump");
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

        public void UseHealPotion()
        {
            if (_session.Data.Inventory.Count(Constants.ItemsId.POTION) <= 0)
            {
                Debug.Log("No heal potions in the inventory");
                return;
            }

            bool isUsed = _session.Data.Inventory.TryUseItem(Constants.ItemsId.POTION);
            if (!isUsed) return;
            _session.Data.Inventory.Remove(Constants.ItemsId.POTION, 1);
            Debug.Log("UI: Heal Potion successfully used");
            Debug.Log("UI: Heal Potions left: " + _session.Data.Inventory.Count(Constants.ItemsId.POTION));
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
            if (CoinsCount > 0)
                DropCoins();
        }

        private void DropCoins()
        {
            int coinsToDrop = Mathf.Min(CoinsCount, 5);
            _session.Data.Inventory.Remove(Constants.ItemsId.COIN, coinsToDrop);

            var coinsBurst = _coinsParticles.emission.GetBurst(0);
            coinsBurst.count = coinsToDrop;
            _coinsParticles.emission.SetBurst(0, coinsBurst);

            _coinsParticles.gameObject.SetActive(true);
            _coinsParticles.Play();
        }

        public bool TryAddInInventory(string id, int value)
        {
            return _session.Data.Inventory.TryAdd(id, value);
        }

        private void OnInventoryChanged(string id, int count)
        {
            if (id == Constants.ItemsId.SWORD)
                UpdateHeroWeapon();
        }

        public override void StartAttackAnimation()
        {
            if (SwordsCount <=0) return;
            base.StartAttackAnimation();
            Sounds.PlaySound("mele");
        }

        public void StartThrowAnimation()
        {
            //проверка прыжка??
            if(SwordsCount <= 1) return;
            if(!_throwCooldown.IsReady) return;

            _session.Data.Inventory.Remove(Constants.ItemsId.SWORD, 1);
            Animator.SetTrigger(throwKey);
            _throwCooldown.Reset();
            Debug.Log("Projectiles = " + SwordsCount);
        }

        public void StartMultithrow()
        {
            if (SwordsCount <= 1) return;
            if (!_throwCooldown.IsReady) return;
            StartCoroutine(MultithrowCoroutine());
        }

        private IEnumerator MultithrowCoroutine()
        {
            for (int i = 0; i <= _projectilesPerMultithrow; i++)
            {
                if (SwordsCount <= 1) break;
                _session.Data.Inventory.Remove(Constants.ItemsId.SWORD, 1);
                Animator.SetTrigger(throwKey);
                yield return new WaitForSeconds(_secBetweenProjectilesInMultithrow);
            }
            _throwCooldown.Reset();
            Debug.Log("Projectiles = " + SwordsCount);
        }

        public void DoThrow()
        {
            Sounds.PlaySound("range");
            _particlesSpawners.Spawn("Throw");
        }

        public void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordsCount >= 1 ? _armed : _unarmed;
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnInventoryChanged -= OnInventoryChanged;
        }
    }
}
