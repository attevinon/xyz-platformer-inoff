using System.Collections;
using UnityEngine;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.Interactions;
using PixelCrew.Components.Health;
using PixelCrew.Model;
using PixelCrew.Utils;

namespace PixelCrew.Creatures.Hero
{
    [RequireComponent(typeof(HealthComponent))]
    public class HeroScript : Creature
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
        private bool IsAbleToThrow => _session.Data.IsArmed && _session.Data.Projectiles > 1;

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

            if(_session.Data.IsArmed && _session.Data.Projectiles <= 0)
            {
                _session.Data.Projectiles = 1;
            }
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
            //проверка прыжка??
            if(!IsAbleToThrow) return;
            if(!_throwCooldown.IsReady) return;

            _session.Data.Projectiles -= 1;
            Animator.SetTrigger(throwKey);
            _throwCooldown.Reset();
            Debug.Log("Projectiles = " + _session.Data.Projectiles);
        }

        public void StartMultithrow()
        {
            if (!IsAbleToThrow) return;
            if (!_throwCooldown.IsReady) return;
            StartCoroutine(MultithrowCoroutine());
        }

        private IEnumerator MultithrowCoroutine()
        {
            for (int i = 0; i <= _projectilesPerMultithrow; i++)
            {
                if (_session.Data.Projectiles <= 1) break;
                _session.Data.Projectiles -= 1;
                Animator.SetTrigger(throwKey);
                yield return new WaitForSeconds(_secBetweenProjectilesInMultithrow);
            }
            _throwCooldown.Reset();
            Debug.Log("Projectiles = " + _session.Data.Projectiles);
        }

        public void DoThrow()
        {
            _particlesSpawners.Spawn("Throw");
        }

        public void ArmHero()
        {
            if (!_session.Data.IsArmed)
            {
                _session.Data.IsArmed = true;
                UpdateHeroWeapon();
            }

            _session.Data.Projectiles += 1;
            Debug.Log("Projectiles = " + _session.Data.Projectiles);
        }

        public void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = (_session.Data.IsArmed) ? _armed : _unarmed;
        }
    }
}
