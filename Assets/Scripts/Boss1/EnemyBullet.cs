using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    HealthBar healthBar;
    public float dieTime, damage;

    void Awake(){
        healthBar = FindObjectOfType<HealthBar> ();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDownTimer());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            healthBar.Damage(damage);
            col.gameObject.GetComponent<Player>().InitDamageAnimation(false);
            Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CountDownTimer()
    {
        yield return new WaitForSeconds(dieTime);
        Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
