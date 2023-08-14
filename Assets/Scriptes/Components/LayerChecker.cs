using PixelCrew.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    public bool isTouchingLayer;
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isTouchingLayer = _collider.IsTouchingLayers(_groundLayer);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTouchingLayer = _collider.IsTouchingLayers(_groundLayer);
    }

    private void OnDrawGizmos()
    {
        if(_collider != null)
        {
            Gizmos.color = (isTouchingLayer) ? HandlesUtils.TransperentYellow : HandlesUtils.TransperentBlue;
            Gizmos.DrawCube(transform.position, _collider.bounds.size);
        }
    }
}
