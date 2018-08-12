using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TowerSpot : MonoBehaviour
{
    public Tower prefabTower;
    public Tower inGameTower;
    
    private void OnMouseUpAsButton ()
    {
        if (inGameTower != null)
            return;

        if (GameManager.Instance.RemoveCurrency (prefabTower.price))
        {
            inGameTower = Instantiate (prefabTower, transform);
            inGameTower.transform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.Log ("[TowerSpot] (OnMouseUpAsButton) Você não tem dinheiro suficiente para construir essa torre!");
        }
    }
}
