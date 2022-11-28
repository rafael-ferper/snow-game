using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    public Animator playerAnimator;
    
    Rigidbody2D bossRb;
    Score score;
    HealthBar healthBar;
    Player player;

    [SerializeField]
    float bossVelocidade;

    [SerializeField]
    public float bossDamage, bossLife, bossTimeTkgDamage;
    public float bossLifeActual;
    bool mustRunning, canShoot, isTkgDamage;
    public bool isDead;

    public Transform shootPos;
    public float range, timeBtwShots, shootSpeed;
    public GameObject bullet;
    private float distToPlayer;
    // Start is called before the first frame update

    void Awake(){
        healthBar = FindObjectOfType<HealthBar> ();
        player = FindObjectOfType<Player> ();
    }

    void Start()
    {
        isDead = false;
        bossLifeActual = bossLife;
        bossRb = GetComponent<Rigidbody2D>();
        score = FindObjectOfType<Score>();
        canShoot = mustRunning = true;
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mustRunning){
            Correr();
        }

        distToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if(distToPlayer <= range)
        {
            if(player.transform.position.x > transform.position.x && transform.localScale.x < 0
                || player.transform.position.x < transform.position.x && transform.localScale.x > 0)
            {
                FlipSprite();
            }
            mustRunning = false;
            playerAnimator.SetBool("isShooting",!mustRunning);
            if(canShoot && !isTkgDamage)
                StartCoroutine(Shoot());
        } else{
            mustRunning = true;
            playerAnimator.SetBool("isShooting",!mustRunning);
        }
    }

    void Correr(){
        bossRb.velocity = new Vector2(bossVelocidade,bossRb.velocity.y);
    }

    void FlipSprite(){
        mustRunning = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y); 
        bossVelocidade *= -1;
        mustRunning = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            if(col.collider as BoxCollider2D)
            {
                Damage(0.02f);
            }
            else
            {
                healthBar.Damage(bossDamage);
                col.gameObject.GetComponent<Player>().InitDamageAnimation(true);
            }
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(timeBtwShots); 
        GameObject newBullet = Instantiate(bullet, shootPos.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootSpeed * (bossVelocidade*10) * Time.fixedDeltaTime, 0f);
        canShoot = true;
        
    }

    public void InitDamageAnimation()
    {
        StartCoroutine(TakingDamage());
    }

    IEnumerator TakingDamage()
    {
        isTkgDamage = true;
        playerAnimator.SetBool("isTkgDamage", true);
        yield return new WaitForSecondsRealtime(bossTimeTkgDamage);
        isTkgDamage = false;
        playerAnimator.SetBool("isTkgDamage", isTkgDamage);
    }

    void Damage(float damage){
        if((bossLifeActual -= damage) <= 0f ){
            InitDeathAnimation();
        }else{
            bossLifeActual -= damage;
            InitDamageAnimation();
        }
    }

    
    public void InitDeathAnimation()
    {
        StartCoroutine(BossDeath());
    }

    IEnumerator BossDeath()
    {
        isDead = true;
        playerAnimator.SetBool("isDead", isDead);
             
        yield return new WaitForSecondsRealtime(1.5f);
        Destroy(gameObject);
    }
}
