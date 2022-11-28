using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Score : MonoBehaviour
{
    TextMeshProUGUI textMeshPro;
    
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        textMeshPro.text = GameSession.totalScore.ToString();
    }

    public void AddScore(int score = 1){
        GameSession.totalScore+=score;
        textMeshPro.text = GameSession.totalScore.ToString();
    }
}
