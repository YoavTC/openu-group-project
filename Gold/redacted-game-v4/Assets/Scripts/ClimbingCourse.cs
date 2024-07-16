using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClimbingCourse : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float bufferDistance;
    public int pointsLength => points.Length;

    private void Start()
    {
        points = HelperFunctions.GetChildren(transform).ToArray();
    }

    public Transform GetClosestPointAbove(Vector3 playerPosition)
    {
        Transform closestPoint = null;
        float closestDistance = 1000f;

        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].position.y > playerPosition.y)
            {
                float distance = Vector3.Distance(playerPosition, points[i].position);
                Debug.Log("Distance: " + distance);
                if (distance < closestDistance && distance > bufferDistance)
                {
                    closestDistance = distance;
                    closestPoint = points[i];
                }
            }
        }
        if (closestPoint == null) return points[points.Length - 1];
        return closestPoint;
    }

    #region Detection Functions
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerClimbingController>().UpdateCurrentCourse(this);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetComponentInParent<PlayerClimbingController>())
        {
            other.GetComponentInParent<PlayerClimbingController>().UnsetCurrentCourse();
        }
    }
    #endregion
}
