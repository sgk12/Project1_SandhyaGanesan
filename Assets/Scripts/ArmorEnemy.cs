using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorEnemy : MonoBehaviour {
    #region movement variables
    public float moveSpeed;
    #endregion

    #region physics components
    Rigidbody2D EnemyRB;
    #endregion

    #region targeting vars
    public Transform player;
    #endregion

    #region attack vars
    public float explosionDamage;
    public float explosionRadius;
    public GameObject explosionObj;

    public GameObject enemy;

    public float staggerTime = 1;
    #endregion

    #region health vars
    public float maxHealth;
    public float currHealth;
    #endregion

    #region unity funcs

    private void Awake() {
        EnemyRB = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;
    }

    private void Update() {
        if(player == null) {
            return;
        } 
        Move();
    }
    #endregion

    #region movement funcs

    private void Move() {
        Vector2 dir = player.position - transform.position;
        EnemyRB.velocity = dir.normalized * moveSpeed;

    }
    #endregion

    #region attack funcs

    private void Explode() {
        FindObjectOfType<AudioManager>().Play("Explosion");

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero);

        foreach (RaycastHit2D hit in hits) {
            if (hit.transform.CompareTag("Player")) {
                Debug.Log("kaboom");
                Instantiate(explosionObj, transform.position, transform.rotation);
                hit.transform.GetComponent<PlayerController>().TakeDamage(explosionDamage);
                Destroy(this.gameObject);
                
            }
        }
    } 

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player")) {
                Explode();
            }
        }

    #endregion

    #region health funcs

    public void TakeDamage(float val) {
        Debug.Log("armor break");
        Instantiate(enemy, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
    private void Die() {
        Destroy(this.gameObject);
    }

    #endregion
}
