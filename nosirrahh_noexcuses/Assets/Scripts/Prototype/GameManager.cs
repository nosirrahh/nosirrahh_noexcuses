using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
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

    public int initialCurrency = 100;
    public int currentCurrency;
    
    public Health health;
    public Path path;
    public EnemySpawner enemySpawner;

    #endregion

    #region Unity Methods

    public void Awake ()
    {
        health = GetComponent<Health> ();
        health.OnHealthChange += HandleOnHealthChange;
    }
    
    public void Start ()
    {
        currentCurrency = initialCurrency;
        enemySpawner.StartSpawn ();
    }

    #endregion

    #region Public Methods

    public void AddCurrency (int currencyToAdd)
    {
        currentCurrency += Mathf.Abs (currencyToAdd);
    }

    public bool RemoveCurrency (int currencyToRemove)
    {
        currencyToRemove = Mathf.Abs (currencyToRemove);
        if (currentCurrency - currencyToRemove < 0)
        {
            Debug.Log ("[GameManager] (RemoveCurrenty) Não possui a quantidade suficiente de dinheiro para a ação.");
            return false;
        }

        currentCurrency -= currencyToRemove;
        return true;
    }

    public void RemoveHealth (uint healthToRemove)
    {
        health.RemoveHealth (healthToRemove);
    }

    #endregion

    #region Health Events Handlers

    private void HandleOnHealthChange (Health.HealthEvent eventData)
    {
        if (eventData.currentHealth <= 0)
        {
            Debug.Log ("[GameManager] (HandleOnHealthChange) A VIDA CHEGOU A ZERO!");
        }
    }

    #endregion
}
