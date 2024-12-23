using PixelCrew.Components.Health;
using System.Collections.Generic;
using UnityEngine;

public class CirculationMovement : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _speed;

    private List<Rigidbody2D> _childrensRigidbodies;

    private void Awake()
    {
        _childrensRigidbodies = GetRemadeChildren();
    }

    private void Start()
    {
        PlaceChildren();

        foreach (var rigidbody in _childrensRigidbodies)
        {
            if (rigidbody.gameObject.TryGetComponent(out HealthComponent health))
            {
                health.OnDie.AddListener(() => _childrensRigidbodies.Remove(rigidbody));
                Debug.Log("podpiska oformlena");
            }
        }
    }

    private void FixedUpdate()
    {
        if(_childrensRigidbodies.Count == 0)
        {
            this.enabled = false;
            return;
        }

        if (_speed == 0) return;

        foreach (var rigidbody in _childrensRigidbodies)
        {
            if(rigidbody == null) continue;
            var localPosition = rigidbody.transform.localPosition;
            float startAngle = Mathf.Atan2(localPosition.y, localPosition.x);
            Vector2 newPosition = GetPointOnCircle(startAngle + _speed,
                new Vector2(transform.position.x, transform.position.y));
            rigidbody?.MovePosition(newPosition);
        }
    }

    private Vector2 GetPointOnCircle(float angle, Vector2 circleCenter)
    {
        Vector2 position;
        position.x = circleCenter.x + _radius * Mathf.Cos(angle);
        position.y = circleCenter.y + _radius * Mathf.Sin(angle);
        return position;
    }

    private void PlaceChildren()
    {
        float angleBetweenChildren = 2 * Mathf.PI / _childrensRigidbodies.Count;
        float currentAngle = 0;

        foreach (var rigidbody in _childrensRigidbodies)
        {
            rigidbody.gameObject.transform.localPosition = GetPointOnCircle(currentAngle, Vector2.zero);
            currentAngle += angleBetweenChildren;
        }
    }

    private List<Rigidbody2D> GetRemadeChildren()
    {
        var _childrenTransformArray = GetComponentsInChildren<Rigidbody2D>();
        return new List<Rigidbody2D>(_childrenTransformArray);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _childrensRigidbodies = GetRemadeChildren();
        if (_childrensRigidbodies.Count == 0) return;

        PlaceChildren();
    }

    private void OnDrawGizmosSelected()
    {
        var position = this.transform.position;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(position, _radius);
    }
    
#endif
}
