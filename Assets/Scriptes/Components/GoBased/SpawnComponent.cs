using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _prefabToSpawn;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var instantiate = Instantiate(_prefabToSpawn, _spawnPoint.position, Quaternion.identity);
            instantiate.transform.localScale = _spawnPoint.lossyScale;
            instantiate.SetActive(true);
        }
    }
}
