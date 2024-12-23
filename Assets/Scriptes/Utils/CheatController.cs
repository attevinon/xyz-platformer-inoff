using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PixelCrew.Utils
{
    public class CheatController : MonoBehaviour
    {
        [SerializeField] private CheatItem[] _cheats;
        [SerializeField] private float _inputLifetime;

        private float _inputTime;
        private string _currentInput;

        void Awake()
        {
            Keyboard.current.onTextInput += OnTextInput;
        }

        private void OnTextInput(char inputChar)
        {
            _currentInput += inputChar;
            _inputTime = _inputLifetime;
            FindCheats();
        }

        void Update()
        {
            if (_inputTime < 0)
            {
                _currentInput = string.Empty;
            }
            else
            {
                _inputTime -= Time.deltaTime;
            }
        }

        void FindCheats()
        {
            foreach (var cheat in _cheats)
            {
                if (_currentInput.Contains(cheat.name))
                {
                    cheat.action?.Invoke();
                    _currentInput = string.Empty;
                }
            }
        }

        void OnDestroy()
        {
            Keyboard.current.onTextInput -= OnTextInput;
        }
    }

    [Serializable]
    public class CheatItem
    {
        public string name;
        public UnityEvent action;
    }
}