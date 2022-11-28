using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    private AudioSource audioSource;
    private Score score;
    [SerializeField] bool collected;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        score = FindObjectOfType<Score>();
        collected = false;
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !collected){
            collected = true;
            Destroy(this.gameObject);
            score.AddScore();
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        }

    }
}
