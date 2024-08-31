using UnityEngine;

namespace PixelCrew.Utils
{
    public static class GameObjectExtensions
    {
        public static bool IsInLayer(this GameObject obj, LayerMask layer)
        {
            return layer == (layer | 1 << obj.layer);
        }

        public static TInterfaceType GetInterface<TInterfaceType>(this GameObject obj)
        {

            var components = obj.GetComponents<Component>();

            foreach (var component in components)
            {
                if (component is TInterfaceType type)
                    return type;
            }

            return default;
        }
    }
}

