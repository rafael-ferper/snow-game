using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1End : MonoBehaviour
{
    Boss1 boss1;

    void Awake(){
        boss1 = FindObjectOfType<Boss1> ();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.CompareTag("Player")){
        if(boss1.isDead){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
       }
    }
}
