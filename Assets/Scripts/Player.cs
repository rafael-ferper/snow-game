using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    
    Rigidbody2D playerRb;
    Animator playerAnimator;
    BoxCollider2D boxCollider;
    Score score;

    [SerializeField]
    float playerVeloH;
    [SerializeField]
    float playerPulo;
    [SerializeField]
    float playerClimb = 3;

    float playerDefaultGravity;
    Vector2 playerInput;
    bool canDoubleJump = false;
    bool isOnLadder = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        score = FindObjectOfType<Score>();
        playerDefaultGravity = playerRb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Correr();
        if(isOnLadder)
            Climb();
    }

    void Correr(){

        playerRb.velocity = new Vector2(playerInput.x*playerVeloH, playerRb.velocity.y);

        bool isRunning = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;

        playerAnimator.SetBool("isRunning",isRunning);

        if(isRunning)
            FlipSprite();
    }

    void FlipSprite(){
        transform.localScale = new Vector2(Mathf.Sign(playerRb.velocity.x),1); 
    }

    void Climb(){

        playerRb.velocity = new Vector2(playerRb.velocity.x,playerClimb*playerInput.y);

        bool isClimbing = Mathf.Abs(playerRb.velocity.y) > Mathf.Epsilon;
        playerAnimator.enabled = isClimbing;



    }

    void OnMove(InputValue inputValue){
        playerInput = inputValue.Get<Vector2>();
    }

    void OnJump(InputValue inputValue){
        
        if(inputValue.isPressed){
          if(boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){
            playerRb.velocity = new Vector2(0,playerPulo);
            canDoubleJump = true;
          }else if(canDoubleJump){
            playerRb.velocity = new Vector2(0,playerPulo);
            canDoubleJump = false;
          }
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        // if(other.gameObject.CompareTag("Enemy")){
        //     Destroy(other.gameObject);
        // }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       if(other.CompareTag("Enemy")){
            if(other as CapsuleCollider2D){
                score.AddScore(5);
                Destroy(other.gameObject);
            }
       }
       else if(other.CompareTag("Ladder")){
            isOnLadder = true;
            playerAnimator.SetBool("isClimbing",true);
            playerRb.gravityScale = 0;
       }
    }

    void OnTriggerExit2D(Collider2D other) {

        if(other.CompareTag("Ladder")){
            isOnLadder = false;
            playerAnimator.SetBool("isClimbing",isOnLadder);
            playerAnimator.enabled = true;
            playerRb.gravityScale = playerDefaultGravity;
        }

    }
}