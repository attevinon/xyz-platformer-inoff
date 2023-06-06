using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class InteractableComponent : MonoBehaviour
{
    [SerializeField] private UnityEvent _action;

    public void Interact()
    {
        _action?.Invoke();
    }
}
