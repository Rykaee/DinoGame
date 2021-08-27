using System.Collections;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    AudioManager audiomanager;
    private void Start()
    {
        audiomanager = FindObjectOfType<AudioManager>();
    }
    public void ChangeScene(string sceneName)
    {
        Application.LoadLevel(sceneName);

        audiomanager.PlaySound("buttonSoundFX");

        Debug.Log("scene loaded");

        if (sceneName == "MainMenu" || sceneName == "ChangeLevel")
        {
            audiomanager.StopSound("chickens");
        } 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
