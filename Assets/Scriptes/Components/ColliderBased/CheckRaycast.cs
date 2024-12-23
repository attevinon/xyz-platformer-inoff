using UnityEditor;
using UnityEngine;

namespace PixelCrew.Components.ColliderBased 
{
    [RequireComponent(typeof(Collider2D))]
    public class CheckRaycast : MonoBehaviour
    {
        [SerializeField] private LayerMask _layersToCheck;
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private float _originY;
        [SerializeField] private bool _flip;
        [SerializeField] private EnterEvent _onRaycastHitTarget;
        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        public bool IsRaycastHitTarget()
        {
            var otherCollider = GetRaycastHitCollider();
            if(otherCollider == null) return false; 
            Debug.Log($"IsRaycastHitTarget: {otherCollider.gameObject.name}");
            return (_targetLayer.value & (1 << otherCollider.gameObject.layer)) != 0;
        }
           
        public void Raycast()
        {
            var otherCollider = GetRaycastHitCollider();
            if (otherCollider == null) return;

            if ((_targetLayer.value & (1 << otherCollider.gameObject.layer)) != 0)
            {
                _onRaycastHitTarget?.Invoke(otherCollider.gameObject);
            }
        }

        private Vector2 GetRayOrigin()
        {
            float originX =  transform.position.x + (_collider.bounds.extents.x * GetRayDirection() * -1);
            return new Vector2(originX, _originY + transform.position.y);
        }

        private int GetRayDirection()
        {
            return transform.parent.position.x - transform.position.x > 0 ? -1 : 1;
        }

        private float GetRayLength() => _collider.bounds.size.x;

        private Collider2D GetRaycastHitCollider()
        {
            var hit = Physics2D.Raycast(
                GetRayOrigin(),
                new Vector2(GetRayDirection(), 0),
                GetRayLength(),
                _layersToCheck);

            return hit.collider;
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_collider != null)
            {
                Handles.color = Color.red;
                var endPointX = GetRayOrigin().x + (GetRayLength() * GetRayDirection());
                Handles.DrawLine(GetRayOrigin(), new Vector2(endPointX, GetRayOrigin().y));
                Handles.DrawSolidDisc(new Vector3(GetRayOrigin().x, GetRayOrigin().y), Vector3.forward, 0.05f);
            }
        }
#endif

    }
}
