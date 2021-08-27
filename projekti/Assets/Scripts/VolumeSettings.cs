using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{

    AudioManager music;
    AudioSource backgroundMusic;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider fxVolumeSlider;
   

    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string BackgroundPref = "BackgroundPref";
    private static readonly string FxVolumePref = "FxVolumePref";
    private int firstPlayValue;
    private float musicVolumeValue;
    private float fxVolumeValue;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        //Get playerprefs to see if players first play
        music = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();

        firstPlayValue = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayValue == 0)//set values
        {
            musicVolumeValue = 1.0f;
            fxVolumeValue = 1.0f;
            musicVolumeSlider.value = musicVolumeValue;
            fxVolumeSlider.value = fxVolumeValue;

            PlayerPrefs.SetFloat(BackgroundPref, musicVolumeValue);
            PlayerPrefs.SetFloat(FxVolumePref, fxVolumeValue);
            PlayerPrefs.SetInt(FirstPlay, -1);

            SaveSoundSettings();
        }
        else //get values from playerprefs and set slider
        {
            musicVolumeValue = PlayerPrefs.GetFloat(BackgroundPref);
            musicVolumeSlider.value = musicVolumeValue;

            fxVolumeValue = PlayerPrefs.GetFloat(FxVolumePref);
            fxVolumeSlider.value = fxVolumeValue; 
            
        }
    }

    //check when slider is released
    public void SliderIsReleased()
    {
        music.PlaySound("buttonSoundFX");
    }


    //Listen when player drops slider handle 
    public void DragHasEnded()
    {
        music.PlaySound("buttonSoundFX");
    }

    //save sound values
    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(BackgroundPref, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(FxVolumePref, fxVolumeSlider.value);
    }

    //if player loses focus in the game -> save settings
    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            SaveSoundSettings();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Update backgroundmusic volume
        foreach (Music m in music.musics)
        {
            m.source.volume = m.volume * musicVolumeSlider.value;
        }

        //Update volume of sound effects
        foreach (Sound s in music.sounds)
        {
            s.source.volume = s.volume * fxVolumeSlider.value;
        }

        SaveSoundSettings();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
