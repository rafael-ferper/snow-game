using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadScene(){
        if(SceneManager.GetActiveScene().buildIndex == 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }else{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void LoadSpecificScene(string scene){
        SceneManager.LoadScene(scene);
    }
}