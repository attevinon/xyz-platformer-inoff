using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnerData[] _spawners;

        public void Spawn(string id)
        {
            var spawnerData = _spawners.FirstOrDefault(spawner => spawner.Id == id);
            spawnerData?.Component.Spawn();
        }

        public void SpawnAll()
        {
            foreach (var spawnerData in _spawners)
            {
                spawnerData?.Component.Spawn();
            }
        }

    }

    [Serializable]
    public class SpawnerData
    {
        public string Id;
        public SpawnComponent Component;
    }
}
