using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AnimatorComponent : MonoBehaviour
    {
        [SerializeField][Range(1, 60)] private int _frameRate = 10;
        [SerializeField] private AnimationState[] _states;
        [SerializeField] bool isCustomFirstClip;
        [SerializeField] int firstClip;

        private SpriteRenderer _render;
        private int _currentStateIndex;
        private int _currentFrameIndex;
        private float _secondsPerFrame;
        private float _nextFrameTime;
        //private bool _isPlaying = true; 

        private void Awake()
        {
            _render = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _secondsPerFrame = 1f / _frameRate;
            _currentFrameIndex = 0;
        }

        private void Start()
        {
            if (isCustomFirstClip)
            {
                _currentStateIndex = firstClip;
            }
            StartAnimation();
        }

        private void Update()
        {
            if (_nextFrameTime > Time.time)
                return;

            PlayAnimation();

            _nextFrameTime += _secondsPerFrame;
        }

        private void StartAnimation()
        {
            _nextFrameTime = Time.time + _secondsPerFrame;
            enabled = true;
            _currentFrameIndex = 0;
        }

        private void PlayAnimation()
        {
            var state = _states[_currentStateIndex];

            if (_currentFrameIndex >= state.Sprites.Length)
            {
                if (state.Loop)
                {
                    _currentFrameIndex = 0;
                }
                else
                {
                    enabled = state.AllowNext;
                    state.OnComplete?.Invoke();

                    if (state.AllowNext)
                    {
                        _currentStateIndex = (int)Mathf.Repeat(_currentStateIndex + 1, _states.Length);
                        _currentFrameIndex = 0;
                    }

                    return;
                }
            }

            _render.sprite = state.Sprites[_currentFrameIndex];
            _currentFrameIndex++;
        }

        public void SetClip(string name)
        {
            for (int i = 0; i < _states.Length; i++)
            {
                if (_states[i].Name == name)
                {
                    _currentStateIndex = i;
                    StartAnimation();
                    return;
                }
            }

            Debug.Log($"There is no state with name {name} in {this.name}");
            //enabled = _isPlaying = false;
        }
    }

    [Serializable]
    public class AnimationState
    {
        [SerializeField] private string _name;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _allowNext;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private UnityEvent _onComplete;

        public string Name => _name;
        public bool Loop => _loop;
        public bool AllowNext => _allowNext;
        public Sprite[] Sprites => _sprites;
        public UnityEvent OnComplete => _onComplete;

    }
}