using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
    #region gameobject variables
    [SerializeField]
    [Tooltip("Health Pack")]
    private GameObject healthPack;

    [SerializeField]
    [Tooltip("Speed Boost")]
    private GameObject speedBoost;
    
    #endregion

    #region chest funcs

    IEnumerator DestroyChest() {
        yield return new WaitForSeconds(.3f);
        
        GameObject[] buffs = new GameObject[] { healthPack, speedBoost };
        var random = new System.Random();
        GameObject buff = buffs[random.Next(0, buffs.Length)];
        Instantiate(buff, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    public void Interact() {
        StartCoroutine("DestroyChest");
    }
    #endregion
}
