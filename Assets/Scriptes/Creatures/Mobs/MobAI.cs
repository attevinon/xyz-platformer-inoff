﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Creatures.Mobs.Patrolling;

namespace PixelCrew.Creatures.Mobs
{
    [RequireComponent(typeof(Creature))]
    [RequireComponent(typeof(Patrol))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpawnListComponent))]
    public class MobAI : MonoBehaviour
    {
        [SerializeField] private CheckRaycast _visionRaycastChecker;
        [SerializeField] private LayerChecker _attackRange;

        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _missCooldown;
        [SerializeField] private float _detectCooldown;

        private bool _isDead;
        private IEnumerator _currentCoroutine;

        private GameObject _target;
        private Creature _creature;
        private Patrol _patrol;
        private SpawnListComponent _particles;
        private Animator _animator;

        private readonly int isDeadKey = Animator.StringToHash("is-dead"); 

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _creature = GetComponent<Creature>();
            _particles = GetComponent<SpawnListComponent>();
            _patrol = GetComponent<Patrol>();
        }

        void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        public void OnTargetInVision(GameObject go)
        {
            if (_isDead) return;

            _target = go;

            StartState(ReactOnTarget());
        }

        private IEnumerator ReactOnTarget()
        {
            LookAtTarget();
            _particles.Spawn("Detect");
            yield return new WaitForSeconds(_detectCooldown);

            StartState(GoToTarget());
        }
        private void LookAtTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(Vector2.zero);
            _creature.UpdateSpriteDirection(direction);
        }

        private IEnumerator GoToTarget()
        {
            while (_visionRaycastChecker.IsRaycastHitTarget())
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
            StartState(_patrol.DoPatrol());
        }

        private IEnumerator Attack()
        {
            while (_attackRange.IsTouchingLayer)
            {
                _creature.SetDirection(Vector2.zero);
                _creature.StartAttackAnimation();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(GoToTarget());
        }

        public void OnDie()
        {
            _isDead = true;
            _creature.SetDirection(Vector2.zero);

            if(TryGetComponent(out CapsuleCollider2D collider))
            {
                collider.direction = CapsuleDirection2D.Horizontal;
                this.gameObject.layer = LayerMask.NameToLayer("Trash");
            }

            _animator.SetBool(isDeadKey, true); 

            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }
        }

        private void StartState(IEnumerator coroutine)
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }
            _currentCoroutine = coroutine;
            StartCoroutine(coroutine);
        }

        private void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(direction);
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }
    }
}
