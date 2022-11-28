using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
    public Rigidbody2D playerRb;
    public Animator playerAnimator;
    BoxCollider2D boxCollider;
    GameSession gameSession;
    Boss1 boss1;
    HealthBar healthBar;

    [SerializeField] float playerVeloH;
    [SerializeField] float playerPulo;
    [SerializeField] float playerClimb = 3;
    [SerializeField] float playerTimeTkgDamage;
    [SerializeField] float playerMagVerDamage;
    [SerializeField] float playerMagHorDamage;

    float playerDefaultGravity;
    Vector2 playerInput;
    bool canDoubleJump = false;
    bool isOnLadder = false;
    bool isTkgDamage;
    bool isDead;
    AudioSource audioSourceDamage;
    public Vector3 respawnPoint;

    // Start is called before the first frame update

    void Awake(){
        healthBar = FindObjectOfType<HealthBar> ();
        boss1 = FindObjectOfType<Boss1> ();
    }

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerDefaultGravity = playerRb.gravityScale;
        respawnPoint = transform.position;
        audioSourceDamage = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(!isTkgDamage)
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
        
        if(inputValue.isPressed && !isDead){
          if(boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){
            playerRb.velocity = new Vector2(0,playerPulo);
            canDoubleJump = true;
          }else if(canDoubleJump){
            playerRb.velocity = new Vector2(0,playerPulo);
            canDoubleJump = false;
          }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.CompareTag("Enemy")){
        
            if(collision as CapsuleCollider2D){
                Destroy(collision.gameObject);
            }
       }
       else if(collision.CompareTag("Ladder")){
            isOnLadder = true;
            playerAnimator.SetBool("isClimbing",true);
            playerRb.gravityScale = 0;
       }
       else if(collision.CompareTag("FallDetector")){
            healthBar.Damage(0.1f);
            transform.position = respawnPoint;
       }
       else if(collision.CompareTag("Checkpoint")){
            respawnPoint = transform.position;
       }
    }
    void OnTriggerStay2D(Collider2D collision){
        if(collision.gameObject.tag == "Ice"){
            InitDamageAnimation(false);
            healthBar.Damage(0.002f);
        }
    }

    void OnTriggerExit2D(Collider2D collision) {

        if(collision.CompareTag("Ladder")){
            isOnLadder = false;
            playerAnimator.SetBool("isClimbing",isOnLadder);
            playerAnimator.enabled = true;
            playerRb.gravityScale = playerDefaultGravity;
        }
    }

    public void InitDamageAnimation(bool shove)
    {
        StartCoroutine(TakingDamage(shove));
    }

    IEnumerator TakingDamage(bool shove)
    {
        isTkgDamage = true;
        playerAnimator.SetBool("isTkgDamage", true);
        if(shove){
        AudioSource.PlayClipAtPoint(audioSourceDamage.clip, transform.position);
            playerRb.velocity = new Vector2(-playerMagHorDamage*transform.localScale.x, playerMagVerDamage);
        }
             
        yield return new WaitForSecondsRealtime(playerTimeTkgDamage);
        isTkgDamage = false;
        playerAnimator.SetBool("isTkgDamage", isTkgDamage);
    }
    

    public void InitDeathAnimation()
    {
        StartCoroutine(PlayerDeath());
    }

    IEnumerator PlayerDeath()
    {
        isDead = true;
        playerAnimator.SetBool("isDead", isDead);
             
        yield return new WaitForSecondsRealtime(1.5f);

        isDead = false;
        playerAnimator.SetBool("isDead", isDead);

        healthBar.SetHealth(1f);
        Respawn();
    }

    public void Respawn(){
        transform.position = respawnPoint;
        boss1.bossLifeActual = boss1.bossLife;
    }
}