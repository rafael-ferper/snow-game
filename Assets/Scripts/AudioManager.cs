using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AudioManagerInstance {get; private set;}

    Slider audioSliderVolume;
    AudioSource audioSourceBG; 

    [SerializeField] Sprite[] btnMuteSprites;
    int buttonCurrentSpriteIndex;

    GameObject btnMute;

    void Awake()
    {
        if( AudioManagerInstance == null)
        {
            AudioManagerInstance = FindObjectOfType<AudioManager>();
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        audioSourceBG = GetComponent<AudioSource>();

        if(PlayerPrefs.HasKey("Volume"))
        {
            audioSourceBG.volume = PlayerPrefs.GetFloat("Volume");
        }else
        {
            audioSourceBG.volume = 0.5f;
            PlayerPrefs.SetFloat("Volume", audioSourceBG.volume);
            SaveAudio(audioSourceBG.volume);
        }
    }

    void OnSceneLoad(Scene scene, LoadSceneMode sceneMode)
    {
        if(scene.buildIndex == 1)
        {
            audioSliderVolume = FindObjectOfType<Slider>();
            audioSliderVolume.value = audioSourceBG.volume;
            audioSliderVolume.onValueChanged.AddListener(delegate { OnSliderValueChanged();});

            btnMute = GameObject.FindGameObjectWithTag("ButtonMute");
            buttonCurrentSpriteIndex = audioSourceBG.volume == 0 ? 0 : 1;
            btnMute.GetComponent<Image>().sprite = btnMuteSprites[buttonCurrentSpriteIndex];
            btnMute.GetComponent<Button>().onClick.AddListener(delegate {OnClickButtonMute();});
        }
    }

    void OnClickButtonMute()
    {
        buttonCurrentSpriteIndex = (buttonCurrentSpriteIndex+1) % 2;
        btnMute.GetComponent<Image>().sprite = btnMuteSprites[buttonCurrentSpriteIndex];
        SaveAudio(buttonCurrentSpriteIndex == 0 ? 0 : audioSliderVolume.value);
    }

    void OnSliderValueChanged()
    {
        float volume = audioSliderVolume.value;
        buttonCurrentSpriteIndex = volume == 0 ? 0 : 1;
        btnMute.GetComponent<Image>().sprite = btnMuteSprites[buttonCurrentSpriteIndex];
        SaveAudio(audioSliderVolume.value);
        
    }

    void SaveAudio(float volume)
    {
        audioSourceBG.volume  = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
