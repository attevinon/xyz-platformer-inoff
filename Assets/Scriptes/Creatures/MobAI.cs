using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures
{
    [RequireComponent(typeof(Creature))]
    [RequireComponent(typeof(Patrol))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(SpawnListComponent))]
    public class MobAI : MonoBehaviour
    {
        [SerializeField] private LayerChecker _vision;
        [SerializeField] private LayerChecker _attackRange;

        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _missCooldown;
        [SerializeField] private float _detectCooldown;

        private bool _isDead;
        private Coroutine _currentCoroutine;

        private GameObject _target;
        private Creature _creature;
        private Patrol _patrol;
        private SpawnListComponent _particles;
        private Animator _animator;
        private CapsuleCollider2D _collider;

        private readonly int isDeadKey = Animator.StringToHash("is-dead"); 

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _creature = GetComponent<Creature>();
            _particles = GetComponent<SpawnListComponent>();
            _patrol = GetComponent<Patrol>();
            _collider = GetComponent<CapsuleCollider2D>();
        }

        void Start()
        {
            StartCoroutine(_patrol.DoPatrol());
        }

        public void OnTargetInVision(GameObject go)
        {
            if (_isDead) return;

            _target = go;

            StartState(ReactOnTarget());
        }

        private IEnumerator ReactOnTarget()
        {
            _patrol.enabled = false;

            _particles.Spawn("Detect");
            yield return new WaitForSeconds(_detectCooldown);

            StartState(GoToTarget());
        }

        private IEnumerator GoToTarget()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_attackRange.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }

                yield return null;
            }

            _particles.Spawn("Miss");
            yield return new WaitForSeconds(_missCooldown);

            _patrol.enabled = true;
            StartState(_patrol.DoPatrol());
        }
        private void SetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            _creature.SetDirection(direction.normalized);
        }

        private IEnumerator Attack()
        {
            while (_attackRange.IsTouchingLayer)
            {
                _creature.StartAttackAnimation();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(GoToTarget());
        }

        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);

            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }

            _currentCoroutine = StartCoroutine(coroutine);
        }

        public void OnDie()
        {
            _isDead = true;
            _collider.direction = CapsuleDirection2D.Horizontal;
            _animator.SetBool(isDeadKey, true);

            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }

            /*var layer = LayerMask.NameToLayer("Trash");
            gameObject.layer = layer;*/
        }
    }
}
