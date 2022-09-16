using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region movement variables
    public float moveSpeed;
    float currSpeed;
    float x_input;
    float y_input;
    #endregion

    #region attack variables
    public float Damage;
    public float attackSpeed = 1;
    float attackTimer;
    public float hitBoxTiming;
    public float endAnimationTiming;
    public float speedUpTiming;
    bool isAttacking;
    Vector2 currDirection;
    #endregion

    #region physics
    Rigidbody2D PlayerRB;
    #endregion

    #region animation
    Animator anim;
    #endregion

    #region health
    public float maxHealth;
    public float currHealth;
    public Slider HPSlider;
    #endregion

    #region unity funcs

    // called once when object created
    private void Awake() {
        PlayerRB = GetComponent<Rigidbody2D>();
        attackTimer = 0;
        anim = GetComponent<Animator>();
        currSpeed = moveSpeed;
        currHealth = maxHealth;
        HPSlider.value = currHealth / maxHealth;
    }

    // called once per frame
    private void Update() {
        if (isAttacking) {
            return;
        }
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        Move();

        if (Input.GetKeyDown(KeyCode.J) && attackTimer <= 0) {
            Attack();
        } else {
            attackTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            Interact();
        }
    }
    #endregion 

    #region movement funcs

    private void Move() {
        anim.SetBool("Moving", true);
        if (x_input > 0) {
            PlayerRB.velocity = Vector2.right * currSpeed;
            currDirection = Vector2.right;

        } else if (x_input < 0) {
            PlayerRB.velocity = Vector2.left * currSpeed;
            currDirection = Vector2.left;
            
        } else if (y_input > 0) {
            PlayerRB.velocity = Vector2.up * currSpeed;
            currDirection = Vector2.up;

        } else if (y_input < 0) {
            PlayerRB.velocity = Vector2.down * currSpeed;
            currDirection = Vector2.down;
        } else {
            PlayerRB.velocity = Vector2.zero;
            anim.SetBool("Moving", false);
        }
        anim.SetFloat("DirX", currDirection.x);
        anim.SetFloat("DirY", currDirection.y);
    }

    public void SpeedUp(float speed) {
        Debug.Log("speed up");
        StartCoroutine(SpeedRoutine(speed)); // buff speed for 3 seconds
    }

    IEnumerator SpeedRoutine(float speed) {
        currSpeed += speed;
        Debug.Log("speed is now" + currSpeed.ToString());
        yield return new WaitForSeconds(speedUpTiming);
        currSpeed = moveSpeed;
        Debug.Log("speed reset to" + moveSpeed.ToString());
        yield return null;
    }

    #endregion

    #region attack funcs

    private void Attack() {
        Debug.Log("attacking now");
        Debug.Log(currDirection);
        attackTimer = attackSpeed;
        StartCoroutine(AttackRoutine()); // handle animation + hitbox
    }

    IEnumerator AttackRoutine() {
        isAttacking = true;
        PlayerRB.velocity = Vector2.zero;

        anim.SetTrigger("Attacking");
        FindObjectOfType<AudioManager>().Play("PlayerAttack");

        yield return new WaitForSeconds(hitBoxTiming);
        Debug.Log("casting hitbox now");
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, Vector2.one, 0f, Vector2.zero);

        foreach (RaycastHit2D hit in hits) {
            Debug.Log(hit.transform.name);
            if(hit.transform.CompareTag("Enemy")) {
                if (hit.transform.GetComponent<Enemy>() != null) {
                    hit.transform.GetComponent<Enemy>().TakeDamage(Damage);
                    Debug.Log("damaged enemy");
                }
                else if (hit.transform.GetComponent<ArmorEnemy>() != null) {
                    hit.transform.GetComponent<ArmorEnemy>().TakeDamage(Damage);
                    Debug.Log("damaged armored enemy");
                }
            }     
        }
        yield return new WaitForSeconds(hitBoxTiming);
        isAttacking = false;
        yield return null;
    }
    #endregion

    #region health funcs

    public void TakeDamage(float val) {
        FindObjectOfType<AudioManager>().Play("PlayerHurt");

        currHealth -= val;
        Debug.Log("health is now " + currHealth.ToString());

        HPSlider.value = currHealth / maxHealth;

        if (currHealth <= 0) {
            Die();
        }
    }

    public void Heal(float val) {
        currHealth = Mathf.Min(currHealth + val, maxHealth);
        Debug.Log("health is now " + currHealth.ToString());

        HPSlider.value = currHealth / maxHealth;
    }

    private void Die() {
        FindObjectOfType<AudioManager>().Play("PlayerDeath");
        Destroy(this.gameObject);

        GameObject gm = GameObject.FindWithTag("GameController");
        gm.GetComponent<GameManager>().LoseGame();
    }

    #endregion

    #region interaction funcs

    private void Interact() {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, 
            new Vector2(0.5f, 0.5f), 0f, Vector2.zero, 0f);
        foreach (RaycastHit2D hit in hits) {
            if (hit.transform.CompareTag("Chest")) {
                hit.transform.GetComponent<Chest>().Interact();
            }
        }
        {
            
        }
    }

    #endregion

}
