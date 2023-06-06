using System;
using UnityEngine;
using UnityEngine.Events;
using static EnterCollisionComponent;

public class EnterTriggerComponent : MonoBehaviour
{
    [SerializeField] private string _tag;
    [SerializeField] private EnterEvent _action;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(_tag))
            _action?.Invoke(collision.gameObject);
    }
}
