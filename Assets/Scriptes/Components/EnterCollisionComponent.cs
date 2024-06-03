using System;
using UnityEngine;
using UnityEngine.Events;
using PixelCrew.Utils;

public class EnterCollisionComponent : MonoBehaviour
{
    [SerializeField] private LayerMask _layer = ~0;
    [SerializeField] private string[] _tags;
    [SerializeField] private EnterEvent _action;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.IsInLayer(_layer)) return;

        foreach (var tag in _tags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                _action?.Invoke(collision.gameObject);
                return;
            }
        }
    }
}

[Serializable]
public class EnterEvent : UnityEvent<GameObject>
{

}
