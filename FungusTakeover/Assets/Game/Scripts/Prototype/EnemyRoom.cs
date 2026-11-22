using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyRoom
{
    public List<EnemyWave> waves = new List<EnemyWave>();
    [Tooltip("Determines in which room the enemies are going to be spawned")]
    [Min(0)] public int roomNumber = 0;

}