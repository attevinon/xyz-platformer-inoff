using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures
{
    [RequireComponent(typeof(Creature))]
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerChecker _nextStepChecker;
        [SerializeField] private LayerChecker _obstacleChecker;
        [SerializeField] private float _stayOnBorderForSec = 1f;

        private Creature _creature;
        private int _directionX;
        void Awake()
        {
            _creature = GetComponent<Creature>();
            _directionX = -1;
        }
        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (!_nextStepChecker.IsTouchingLayer || _obstacleChecker.IsTouchingLayer)
                {
                    _creature.SetDirection(Vector2.zero);
                    _directionX = -_directionX;
                    yield return new WaitForSeconds(_stayOnBorderForSec);
                }

                _creature.SetDirection(new Vector2(_directionX, 0));

                yield return null;
            }
        }
    }
}
