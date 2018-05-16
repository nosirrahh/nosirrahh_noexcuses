using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        None,
        Idle,
        Moving,
        Dying
    }

    public float health = 100;
    public float speed = 1;
    public uint damage = 1;

    public float pathPointThreshold = 0.5F;
    public int currentPathPoint;
    public List<Transform> path;
    public EnemyState state;
    
    private void Awake ()
    {
        ChangeState (EnemyState.Moving);
    }

    private void Update ()
    {
        Move ();
    }

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
                currentPathPoint = 0;
                if (path == null || path.Count == 0)
                    ChangeState (EnemyState.Idle);
                break;
            case EnemyState.Dying:
                Destroy (gameObject);
                break;
            default:
                break;
        }
    }

    public void RemoveHealth (uint healhToRemove)
    {
        health -= healhToRemove;
        if (health <= 0)
            ChangeState (EnemyState.Dying);
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
                GameManager.Instance.RemoveHealth (damage);
                ChangeState (EnemyState.Dying);
            }
                
        }
    }
}
