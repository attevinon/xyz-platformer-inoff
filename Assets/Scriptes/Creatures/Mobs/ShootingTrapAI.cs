using UnityEngine;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Utils;
using UnityEngine.PlayerLoop;

public class ShootingTrapAI : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] private LayerChecker _vision;
    [SerializeField] private SpawnComponent _rangeAttackSpawner;
    [SerializeField] private Cooldown _rangeCooldown;

    [Header("Melee")]
    [SerializeField] private LayerChecker _meleeCanAttack;
    [SerializeField] private CheckCircleOverlap _meleeAttack;
    [SerializeField] private Cooldown _meleeCooldown;


    private GameObject _target;
    private Animator _animator;
    static readonly int meleeKey = Animator.StringToHash("melee");
    static readonly int rangeKey = Animator.StringToHash("range");
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_vision.IsTouchingLayer)
        {
            if(_meleeCanAttack.IsTouchingLayer)
            {
                if(_meleeCooldown.IsReady) StartMeleeAttackAnimation();
                return;
            }

            if(_rangeCooldown.IsReady) StartRangeAttackAnimation();
        }
    }

    private void StartMeleeAttackAnimation()
    {
        _animator.SetTrigger(meleeKey);
        _meleeCooldown.Reset();
    }

    public void DoMeleeAttack()
    {
        _meleeAttack.CheckOverlap();
    }

    private void StartRangeAttackAnimation()
    {
        _animator.SetTrigger(rangeKey);
        _rangeCooldown.Reset();
    }

    public void DoRangeAttack()
    {
        _rangeAttackSpawner.Spawn();
    }
}
