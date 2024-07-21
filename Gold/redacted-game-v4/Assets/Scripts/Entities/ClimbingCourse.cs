using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClimbingCourse : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float xBufferDistance;

    private void Start()
    {
        points = HelperFunctions.GetChildren(transform).ToArray();
    }
    
    public Transform GetClosestPoint(Vector3 playerPosition)
    {
        Transform closestPoint = points[0];
        float closestDistance = Vector3.Distance(playerPosition, closestPoint.position);
        
        Debug.Log("Point 0 distance: " + Math.Abs(playerPosition.x - points[0].position.x), points[0].gameObject);

        for (int i = 1; i < points.Length; i++)
        {
            float distance = Vector3.Distance(playerPosition, points[i].position);
            float xDistance = playerPosition.x - points[i].position.x;
            Debug.Log("Point " + i + " distance: " + Math.Abs(xDistance), points[i].gameObject);
            if (distance < closestDistance && xBufferDistance > Math.Abs(xDistance))
            {
                closestDistance = distance;
                closestPoint = points[i];
            }
        }
        Debug.Log("ClimbingCourse return: " + closestPoint);
        return closestPoint;
    }
    
    public bool IsLastPoint(Transform point)
    {
        return point == points[points.Length - 1];
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
