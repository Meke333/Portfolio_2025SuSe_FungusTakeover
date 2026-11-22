using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyWave
{
    public List<GameObject> enemies = new List<GameObject>();
    [Tooltip("Time until this wave of enemies spawns since the last one (also for first wave!!)")]
    public float timeTillSpawn = .0f;
}
