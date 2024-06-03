using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnlockerComponent : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private int _numberOfStates;
    [SerializeField] public int _currentState;

    [SerializeField] private UnityEvent _onNextState;

    private Animator _animator;
    static readonly int stateKey = Animator.StringToHash("state");

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        SetStateForAnimator();
    }

    public void NextState()
    {
        if (_currentState < _numberOfStates)
        {
            _currentState++;
        }
        else if (_currentState == _numberOfStates)
        {
            _currentState = 1;
        }

        SetStateForAnimator();
        _onNextState?.Invoke();
    }

    private void SetStateForAnimator()
    {
        _animator.SetInteger(stateKey, _currentState);
    }
}
