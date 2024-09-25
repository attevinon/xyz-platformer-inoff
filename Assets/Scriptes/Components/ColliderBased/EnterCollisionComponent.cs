using UnityEngine;

namespace PixelCrew.Components.ColliderBased
{
    public class EnterCollisionComponent : BaseEnterComponent
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsInConditions(collision.gameObject))
            {
                _action?.Invoke(collision.gameObject);
            }
        }
    }
}

