using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnListComponent : MonoBehaviour
{
    [SerializeField] private SpawnerData[] _spawners;

    public void SpawnAll()
    {
        foreach (var spawner in _spawners)
        {
            spawner?.Component.Spawn();
        }
    }
    
}

[Serializable]
public class SpawnerData
{
    public string Id;
    public SpawnComponent Component;
}
