using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    AudioManager audiomanager;
    private int hitNumber = 1;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.name == "Plane")
        {
            audiomanager.PlaySound("palikka" + hitNumber);
        }

        if(hitNumber == 3)
        {
            hitNumber = 1;
        } else
        {
            hitNumber++;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audiomanager = FindObjectOfType<AudioManager>();
        audiomanager.PlaySound("chickens");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
