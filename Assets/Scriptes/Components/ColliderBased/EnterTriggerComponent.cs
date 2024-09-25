using UnityEngine;

namespace PixelCrew.Components.ColliderBased
{
    public class EnterTriggerComponent : BaseEnterComponent
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (IsInConditions(collider.gameObject))
            {
                _action?.Invoke(collider.gameObject);
            }
        }
    }
}
