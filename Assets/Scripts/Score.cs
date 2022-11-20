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
    }

    public void AddScore(int score = 1){
        int currentScore = int.Parse(textMeshPro.text);
        currentScore+=score;
        textMeshPro.text = currentScore.ToString();
    }
}
