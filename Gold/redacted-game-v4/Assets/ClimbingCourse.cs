using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClimbingCourse : MonoBehaviour
{
    private List<Vector3> points = new List<Vector3>();
    [SerializeField] private float bufferDistance;

    private void Start()
    {
        points = HelperFunctions.GetChildren(transform).Select(child => child.position).ToList();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent.parent.GetComponent<PlayerMovement>().currentClimbingCourse = this;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent.parent.GetComponent<PlayerMovement>().currentClimbingCourse = null;
        }
    }

    public Vector3 GetClosestAbovePlayer(Vector3 playerPos)
    {
        Vector3 closestPoint = Vector3.zero;
        float closestDistance = float.MaxValue;

        foreach (var point in points)
        {
            // Check if the point is above the player's Y position
            if (point.y <= playerPos.y)
                continue;

            // Calculate distance to player position
            float distance = Vector3.Distance(point, playerPos);

            // Check if the distance is within the buffer zone
            if (distance < bufferDistance)
                continue;

            // Update closest point if this point is closer
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        // If no valid points above the player's Y position (excluding player's current position and buffer zone) are found, handle accordingly
        if (closestPoint == Vector3.zero)
        {
            return points[points.Count - 1];
        }

        return closestPoint;
    }
}
