using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMusicOnLoad : MonoBehaviour
{
    private AudioManager audiomanager;
    public string trackName;

    // Start is called before the first frame update
    void Start()
    {
        audiomanager = FindObjectOfType<AudioManager>();

        if (trackName != null)
        {
            audiomanager.ChangeBackgroundMusic(trackName);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
