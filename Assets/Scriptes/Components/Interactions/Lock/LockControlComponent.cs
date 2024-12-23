using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Interactions.Lock
{
    public class LockControlComponent : MonoBehaviour
    {
        [SerializeField] private int[] _correctCombination;
        [SerializeField] private UnlockerComponent[] _unlockers;
        [SerializeField] private UnityEvent _onCorrectCombination;
        [SerializeField] private UnityEvent _onBackToWrongCombination;

        private int _numberOfUnlockers;
        private int[] _currentResult;
        private bool isCorrectCombination;

        void Start()
        {
            //_unlockers.Length or _correctCombination.Length ???
            _numberOfUnlockers = _unlockers.Length;
            _currentResult = new int[_numberOfUnlockers];

            for (int i = 0; i < _numberOfUnlockers; i++)
            {
                _currentResult[i] = _unlockers[i]._currentState;
            }
        }

        public void CheckResult(int id)
        {
            _currentResult[id] = _unlockers[id]._currentState;

            for (int i = 0; i < _numberOfUnlockers; i++)
            {
                if (_currentResult[i] != _correctCombination[i])
                {
                    if (isCorrectCombination)
                    {
                        isCorrectCombination = false;
                        _onBackToWrongCombination?.Invoke();
                    }
                    return;
                }
            }
            isCorrectCombination = true;
            _onCorrectCombination?.Invoke();
        }

        public void CheckResult()
        {
            for (int i = 0; i < _correctCombination.Length; i++)
            {

            }
        }
    }
}