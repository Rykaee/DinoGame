using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DinoStateCheckerLevel3 : MonoBehaviour
{
    // Audiomanager for playing sound effects
    private AudioManager audiomanager;
    private int numChickensInGame = 0;
    private int numChickensEaten = 0;
    private GameObject lastChicken = null;

    // Start is called before the first frame update
    void Start()
    {
        audiomanager = FindObjectOfType<AudioManager>();
        numChickensInGame = GameObject.FindGameObjectsWithTag("Kana").Length;
    }

    // Update is called once per frame
    void Update()
    {
        // Check every frame until chicken has done particle effects
        if(lastChicken != null)
        {
            if(lastChicken.GetComponentInChildren<ParticleSystem>().IsAlive() == false)
            {
                // Feather animation just ended, move to next scene
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    // Handle collisions
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Kana"))
        {
            numChickensEaten++;
            audiomanager.PlaySound("chickenGetsEaten");
            collision.collider.GetComponentInChildren<PathFollowerLevel3>().enabled = false;
            collision.collider.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            collision.collider.GetComponentInChildren<BoxCollider>().enabled = false;
            collision.collider.GetComponentInChildren<Rigidbody>().isKinematic = true;
            collision.collider.GetComponentInChildren<ParticleSystem>().Play();

            // Game is now over, store reference to last chicken
            if(numChickensEaten == numChickensInGame)
            {
                // Reset velocity for RigidBody and stop animating dinosaur
                this.GetComponent<PathFollowerLevel3>().enabled = false;
                this.GetComponent<Rigidbody>().AddForce(Vector3.zero);
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.GetComponentInChildren<Animator>().enabled = false;
                lastChicken = collision.gameObject;
            }
        }

        if (collision.collider.gameObject.CompareTag("Block"))
        {
            //// Dinosaur slowing
            //if (collision.collider.gameObject.GetComponent<BlockDraggingLevel3>().GetIsScaled())
            //{
            //    // Check if dino has pathfollowing component
            //    PathFollowerLevel2 script = this.GetComponent<PathFollowerLevel3>();
            //    if(script != null)
            //    {
            //        Score.AddScore(1);
            //        script.SetSlowedState(true, 2.0f);
            //    }
            //}
         
        }
    }
}
