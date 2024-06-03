using UnityEngine.Events;
using UnityEngine;

namespace PixelCrew.Components.Interactions
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;

        public void Interact()
        {
            _action?.Invoke();
        }
    }
}
