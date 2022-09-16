using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorLineofSight : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag("Player")) {
            GetComponentInParent<ArmorEnemy>().player = col.transform;
            Debug.Log("player seen");
        }
    }
}
