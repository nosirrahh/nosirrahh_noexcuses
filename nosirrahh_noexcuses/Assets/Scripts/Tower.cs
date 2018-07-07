using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    #region Enumerations

    public enum TowerState
    {
        Building,
        SearchingTarget,
        Attacking
    }

    #endregion

    #region Fields

    public int price;
    public uint damage;
    public float firerate;
    public float range;
    public Projectile projectile;

    public TowerState state;
    public Coroutine searchTargetCoroutine;
    public Coroutine attackCoroutine;

    public Enemy target;
    public LayerMask enemyLayerMask;

    #endregion

    #region Unity Methods

    private void Start ()
    {
        ChangeState (TowerState.SearchingTarget);
    }

    private void OnDrawGizmos ()
    {
        Gizmos.DrawWireSphere (transform.position, range);
        Gizmos.color = Color.red;
        if (target != null)
            Gizmos.DrawLine (transform.position, target.transform.position);
    }

    #endregion

    #region Private Methods

    private void ChangeState (TowerState toState)
    {
        if (toState == state)
            return;

        state = toState;

        switch (state)
        {
            case TowerState.Building:
                break;
            case TowerState.SearchingTarget:
                searchTargetCoroutine = StartCoroutine (SearchTargetCoroutine ());
                break;
            case TowerState.Attacking:
                attackCoroutine = StartCoroutine (AttackCoroutine ());
                break;
            default:
                break;
        }
    }

    private bool IsTargetInRange ()
    {
        float distance = Vector3.Distance (transform.position, target.transform.position);
        return distance <= range;
    }

    #endregion

    #region Coroutine Methods

    IEnumerator SearchTargetCoroutine ()
    {
        while (state == TowerState.SearchingTarget)
        {
            if (target != null)
            {
                ChangeState (TowerState.Attacking);
            }
            else
            {
                Collider[] colliders = Physics.OverlapSphere (transform.position, range, enemyLayerMask);
                if (colliders != null && colliders.Length > 0)
                {
                    for (int i = 0; i < colliders.Length && target == null; i++)
                    {
                        target = colliders[i].GetComponent<Enemy> ();
                        if (target != null && target.state == Enemy.EnemyState.Dying)
                            target = null;
                    }
                        
                }
                else
                {
                    target = null;
                }
            }
            // Executa 1 vez por Update
            yield return new WaitForSeconds (0F);
        }
    }

    IEnumerator AttackCoroutine ()
    {
        while (state == TowerState.Attacking)
        {
            if (target == null || target.state == Enemy.EnemyState.Dying)
            {
                ChangeState (TowerState.SearchingTarget);
            }
            else if (!IsTargetInRange())
            {
                target = null;
                ChangeState (TowerState.SearchingTarget);
            }
            else
            {
                Projectile p = Instantiate (projectile, transform.position, Quaternion.identity);
                p.BuildProjectile (target.gameObject, damage);
                yield return new WaitForSecondsRealtime (firerate);
            }
        }
    }

    #endregion
}
