using System.ComponentModel;
using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class SinusoidalProjectile : BaseProjectile
    {
        [Description("Изменяет частоту")]
        [SerializeField] private float _frequency = 1f;

        [Description("Изменяет высоту")]
        [SerializeField] private float _amplitude = 1f;

        private float _originalY;

        protected override void Start()
        {
            base.Start();
            _originalY = Rigidbody.position.y;
        }

        private void FixedUpdate()
        {
            Vector2 position = Rigidbody.position;
            position.x += Direction * Speed;
            position.y = _originalY + Mathf.Sin(position.x * _frequency) * _amplitude;
            Rigidbody.MovePosition(position);
        }
    }
}
