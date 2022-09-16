using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    #region speed boost vars
    private float boostAmount;
    #endregion

    #region speed funcs

    private void OnTriggerEnter2D(Collider2D coll) {
        if(coll.transform.CompareTag("Player")) {
            boostAmount = 1.2f * (coll.transform.GetComponent<PlayerController>().maxHealth);
            coll.transform.GetComponent<PlayerController>().SpeedUp(boostAmount);
            Destroy(this.gameObject);
        }
    }
    
    #endregion
}