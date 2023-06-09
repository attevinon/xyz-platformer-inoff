using System;
using UnityEngine;
using UnityEngine.Events;

public class EnterCollisionComponent : MonoBehaviour
{
    [SerializeField] private string[] _tags;
    [SerializeField] private EnterEvent _action;

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
