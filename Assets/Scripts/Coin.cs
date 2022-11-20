using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private AudioSource audioSource;
    private Score score;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        score = FindObjectOfType<Score>();
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.CompareTag("Player")){
            score.AddScore();
            Destroy(this.gameObject);
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        }

    }
    
}
