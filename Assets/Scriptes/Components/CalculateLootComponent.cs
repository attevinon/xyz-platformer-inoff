using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class CalculateLootComponent : MonoBehaviour
    {
        [SerializeField][Min(0)] private int _minLootAmount;
        [SerializeField] private int _maxLootAmount;
        [SerializeField] private bool _isSpawnOnEnable = false;
        [SerializeField] private List<LootData> _lootVariations;
        [SerializeField] private LootEvent _onLootCalculated;

        private GameObject[] _currentLoot;
        private List<LootData> _sortedLootVariations;
        private float _totalPropability;
        private int _lootAmount;

        private void OnEnable()
        {
            if (_isSpawnOnEnable)
            {
                CalculateLoot();
            }
        }

        [ContextMenu("Calculate loot")]
        public void CalculateLoot()
        {
            _lootAmount = UnityEngine.Random.Range(_minLootAmount, _maxLootAmount);
            if (_lootAmount == 0)
            {
                Debug.LogWarning("Не установлено количество лута для " + gameObject.name);
                return;
            }
            _currentLoot = new GameObject[_lootAmount];
            _sortedLootVariations = new List<LootData>(_lootVariations.OrderBy(loot => loot.propability));
            _totalPropability = _lootVariations.Sum(loot => loot.propability);

            int i = 0;

            while (i < _lootAmount)
            {
                var lootPrefab = GetRandomLoot();
                if (lootPrefab != null)
                {
                    _currentLoot[i] = lootPrefab;
                    i++;
                }
                //если null то i не изменяется и снова подбирается предмет на этот индекс
            }
            _onLootCalculated?.Invoke(_currentLoot);
        }

        private GameObject GetRandomLoot()
        {
            float randomValue = UnityEngine.Random.value * _totalPropability;
            float current = 0f;
            foreach (var loot in _sortedLootVariations)
            {
                current += loot.propability;

                if (current >= randomValue)
                {
                    return loot.prefab;
                }
            }
            return null;
        }

        [Serializable]
        public class LootData
        {
            [SerializeField] private GameObject _prefab;
            [SerializeField][Range(1, 100)] private float _probability;

            public GameObject prefab => _prefab;
            public float propability => _probability;
        }
    }

    [Serializable]
    public class LootEvent : UnityEvent<GameObject[]>
    {

    }
}