using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    #region Enumerations

    public enum ProjectileState
    {
        None,
        SeekingTarget,
        Destroying
    }

    #endregion

    #region Fields

    public float speed = 10F;
    
    private Enemy target;
    private uint damage;
    private ProjectileState state;

    #endregion

    #region Unity Methods

    private void Awake ()
    {
        Rigidbody r = GetComponent<Rigidbody> ();
        r.useGravity = false;
        BoxCollider b = GetComponent<BoxCollider> ();
        b.isTrigger = true;
    }

    private void Update ()
    {
        SeekTarget ();
    }

    private void OnTriggerEnter (Collider other)
    {
        if (state != ProjectileState.SeekingTarget)
            return;

        if (other.transform == target.transform)
        {
            Enemy e = target.GetComponent<Enemy> ();
            if (e != null)
                e.RemoveHealth (damage);
            ChangeState (ProjectileState.Destroying);
        }
    }

    #endregion

    #region Public Methods

    public void BuildProjectile (Enemy target, uint damage)
    {
        this.target = target;
        this.damage = damage;
        ChangeState (ProjectileState.SeekingTarget);
    }

    #endregion

    #region Private Methods

    private void ChangeState (ProjectileState toState)
    {
        if (toState == state)
            return;

        state = toState;

        switch (state)
        {
            case ProjectileState.None:
                break;
            case ProjectileState.SeekingTarget:
                break;
            case ProjectileState.Destroying:
                Destroy (gameObject);
                break;
            default:
                break;
        }
    }

    private void SeekTarget ()
    {
        if (state != ProjectileState.SeekingTarget)
            return;

        if (target == null)
        {
            ChangeState (ProjectileState.Destroying);
            return;
        }

        Vector3 targetDirection = (target.transform.position - transform.position).normalized;
        Vector3 translation = targetDirection * speed * Time.deltaTime;
        transform.Translate (translation);
    }

    #endregion
}
