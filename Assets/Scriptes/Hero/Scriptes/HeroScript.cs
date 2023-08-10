using Model;
using PixelCrew.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : MonoBehaviour
{
    [SerializeField] private int _damage;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpDamageForce;
    [SerializeField] private float _slamdownVelocity;

    [SerializeField] private SpawnComponent _runDustSpawner;
    [SerializeField] private SpawnComponent _jumpDustSpawner;
    [SerializeField] private SpawnComponent _slamdownDustSpawner;
    [SerializeField] private SpawnComponent _attackWaveSpawner;

    [SerializeField] private ParticleSystem _coinsParticles;
    [SerializeField] private LayerChecker _groundCheker;
    [SerializeField] private CheckCircleOverlap _attackRange;

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private float _interactionRadius;

    [SerializeField] private RuntimeAnimatorController _unarmed;
    [SerializeField] private RuntimeAnimatorController _armed;

    private GameSession _session;

    private Vector2 _direction;
    private bool _isGrounded;
    private bool _allowDoubleJump;
    private bool _isJumping;

    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private Collider2D[] _interactionResult = new Collider2D[1];

    static readonly int isRunningKey = Animator.StringToHash("is-running");
    static readonly int isGroundedKey = Animator.StringToHash("is-grounded");
    static readonly int verticalVelocityKey = Animator.StringToHash("vertical-velocity");
    static readonly int hitKey = Animator.StringToHash("hit");
    static readonly int attackKey = Animator.StringToHash("attack");

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();

        var health = GetComponent<HealthComponent>();
        health.SetHealth(_session.Data.Health);

        UpdateHeroWeapon();
    }

    void Update()
    {
        _isGrounded = IsGrounded();
    }

    void FixedUpdate()
    {
        float xVelocity = _direction.x * _speed;
        float yVelocity = CalculateYVelocity();

        _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

        SetAnimationKeys();
        UpdateSpriteDirection();
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private float CalculateYVelocity()
    {
        float yVelocity = _rigidbody.velocity.y;
        bool isJumpPressing = _direction.y > 0;

        if (_isGrounded)
        {
            _allowDoubleJump = true;
            _isJumping = false;
        }

        if (isJumpPressing)
        {
            _isJumping = true;
            yVelocity = CalculateJumpVelocity(yVelocity);
        }
        else if (_rigidbody.velocity.y > 0 && _isJumping) //падение
        {
            yVelocity *= 0.5f;
        }

        return yVelocity;
    }

    private float CalculateJumpVelocity(float yVelocity)
    {
        bool isFalling = _rigidbody.velocity.y <= 0.001f;
        if (!isFalling) return yVelocity;

        if (_isGrounded)
        {
            //_rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            yVelocity += _jumpForce;
            _jumpDustSpawner.Spawn();
        }

        else if (_allowDoubleJump)
        {
            yVelocity = _jumpForce;
            _allowDoubleJump = false;
            _jumpDustSpawner.Spawn();
        }

        return yVelocity;
    }

    private void SetAnimationKeys()
    {
        _animator.SetBool(isRunningKey, _direction.x != 0);
        _animator.SetBool(isGroundedKey, _isGrounded);
        _animator.SetFloat(verticalVelocityKey, _rigidbody.velocity.y);
    }

    public void SpawnRunDust()
    {
        _runDustSpawner.Spawn();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.IsInLayer(_groundLayer))
        {
            var contact = collision.GetContact(0);
            if(contact.relativeVelocity.y >= _slamdownVelocity)
            {
                _slamdownDustSpawner.Spawn();
            }
        }
    }

    private void UpdateSpriteDirection()
    {
        if(_direction.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        else if (_direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private bool IsGrounded()
    {
        return _groundCheker.isTouchingLayer;
    }

    public void OnHealthChanged(int health)
    {
        _session.Data.Health = health;
    }

    public void TakeDamage()
    {
        _animator.SetTrigger(hitKey);
        _rigidbody.velocity = Vector2.up * _jumpDamageForce;

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

    public void TakeHealing()
    {
        //хочу сюда анимацию добавить
    }

    public void RefillScore(int value)
    {
        _session.Data.Coins += value;
        Debug.Log("Total Coins: " + _session.Data.Coins);
    }

    public void ArmHero()
    {
        _session.Data.IsArmed = true;
        UpdateHeroWeapon();
    }

    public void UpdateHeroWeapon()
    {
        _animator.runtimeAnimatorController = (_session.Data.IsArmed) ?  _armed : _unarmed;
    }

    public void Interact()
    {
        int size = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            _interactionRadius,
            _interactionResult,
            _interactionLayer);

        for (int i = 0; i < size; i++)
        {
            var interactable = _interactionResult[i].GetComponent<InteractableComponent>();

            if(interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    public void StartAttack()
    {
        if(_session.Data.IsArmed)
            _animator.SetTrigger(attackKey);
    }

    private void Attack()
    {
        _attackWaveSpawner.Spawn();
        var objectsInRange = _attackRange.GetGameObjectsInRange();

        foreach (var objectInRange in objectsInRange)
        {
            var healthComponent = objectInRange.GetComponent<HealthComponent>();

            if (healthComponent != null && objectInRange.CompareTag("Enemy"))
            {
                healthComponent.ApplyHealthImpact(-_damage);
            }
        }
    }
}
