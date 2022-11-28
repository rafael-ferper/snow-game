using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    Rigidbody2D enemyRb;
    Score score;
    HealthBar healthBar;

    [SerializeField]
    float enemyVelocidade;

    [SerializeField]
    float enemyDamage;
    // Start is called before the first frame update

    void Awake(){
        healthBar = FindObjectOfType<HealthBar> ();
    }

    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        score = FindObjectOfType<Score>();
    }

    // Update is called once per frame
    void Update()
    {
        Correr();
    }

    void Correr(){
        enemyRb.velocity = new Vector2(enemyVelocidade,enemyRb.velocity.y);
    }

    void FlipSprite(){
        transform.localScale = new Vector2(-Mathf.Sign(enemyRb.velocity.x),1); 
    }

    void OnTriggerExit2D(Collider2D collision) {
       if(collision.gameObject.tag == ("Foreground")){
            enemyVelocidade = -enemyVelocidade;
            FlipSprite();
       }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            if(col.collider as BoxCollider2D)
            {
                Destroy(gameObject);
            }
            else
            {
                healthBar.Damage(enemyDamage);
                col.gameObject.GetComponent<Player>().InitDamageAnimation(true);
            }
        }
    }

    
}
