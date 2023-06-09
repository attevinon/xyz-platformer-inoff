using System;
using UnityEngine;
using UnityEngine.Events;

public class EnterTriggerComponent : MonoBehaviour
{
    [SerializeField] private string[] _tags;
    [SerializeField] private EnterEvent _action;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var tag in _tags)
        {
            if (collision.CompareTag(tag))
            {
                _action?.Invoke(collision.gameObject);
                return;
            }
        }
    }
}
