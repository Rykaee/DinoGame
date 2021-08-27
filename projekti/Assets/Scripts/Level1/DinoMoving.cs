using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoMoving : MonoBehaviour
{
    public float speed;
    public float speedMultiplier = 0.7f;
    private float slowDownTimer = 0.0f;
    public float slowDownDuration;
    private bool isSlowed = false;
    private float originalSpeed;

    AudioManager audiomanager;
    // Start is called before the first frame update
    void Start()
    {
        audiomanager = FindObjectOfType<AudioManager>();
        originalSpeed = speed;
    }

    // Update is called once per frame
    //Set speed to dino
    void Update()
    {
        if (isSlowed)
        {
            slowDownTimer -= Time.deltaTime;

            if (slowDownTimer <= 0.0f)
            {
                isSlowed = false;
                speed = originalSpeed;
            }
        }

        // Do nothing if there is no chicken in lane
        bool isChickenFound = false;
        foreach (Transform child in this.transform.parent.transform)
        {
            if (child.gameObject.tag == "Kana")
            {
                isChickenFound = true;
            }
        }

        if(!isChickenFound)
        {
            speed = 0.0f;
        }

        this.transform.position += new Vector3(speed, 0f, 0f) * Time.deltaTime;
    }

    //Check if collision has happened
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Kana")
        {
            Debug.Log("Tormays tapahtunut!");
            Score.RemoveChickenInHome();
            if (Score.score >= 5)
            {
                Score.RemoveScore(5);
            }
            Destroy(collision.gameObject);
            speed = 0.0f;
            audiomanager.PlaySound("chickenGetsEaten");
        }
        else if (collision.gameObject.tag == "Block")
        {
            audiomanager.PlaySound("dino_osuu_palikkaan");

        }
    }

    public void DinoSlowed()
    {
        if(!isSlowed)
        {
            isSlowed = true;
            slowDownTimer = slowDownDuration;
            speed = originalSpeed * speedMultiplier;
        }
    }
}