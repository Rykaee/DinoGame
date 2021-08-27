using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel2 : MonoBehaviour
{
    public GameObject FingerIcon;
    public float fadeInTime = 1;

    // Transforms to act as start and end markers for the journey.
    public Transform startMarker;

    // List of all available end markers
    public Transform parentOfMarkers;

    // Reference to first slot in hotbar.
    public GameObject firstSlot;

    // End marker for journey.
    private Transform endMarker;

    // Movement speed in units per second.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    //private float fadeSpeed;

    bool doCheckBlock = true;

    void Start()
    {

    }

    // Move to the target end position.
    void Update()
    {
        if(doCheckBlock)
        {
            doCheckBlock = false;
            string[] materialNames = { "LaneRed", "LaneGreen", "LaneBlue", "LaneYellow" };
            string blockMaterialName = firstSlot.transform.GetChild(0).GetComponent<MeshRenderer>().material.name;
            bool found = false;
            for (int i = 0; i < materialNames.Length && !found; i++)
            {
                if (string.Compare(materialNames[i], 0, blockMaterialName, 0, materialNames[i].Length) == 0)
                {
                    endMarker = parentOfMarkers.GetChild(i);
                    found = true;
                }
            }

            // Convert marker to position in canvas in world space
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraPosition = Camera.main.transform.position;
            Plane p = new Plane(-cameraForward, cameraPosition);
            float distanceToPlane = p.GetDistanceToPoint(endMarker.position);
            endMarker.position = endMarker.position + cameraForward *
            (distanceToPlane * 0.950f);

            //Fades in and removes at end
            StartCoroutine(DoFadeIn(GetComponent<SpriteRenderer>()));
            Invoke("RemoveSprite", 3);

            // Keep a note of the time the movement started.
            // Starts after 1 second of wait 
            startTime = Time.time + 1;

            // Calculate the journey length.
            journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        }

        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
    }

    IEnumerator DoFadeIn(SpriteRenderer _sprite)
    {
        Color tmpColor = _sprite.color;

        while (tmpColor.a < 1f)
        {
            tmpColor.a += Time.deltaTime / fadeInTime;
            _sprite.color = tmpColor;

            if (tmpColor.a >= 1f)
                tmpColor.a = 1.0f;

            yield return null;
        }

        _sprite.color = tmpColor;

    }

    public void RemoveSprite()
    {
        FingerIcon.GetComponent<Renderer>().enabled = false;
    }
}
