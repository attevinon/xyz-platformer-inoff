using PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LayerChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _layer;
    [NonSerialized] public bool IsTouchingLayer;

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IsTouchingLayer = _collider.IsTouchingLayers(_layer);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IsTouchingLayer = _collider.IsTouchingLayers(_layer);
    }

    private void OnDrawGizmos()
    {
        if(_collider != null)
        {
            Gizmos.color = (IsTouchingLayer) ? HandlesUtils.TransperentYellow : HandlesUtils.TransperentBlue;
            Gizmos.DrawCube((Vector2)transform.position + _collider.offset, _collider.bounds.size);
        }
    }
}
