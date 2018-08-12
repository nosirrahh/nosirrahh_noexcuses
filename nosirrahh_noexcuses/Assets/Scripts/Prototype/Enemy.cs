using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    #region Enumeration

    public enum EnemyState
    {
        None,
        Idle,
        Moving,
        Dying,
        ReachTarget
    }

    #endregion

    #region Fields
    
    public float speed = 1;
    public uint damage = 1;
    public int worth;

    public float pathPointThreshold = 0.5F;
    public int currentPathPoint;
    public List<Transform> path;
    public EnemyState state;
    public Health health;

    #endregion

    #region Unity Methods

    private void Awake ()
    {
        ChangeState (EnemyState.Moving);
        health = GetComponent<Health> ();
        health.OnHealthChange += HandleOnHealthChange;
    }

    private void OnDestroy ()
    {
        health.OnHealthChange -= HandleOnHealthChange;
    }
    
    private void Update ()
    {
        Move ();
    }

    #endregion

    #region Public Methods
    
    #endregion

    #region Private Methods

    private void ChangeState (EnemyState toState)
    {
        if (toState == state)
            return;

        state = toState;

        switch (state)
        {
            case EnemyState.None:
                break;
            case EnemyState.Idle:
                break;
            case EnemyState.Moving:
                path = GameManager.Instance.path.path;
                currentPathPoint = 0;
                if (path == null || path.Count == 0)
                    ChangeState (EnemyState.Idle);
                break;
            case EnemyState.Dying:
                GameManager.Instance.AddCurrency (worth);
                Destroy (gameObject);
                break;
            case EnemyState.ReachTarget:
                GameManager.Instance.RemoveHealth (damage);
                Destroy (gameObject);
                break;
            default:
                break;
        }
    }

    private void Move ()
    {
        if (state != EnemyState.Moving)
            return;

        Vector3 direction = (path[currentPathPoint].position - transform.position).normalized;
        Vector3 translation = direction * speed * Time.deltaTime;

        float magnitude = translation.magnitude;
        float distance = Vector3.Distance (transform.position, path[currentPathPoint].position);

        if (magnitude > distance)
            translation = path[currentPathPoint].position - transform.position;

        transform.Translate (translation);

        distance = Vector3.Distance (transform.position, path[currentPathPoint].position);
        if (distance <= pathPointThreshold)
        {
            currentPathPoint++;
            if (currentPathPoint >= path.Count)
            {
                ChangeState (EnemyState.ReachTarget);
            }

        }
    }

    #endregion

    #region Health Events Handlers

    private void HandleOnHealthChange (Health.HealthEvent eventData)
    {
        if (eventData.currentHealth <= 0)
        {
            ChangeState (EnemyState.Dying);
        }
    }

    #endregion
}
