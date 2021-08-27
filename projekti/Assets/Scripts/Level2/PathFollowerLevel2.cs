using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathFollowerLevel2 : MonoBehaviour
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
        if(currentWaypointIndex < (waypoints.Count - 1))
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
