using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBuilder : MonoBehaviour
{
    #region Fields

    public float spacing = 1;
    public GameObject prefabNode;
    public List<GameObject> nodes;

    public float min_x_value;
    public float max_x_value;
    public float min_z_value;
    public float max_z_value;

    public LayerMask terrainLayerMask;

    #endregion

    #region Properties

    Vector3 Back_Left { get { return new Vector3 (min_x_value, 0, min_z_value); } }
    Vector3 Back_Right { get { return new Vector3 (max_x_value, 0, min_z_value); } }
    Vector3 Front_Left { get { return new Vector3 (min_x_value, 0, max_z_value); } }
    Vector3 Front_Right { get { return new Vector3 (max_x_value, 0, max_z_value); } }

    #endregion

    #region Unity Methods

    private void Start ()
    {
        CreateNodes ();
    }

    private void OnDrawGizmos ()
    {
        Gizmos.DrawLine (Back_Left, Back_Right);
        Gizmos.DrawLine (Back_Left, Front_Left);
        Gizmos.DrawLine (Front_Right, Back_Right);
        Gizmos.DrawLine (Front_Right, Front_Left);
    }

    #endregion

    #region Private Methods

    [ContextMenu ("DefineMinMax")]
    private void DefineMinMax ()
    {
        Collider[] sceneObjects = FindObjectsOfType<Collider> ();
        float current = 0;

        min_x_value = float.MaxValue;
        max_x_value = float.MinValue;
        min_z_value = float.MaxValue;
        max_z_value = float.MinValue;
        
        for (int i = 0; i < sceneObjects.Length; i++)
        {
            if (!IsTerrainLayer (sceneObjects[i].gameObject.layer))
                continue;

            current = sceneObjects[i].transform.position.x - (sceneObjects[i].bounds.size.x / 2F);
            if (current < min_x_value)
                min_x_value = current;

            current = sceneObjects[i].transform.position.x + (sceneObjects[i].bounds.size.x / 2F);
            if (current > max_x_value)
                max_x_value = current;

            current = sceneObjects[i].transform.position.z - (sceneObjects[i].bounds.size.z / 2F);
            if (current < min_z_value)
                min_z_value = current;

            current = sceneObjects[i].transform.position.z + (sceneObjects[i].bounds.size.z / 2F);
            if (current > max_z_value)
                max_z_value = current;
        }
    }

    [ContextMenu ("CreateNodes")]
    private void CreateNodes ()
    {
        DefineMinMax ();

        if (nodes == null)
            nodes = new List<GameObject>();
        else
            ClearNodes ();

        // Garante que o espaçamento seja um valor válido positivo
        spacing = Mathf.Max (spacing, 0.1F);

        GameObject node;
        Vector3 position;

        for (float x = min_x_value; x <= max_x_value; x += spacing)
        {
            for (float z = min_z_value; z <= max_z_value; z += spacing)
            {
                Ray ray = new Ray (new Vector3 (x, 100F, z), Vector3.down);
                RaycastHit hitInfo;

                if (Physics.Raycast (ray, out hitInfo, float.MaxValue))
                {
                    if (IsTerrainLayer(hitInfo.collider.gameObject.layer))
                    {
                        position = new Vector3 (x, hitInfo.point.y + (prefabNode.transform.localScale.y / 2F), z);
                        node = Instantiate (prefabNode, transform);
                        node.transform.position = position;
                        nodes.Add (node);
                    }
                }
            }
        }
    }

    [ContextMenu ("ClearNodes")]
    private void ClearNodes ()
    {
        for (int i = 0; i < nodes.Count; i++)
            DestroyImmediate (nodes[i].gameObject);
        nodes.Clear ();
    }

    private bool IsTerrainLayer (int layer)
    {
        return ((1 << layer) & terrainLayerMask) != 0;
    }

    #endregion
}
