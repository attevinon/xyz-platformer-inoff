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
        Gizmos.color = (isTouchingLayer) ? Color.yellow : Color.cyan;

        if(_collider != null)
            Gizmos.DrawSphere(transform.position, _collider.bounds.extents.y);
    }
}
