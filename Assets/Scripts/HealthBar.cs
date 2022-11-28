using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    RectTransform bar;
    public Image barImage;
    private Player player;

    void Awake(){
        player = FindObjectOfType<Player> ();
    }

    // Start is called before the first frame update
    void Start()
    {
        bar = GetComponent<RectTransform>();
        barImage = GetComponent<Image>();

        if(GameSession.totalHealth < 0.3f)
        {
            barImage.color = Color.red;
        }
        SetSize(GameSession.totalHealth);
    }
    
    public void Damage(float damage)
    {
        if((GameSession.totalHealth - damage) <= 0f ){
            SetSize(0f);
            player.InitDeathAnimation();
        }else{
            GameSession.totalHealth -= damage;
            
            SetSize(GameSession.totalHealth);
        }
    }

    public void SetSize(float size)
    {
        bar.localScale = new Vector3(size, 1f);
        if(size < 0.3f)
        {
            barImage.color = Color.red;
        }else{
            barImage.color = Color.green;
        }
    }

    public void SetHealth(float size)
    {
        GameSession.totalHealth = size;
        SetSize(GameSession.totalHealth);
    }
}
