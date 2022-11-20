using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    Rigidbody2D enemyRb;
    Score score;

    [SerializeField]
    float enemyVelocidade;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyVelocidade = transform.localScale.x;
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

    void OnTriggerExit2D(Collider2D other) {
        enemyVelocidade = -enemyVelocidade;
        FlipSprite();
    }

    
}
