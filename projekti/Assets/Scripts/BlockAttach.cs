using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAttach : MonoBehaviour
{

    public Vector3 pos;
    public GameObject otherObject;
    private bool collisionHappened = false;
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (collisionHappened)
        {
            rigidbody.isKinematic = true;
            transform.position = otherObject.transform.TransformPoint(pos);
            
        }
    }


    void OnCollisionEnter(Collision collision) 
    {
        // Dinosaur collision checking
        if (!collisionHappened)
        {
            if (this.CompareTag("Block"))
            {
                if (collision.other.gameObject.CompareTag("Dino"))
                {
                    collisionHappened = true;
                }
            }
        }
    }

    
}
