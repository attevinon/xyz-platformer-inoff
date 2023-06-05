﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private int _frameRate;
    [SerializeField] private bool _loop;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private UnityEvent _onComplete;

    private SpriteRenderer _render;
    private float _secondsPerFrame;
    private int _currentSpriteIndex;
    private float _nextFrameTime;

    private void Awake()
    {
        _render = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _secondsPerFrame = 1f / _frameRate;
        _nextFrameTime = Time.time + _secondsPerFrame;
    }

    private void Update()
    {
        if (_nextFrameTime > Time.time)
            return;

        if(_currentSpriteIndex >= _sprites.Length)
        {
            if (_loop)
            {
                _currentSpriteIndex = 0;
            }
            else
            {
                enabled = false;
                _onComplete?.Invoke();
                return;
            }
        }

        _render.sprite = _sprites[_currentSpriteIndex];
        _nextFrameTime += _secondsPerFrame;
        _currentSpriteIndex++;
    }
}
