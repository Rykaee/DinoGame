using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PathFollowerLevel3 : MonoBehaviour
{
    // Start position for object and parent of transforms
    // that create path.
    public Transform startPosition;
    public Transform parentOfWaypoints;
    private List<Transform> waypoints;

    // Total duration in seconds for traversing all waypoints
    public float totalPathDurationInSeconds;

    // Rotation speed when changing to new heading
    public float rotationSpeed = 0.05f;

    public float slowedDurationInSeconds = 1.0f;
    public float slowingCoefficient = 0.5f;

    private bool isSlowed = false;
    private float slowedAccumulator = 0.0f;

    // Total length of path
    private float totalPathLength;

    // Lengths and durations for each path segment
    private float[] segmentDurationInSeconds;
    private float[] segmentLength;
    private int currentWaypointIndex;
    private float accumulator;

    // Audiomanager for playing sound effects
    private AudioManager audiomanager;

    void Start()
    {
        accumulator = 0.0f;
        currentWaypointIndex = -1;
        audiomanager = FindObjectOfType<AudioManager>();

        // Add start position to waypoints and them automatically add waypoints
        // from parent of all rest of the path.
        waypoints = new List<Transform>();
        waypoints.Add(startPosition);

        int numWaypoints = parentOfWaypoints.childCount;
        for (int i = 0; i < parentOfWaypoints.childCount; i++)
        {
            waypoints.Add(parentOfWaypoints.GetChild(i).transform);
        }

        if (waypoints.Count > 0)
        {
            // Set initial position to first waypoint
            currentWaypointIndex = 0;
            this.transform.position = waypoints[0].position;

            // Calculate each segment length and total path length
            totalPathLength = 0.0f;
            segmentLength = new float[waypoints.Count - 1];
            for (int i = 0; i < segmentLength.Length; i++)
            {
                Vector3 segmentStart = waypoints[i].position;
                Vector3 segmentEnd = waypoints[i + 1].position;
                segmentLength[i] = Vector3.Distance(segmentEnd, segmentStart);
                totalPathLength += segmentLength[i];
            }

            // Calculate duration for each segment using total path duration
            segmentDurationInSeconds = new float[segmentLength.Length];
            for (int i = 0; i < segmentDurationInSeconds.Length; i++)
            {
                segmentDurationInSeconds[i] = (segmentLength[i] / totalPathLength) * totalPathDurationInSeconds;
            }

            // Orient GameObject toward segment end
            // Orient cube toward new waypoint
            Vector3 direction = waypoints[currentWaypointIndex + 1].position - this.transform.position;
            Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, direction, rotationSpeed, 0.0f);
            this.transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaypointIndex < (waypoints.Count - 1))
        {
            if (isSlowed)
            {
                // Slowed down moving
                slowedAccumulator += Time.deltaTime;
                if (slowedAccumulator > slowedDurationInSeconds)
                {
                    isSlowed = false;
                }

                accumulator += Time.deltaTime * slowingCoefficient;
            }
            else
            {
                // Normal moving
                accumulator += Time.deltaTime;
            }

            float durationForSegment = segmentDurationInSeconds[currentWaypointIndex];

            if (accumulator < durationForSegment)
            {
                // Orient GameObject if not at the end of path
                if (currentWaypointIndex < (waypoints.Count - 1))
                {
                    // Orient cube toward new waypoint
                    Vector3 direction = waypoints[currentWaypointIndex + 1].position - this.transform.position;
                    Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, direction, rotationSpeed, 0.0f);
                    this.transform.rotation = Quaternion.LookRotation(newDirection);
                }

                // Move toward next waypoint
                Vector3 a = waypoints[currentWaypointIndex].position;
                Vector3 b = waypoints[currentWaypointIndex + 1].position;
                float t = accumulator / durationForSegment;
                this.transform.position = Vector3.Lerp(a, b, t);
            }
            else
            {
                // Check for array boundaries
                if (currentWaypointIndex < waypoints.Count)
                {
                    // Switch to next segment
                    currentWaypointIndex++;
                    accumulator = 0.0f;
                    this.transform.position = waypoints[currentWaypointIndex].position;
                }
            }
        }
        else
        {
            // End of path
            //Debug.Log("End of path");
        }
    }

    public void SetSlowedState(bool state, float durationInSeconds)
    {
        isSlowed = state;
        slowedAccumulator = 0.0f;
        slowedDurationInSeconds = durationInSeconds;
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class PathFollowerLevel3 : MonoBehaviour
//{
//    // Dinosaur reference comes here
//    public GameObject dinosaur = null;
//    public GameObject chicken = null;

//    // Start position for object and parent of transforms
//    // that create path.
//    public Transform startPosition;
//    public Transform parentOfWaypoints;
//    private List<Transform> waypoints;

//    // Total duration in seconds for traversing all waypoints
//    public float totalPathDurationInSeconds;

//    // Rotation speed when changing to new heading
//    public float rotationSpeed = 0.05f;

//    public float slowedDurationInSeconds = 1.0f;
//    public float slowingCoefficient = 0.5f;

//    private bool isSlowed = false;
//    private float slowedAccumulator = 0.0f;

//    // Total length of path
//    private float totalPathLength;

//    // Lengths and durations for each path segment
//    private float[] segmentDurationInSeconds;
//    private float[] segmentLength;
//    private int currentWaypointIndex;
//    private float accumulator;

//    //Audiomanager for playing sound effects
//    AudioManager audiomanager;

//    void Start()
//    {
//        accumulator = 0.0f;
//        currentWaypointIndex = -1;
//        audiomanager = FindObjectOfType<AudioManager>();

//        // Add start position to waypoints and them automatically add waypoints
//        // from parent of all rest of the path.
//        waypoints = new List<Transform>();
//        waypoints.Add(startPosition);

//        int numWaypoints = parentOfWaypoints.childCount;
//        for (int i = 0; i < parentOfWaypoints.childCount; i++)
//        {
//            waypoints.Add(parentOfWaypoints.GetChild(i).transform);
//        }

//        if (waypoints.Count > 0)
//        {
//            // Set initial position to first waypoint
//            currentWaypointIndex = 0;
//            this.transform.position = waypoints[0].position;

//            // Calculate each segment length and total path length
//            totalPathLength = 0.0f;
//            segmentLength = new float[waypoints.Count - 1];
//            for (int i = 0; i < segmentLength.Length; i++)
//            {
//                Vector3 segmentStart = waypoints[i].position;
//                Vector3 segmentEnd = waypoints[i + 1].position;
//                segmentLength[i] = Vector3.Distance(segmentEnd, segmentStart);
//                totalPathLength += segmentLength[i];
//            }

//            // Calculate duration for each segment using total path duration
//            segmentDurationInSeconds = new float[segmentLength.Length];
//            for (int i = 0; i < segmentDurationInSeconds.Length; i++)
//            {
//                segmentDurationInSeconds[i] = (segmentLength[i] / totalPathLength) * totalPathDurationInSeconds;
//            }

//            // Orient GameObject toward segment end
//            // Orient cube toward new waypoint
//            Vector3 direction = waypoints[currentWaypointIndex + 1].position - this.transform.position;
//            Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, direction, rotationSpeed, 0.0f);
//            this.transform.rotation = Quaternion.LookRotation(newDirection);
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if(currentWaypointIndex < (waypoints.Count - 1))
//        {
//            if (isSlowed)
//            {
//                // Slowed down moving
//                slowedAccumulator += Time.deltaTime;
//                if (slowedAccumulator > slowedDurationInSeconds)
//                {
//                    isSlowed = false;
//                }

//                accumulator += Time.deltaTime * slowingCoefficient;
//            }
//            else
//            {
//                // Normal moving
//                accumulator += Time.deltaTime;
//            }

//            float durationForSegment = segmentDurationInSeconds[currentWaypointIndex];

//            if (accumulator < durationForSegment)
//            {
//                // Orient GameObject if not at the end of path
//                if (currentWaypointIndex < (waypoints.Count - 1))
//                {
//                    // Orient cube toward new waypoint
//                    Vector3 direction = waypoints[currentWaypointIndex + 1].position - this.transform.position;
//                    Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, direction, rotationSpeed, 0.0f);
//                    this.transform.rotation = Quaternion.LookRotation(newDirection);
//                }

//                // Move toward next waypoint
//                Vector3 a = waypoints[currentWaypointIndex].position;
//                Vector3 b = waypoints[currentWaypointIndex + 1].position;
//                float t = accumulator / durationForSegment;
//                this.transform.position = Vector3.Lerp(a, b, t);
//            }
//            else
//            {
//                // Check for array boundaries
//                if (currentWaypointIndex < waypoints.Count)
//                {
//                    // Switch to next segment
//                    currentWaypointIndex++;
//                    accumulator = 0.0f;
//                    this.transform.position = waypoints[currentWaypointIndex].position;
//                }
//            }
//        }
//        else
//        {
//            // End of path
//            //Debug.Log("End of path");
//        }

//        if (this.CompareTag("Kana"))
//        {
//            // Check if chicken is eaten (== no MeshRenderer active)
//            if (this.GetComponentInChildren<SkinnedMeshRenderer>().enabled == false)
//            {
//                if (this.GetComponentInChildren<ParticleSystem>().IsAlive() == false)
//                {
//                    // Feather animation just ended, move to next scene
//                    SceneManager.LoadScene("GameOver");
//                }
//            }
//        }
//    }

//    public void SetSlowedState(bool state, float durationInSeconds)
//    {
//        isSlowed = state;
//        slowedAccumulator = 0.0f;
//        slowedDurationInSeconds = durationInSeconds;
//    }

//    // Handle collisions
//    void OnCollisionEnter(Collision collision)
//    {
//        // Dinosaur collision handling
//        if (this.CompareTag("Dino"))
//        {
//            if (collision.collider.gameObject.CompareTag("Kana"))
//            {
//                // Dinosaur colliding with chicken
//                chicken.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
//                chicken.GetComponentInChildren<ParticleSystem>().Play();

//                // Reset velocity for RigidBody and stop animating dinosaur
//                this.enabled = false;
//                this.GetComponent<Rigidbody>().AddForce(Vector3.zero);
//                this.GetComponent<Rigidbody>().isKinematic = true;
//                this.GetComponentInChildren<Animator>().enabled = false;

//                // Sound effect here
//                audiomanager.PlaySound("chickenGetsEaten");
//            }
//            else if (collision.collider.gameObject.CompareTag("Block"))
//            {
//                // !!!!!!!!!!!! HUOMIO !!!!!!!!!!!!!!
//                // Korjaa tämä
//                // Dinosaur slowing
//                if (collision.collider.gameObject.GetComponent<BlockDraggingLevel2>().GetIsScaled())
//                {
//                    if (!isSlowed)
//                    {
//                        Score.AddScore(1);
//                        isSlowed = true;
//                        slowedAccumulator = 0.0f;
//                    }
//                }

//                // Sound effect here
//                audiomanager.PlaySound("dino_osuu_palikkaan");
//            }
//        }

//        // Chicken collision handling
//        if (this.CompareTag("Kana"))
//        {
//            if (collision.collider.gameObject.CompareTag("Koti"))
//            {
//                if (dinosaur != null)
//                {
//                    // Stop dinosaur
//                    dinosaur.GetComponent<PathFollowerLevel3>().enabled = false;
//                    dinosaur.GetComponentInChildren<Animator>().enabled = false;

//                    // Reset velocities and reset animation
//                    Rigidbody rb = dinosaur.GetComponent<Rigidbody>();
//                    rb.useGravity = false;
//                    rb.isKinematic = true;
//                    rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
//                    rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
//                }

//                // Hide chicken
//                this.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

//                // Sound effect here
//                audiomanager.PlaySound("chickenGetsHome");

//                // Feather animation just ended, move to next scene
//                SceneManager.LoadScene("GameOver");
//            }
//        }
//    }
//}

