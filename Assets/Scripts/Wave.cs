using System.Collections;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;    // The enemy prefab to spawn
    public int enemyCount;            // Number of enemies to spawn in this wave
    public float spawnInterval;       // Interval between each enemy spawn
}
