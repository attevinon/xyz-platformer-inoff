using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnComponent : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _prefabToSpawn;

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        var instantiate = Instantiate(_prefabToSpawn, _spawnPoint.position, Quaternion.identity);
        instantiate.transform.localScale = _spawnPoint.lossyScale;
    }
}
