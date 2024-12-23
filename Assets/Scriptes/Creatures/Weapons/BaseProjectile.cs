using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] private bool _flip;
        [SerializeField] protected float Speed;

        protected int Direction;
        protected Rigidbody2D Rigidbody;

        void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            Direction = this.transform.lossyScale.x > 0 ? -1 : 1;
            Direction = _flip ? -Direction : Direction;

        }
    }
}
