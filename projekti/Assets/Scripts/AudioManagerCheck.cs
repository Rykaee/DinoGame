using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerCheck : MonoBehaviour
{
    public AudioManager audiomanager; 
    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<AudioManager>())
        {
            return;
        } else
        {
            Instantiate(audiomanager, transform.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
