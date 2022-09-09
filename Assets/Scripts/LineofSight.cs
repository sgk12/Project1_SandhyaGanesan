using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineofSight : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag("Player")) {
            GetComponentInParent<Enemy>().player = col.transform;
            Debug.Log("player seen");
        }
    }
}
