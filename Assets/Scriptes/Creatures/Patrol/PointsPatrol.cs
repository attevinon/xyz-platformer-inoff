using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures
{
    [RequireComponent(typeof(Creature))]
    public class PointsPatrol : Patrol
    {
        [SerializeField] private Transform[] _points;
        [SerializeField] private LayerChecker _nextStepChecker;
        [SerializeField] private LayerChecker _obstacleChecker;
        [SerializeField] private float _stayOnPointForSec = 1f;
        [SerializeField] private float _treshhold = 0.5f;
        private int _destinationPointIndex = 0;

        private Creature _creature; 
        void Awake()
        {
            _creature = GetComponent<Creature>();
        }
        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (IsOnPoint() || _obstacleChecker.IsTouchingLayer || !_nextStepChecker.IsTouchingLayer)
                {
                    //todo: check are points available just like with hero
                    //turn on like platform mode if can't reach points
                    _destinationPointIndex = (int)Mathf.Repeat(_destinationPointIndex + 1, _points.Length);

                    _creature.SetDirection(Vector2.zero);
                    yield return new WaitForSeconds(_stayOnPointForSec);
                }

                var direction = _points[_destinationPointIndex].position - transform.position;
                direction.y = 0;
                _creature.SetDirection(direction.normalized);

                yield return null;
            }
        }

        private bool IsOnPoint()
        {
            return (_points[_destinationPointIndex].position - transform.position).magnitude < _treshhold;
        }
    }
}
