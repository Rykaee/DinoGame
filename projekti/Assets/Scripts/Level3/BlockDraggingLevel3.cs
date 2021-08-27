using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDraggingLevel3 : MonoBehaviour
{
    public GameObject currentBlockSlot;
    public int blockType = 0;
    private bool isDragged = false;
    private bool isAttached = false;
    private Quaternion initialRotation;
    private Vector3 initialPosition;
    private AudioManager audiomanager;
    private GameObject targetDino;

    public int GetBlockType()
    {
        return blockType;
    }

    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.rotation;
        initialPosition = transform.position;
        audiomanager = FindObjectOfType<AudioManager>();
        targetDino = GameObject.Find("Dinosaur");
        if(targetDino)
        {
            Debug.Log("Dinosaur");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isDragged)
            {
                // Continue to drag object
                this.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
                this.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                this.GetComponent<Rigidbody>().useGravity = false;
                this.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragged)
            {
                if(!isAttached)
                {
                    // Ended dragging
                    // Return back to hotbar
                    this.transform.SetPositionAndRotation(initialPosition, initialRotation);
                    this.GetComponent<Rigidbody>().useGravity = false;
                    this.GetComponent<Rigidbody>().isKinematic = true;
                    this.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
                    this.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                    this.GetComponent<MeshCollider>().enabled = true;
                    isDragged = false;
                    isAttached = false;
                    audiomanager.PlaySound("lane missed");
                }
                else
                {
                    // Slow down dino
                    // Triggers every frame?
                    isAttached = false;
                    isDragged = false;
                    targetDino.GetComponent<PathFollowerLevel3>().SetSlowedState(true, 2.0f);
                    Score.AddScore(1);
                    StartCoroutine(Testi());
                    audiomanager.PlaySound("dino_osuu_palikkaan");
                    audiomanager.PlaySound("coin collected");
                }
            }
        }
    }

    void OnMouseDrag()
    {
        // Intersect only with ground
        int layerMask = 1 << 8;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check intersection with only layer 8 (Ground)
        if (Physics.Raycast(ray, out hit, 200.0f, layerMask))
        {
            Vector3 intersectionPoint = hit.point;
            Vector3 offsetFromGround = new Vector3(0.0f, 7.0f, 0.0f);
            Vector3 hoveredPosition = intersectionPoint + offsetFromGround;
            this.transform.position = hoveredPosition;
            this.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

            // Reset velocities
            this.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
            this.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            this.GetComponent<Rigidbody>().isKinematic = true;
            isDragged = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if block hits the dinosaur
        if(collision.collider.CompareTag("Dino"))
        {
            // Move dragged block to dinosaur block slot
            // Continue to drag object
            Transform blockSlotTransform = collision.collider.transform.Find("DinoBlockSlot");
            if(blockSlotTransform != null)
            {
                int allowedBlockType = currentBlockSlot.GetComponentInChildren<RandomizeBlocksLevel3>().GetBlockType();
                if (this.blockType == allowedBlockType)
                {
                    this.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
                    this.transform.SetParent(blockSlotTransform);
                    this.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                    this.transform.rotation = Quaternion.identity;
                    this.GetComponent<Rigidbody>().useGravity = false;
                    this.GetComponent<Rigidbody>().isKinematic = true;
                    this.GetComponent<MeshCollider>().enabled = false;
                    isAttached = true;
                }
                else
                {
                    this.transform.SetPositionAndRotation(initialPosition, initialRotation);
                    this.GetComponent<Rigidbody>().useGravity = false;
                    this.GetComponent<Rigidbody>().isKinematic = true;
                    this.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
                    this.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                    this.GetComponent<MeshCollider>().enabled = true;
                    isDragged = false;
                }
            }
        }
    }

    IEnumerator Testi()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(this.gameObject);
    }
}
