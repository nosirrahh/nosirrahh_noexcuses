using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public WaveObject waveObject;
        public float intervalBetweenObjects;
    }

    [System.Serializable]
    public class WaveObject
    {
        public GameObject objectToSpawn;
        public int amount;
    }

    public float initialDelay;
    public float intervalBetweenWaves;
    public Wave[] waves;
}
