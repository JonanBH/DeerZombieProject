using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    #region Fields
    public GameObject prefab;
    public int cost = 0;
    public int minSpawnWave = 0;
    #endregion
}
