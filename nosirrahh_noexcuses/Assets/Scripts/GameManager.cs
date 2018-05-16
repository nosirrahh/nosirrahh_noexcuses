using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager> ();
            return instance;
        }
    }

    #endregion

    #region Fields

    public float health = 100;
    public float currentHealth;

    #endregion

    #region Unity Methods

    public void Awake ()
    {
        currentHealth = health;
    }

    #endregion

    #region Public Methods

    public void RemoveHealth (uint healhToRemove)
    {
        health -= healhToRemove;
        if (health <= 0)
        {
            Debug.Log ("[GameManager] Você perdeu!");
        }
    }

    #endregion
}
