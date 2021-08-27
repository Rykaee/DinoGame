using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDraggingLevel2 : MonoBehaviour
{
    public int blockType = 0;
    private bool isScaled = false;
    private bool isDragged = false;
    private bool startedDragging = false;
    private bool isOnCorrectLane = false;
    private Vector3 initialScale;
    private AudioManager audiomanager;
    private string[] laneTags = { "LaneBlue", "LaneRed", "LaneYellow", "LaneGreen" };

    public bool GetIsScaled()
    {
        return isScaled;
    }

    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
        audiomanager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isDragged)
            {
                // Continue to drag object
                this.GetComponent<Rigidbody>().useGravity = false;
                this.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragged)
            {
                // Ended dragging
                // Restore initial scale
                this.transform.localScale = initialScale;
                this.GetComponent<Rigidbody>().useGravity = true;
                this.GetComponent<Rigidbody>().isKinematic = false;
                isDragged = false;
            }
        }
    }

    void OnMouseDrag()
    {
        if (!startedDragging)
        {
            // Intersect only with ground
            int layerMask = 1 << 8;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check intersection with only layer 8 (Ground)
            if (Physics.Raycast(ray, out hit, 200.0f, layerMask))
            {
                Vector3 intersectionPoint = hit.point;
                Vector3 offsetFromGround = new Vector3(0.0f, 5.0f, 0.0f);
                Vector3 hoveredPosition = intersectionPoint + offsetFromGround;
                this.transform.position = hoveredPosition;
                this.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                this.transform.localScale = initialScale;

                // Reset velocities
                this.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
                this.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);

                isDragged = true;
                isScaled = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        // Did block hit any lanes?
        string colliderTag = collision.gameObject.tag;
        bool isOnLane = false;
        for(int i = 0; i < laneTags.Length && !isOnLane; i++)
        {
            if(colliderTag.CompareTo(laneTags[i]) == 0)
            {
                isOnLane = true;
            }
        }

        if(isOnLane)
        {
            // Are block and lane materials same?
            // If they are then block is on correct lane.
            isOnCorrectLane = false;
            Material blockMaterial = this.GetComponent<MeshRenderer>().material;
            Material laneMaterial = collision.gameObject.GetComponent<MeshRenderer>().material;
            if (blockMaterial.name.CompareTo(laneMaterial.name) == 0)
            {
                isOnCorrectLane = true;
            }

            if (isOnCorrectLane)
            {
                if (!isScaled)
                {
                    StartCoroutine(ScaleAnimation());
                    isScaled = true;
                    
                }
            }
            else
            {
                audiomanager.PlaySound("palikka1");
            }
        }
    }
    IEnumerator ScaleAnimation()
    {
        float f = 1.1f;
        audiomanager.PlaySound("palikka_kasvaa");
        while (f < 1.109f)
        {
            this.transform.localScale *= f;
            f += 0.001f;
            yield return new WaitForSeconds(0.02f);
        }
    }
}

