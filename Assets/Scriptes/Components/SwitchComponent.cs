using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchComponent : MonoBehaviour
{
    [SerializeField] private string _animationKey;

    private Animator _animator;
    private bool _state;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Switch()
    {
        _state = !_state;
        _animator.SetBool(_animationKey, _state);
    }

    [ContextMenu("Switch")]
    public void SwitchIn()
    {
        Switch();
    }
}
