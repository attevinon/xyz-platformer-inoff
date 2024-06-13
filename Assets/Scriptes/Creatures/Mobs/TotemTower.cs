using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Creatures.Mobs;
using PixelCrew.Utils;
using PixelCrew.Components.Health;
using System.Linq;

public class TotemTower : MonoBehaviour
{
    [SerializeField] private List<ShootingTrapAI> _totems;
    [SerializeField] private Cooldown _cooldown;

    private int _currentTotem;

    private void Start()
    {
        foreach (var totem in _totems)
        {
            totem.enabled = false;
            var health = totem.gameObject.GetComponent<HealthComponent>();
            health.OnDie.AddListener(() => OnTotemDead(totem));
        }
    }

    private void Update()
    {
        if(_totems.Count == 0)
        {
            this.enabled = false;
            Destroy(gameObject, 2f);
        }

        var isTargetInVision = _totems.Any(x => x.Vision.IsTouchingLayer);
        if (isTargetInVision)
        {
            if (!_cooldown.IsReady) return;
            _totems[_currentTotem].OnTargetInVision?.Invoke();
            _cooldown.Reset();
            _currentTotem = (int)Mathf.Repeat(_currentTotem + 1, _totems.Count);
        }
    }

    private void OnTotemDead(ShootingTrapAI totem)
    {
        int index = _totems.IndexOf(totem);
        _totems.Remove(totem);
        if (index < _currentTotem)
        {
            _currentTotem--;
        }
    }
}
