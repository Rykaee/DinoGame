using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChickenStateCheckerLevel2 : MonoBehaviour
{
    private AudioManager audiomanager;
    private GameObject[] dinosaurs;

    // Start is called before the first frame update
    void Start()
    {
        audiomanager = FindObjectOfType<AudioManager>();
        dinosaurs = GameObject.FindGameObjectsWithTag("Dino");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Koti"))
        {
            for(int i = 0; i < dinosaurs.Length; i++)
            {
                // Stop dinosaur
                dinosaurs[i].GetComponent<PathFollowerLevel2>().enabled = false;
                dinosaurs[i].GetComponentInChildren<Animator>().enabled = false;

                // Reset velocities and reset animation
                Rigidbody rb = dinosaurs[i].GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;
                rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
            }

            // Hide chicken
            this.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            audiomanager.PlaySound("chickenGetsHome");
            SceneManager.LoadScene("GameOver");
        }
    }
}
