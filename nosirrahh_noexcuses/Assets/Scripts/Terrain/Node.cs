using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private void OnDrawGizmosSelected ()
    {
        float distance = 2;
        float inclination = 1F;

        float i1 = 1F;
        float i2 = 0.5F;

        for (float i = -distance; i <= distance; i += i1)
        {
            for (float j = -inclination; j <= inclination; j += i2)
            {
                Vector3 v1 = new Vector3 (distance, j, i);
                Gizmos.DrawLine (transform.position, transform.position + v1);
                Vector3 v2 = new Vector3 (-distance, j, i);
                Gizmos.DrawLine (transform.position, transform.position + v2);

                Vector3 v3 = new Vector3 (i, j, distance);
                Gizmos.DrawLine (transform.position, transform.position + v3);
                Vector3 v4 = new Vector3 (i, j, -distance);
                Gizmos.DrawLine (transform.position, transform.position + v4);
            }
        }
    }
}
