using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Classes

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

    #endregion

    #region Enumeration

    public enum SpawnerState
    {
        None,
        Idle,
        InitialDelay,
        Spawning,
        Paused,
        Ended
    }

    #endregion

    #region Fields

    public bool loop;
    public float initialDelay;
    public float intervalBetweenWaves;
    public Wave[] waves;
    
    public SpawnerState state;
    public int currentWave;

    public Coroutine spawnCoroutine;

    #endregion

    #region Public Methods

    public void StartSpawn ()
    {
        spawnCoroutine = StartCoroutine (Spawn ());
    }

    #endregion

    #region Coroutine Methods

    IEnumerator Spawn ()
    {
        // Delay inicial
        state = SpawnerState.InitialDelay;
        yield return new WaitForSecondsRealtime (initialDelay);

        state = SpawnerState.Spawning;
        GameObject spanwnedObject;

        for (currentWave = 0; currentWave < waves.Length; currentWave++)
        {
            for (int o = 0; o < waves[currentWave].waveObject.amount; o++)
            {
                spanwnedObject = Instantiate (waves[currentWave].waveObject.objectToSpawn);
                spanwnedObject.transform.position = transform.position;
                // Intervalo entre objetos de uma Wave
                if (o < waves[currentWave].waveObject.amount - 1)
                    yield return new WaitForSecondsRealtime (waves[currentWave].intervalBetweenObjects);
            }

            // Intervalo entre Waves
            yield return new WaitForSecondsRealtime (intervalBetweenWaves);
        }

        if (loop)
            StartSpawn ();
        else
            state = SpawnerState.Ended;
    }

    #endregion
}
