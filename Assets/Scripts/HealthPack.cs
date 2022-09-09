using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    #region health pack vars
    [SerializeField]
    [Tooltip("the amount the player heals")]
    private int healAmount;
    #endregion

    #region heal funcs
    private void OnTriggerEnter2D(Collider2D coll) {
        if(coll.transform.CompareTag("Player")) {
            coll.transform.GetComponent<PlayerController>().Heal(healAmount);
            Destroy(this.gameObject);
        }
    }
    
    #endregion
}
