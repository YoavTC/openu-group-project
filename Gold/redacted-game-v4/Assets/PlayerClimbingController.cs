using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbingController : MonoBehaviour
{
    [SerializeField] private ClimbingCourse currentCourse;
    
    public void UpdateCurrentCourse(ClimbingCourse course)
    {
        currentCourse = course;
    }

    public void UnsetCurrentCourse()
    {
        currentCourse = null;
    }

    public Vector3 GetNextPoint(Vector3 playerPos)
    {
        return currentCourse.GetClosestPointAbove(playerPos).position;
    }

    // public Vector3 GetNextPoint(Vector3 playerPos)
    // {
    //     int returnPointIndex = (currentPointIndex == -1) ? currentCourse.GetClosestPointIndex(playerPos) : currentPointIndex;
    //     currentPointIndex++;
    //     Vector3 returnPoint = currentCourse.GetNextPoint(returnPointIndex).position;
    //     
    //     if (currentCourse.pointsLength == currentPointIndex)
    //     {
    //         //Completed
    //         currentPointIndex = -1;
    //     }
    //     
    //     return returnPoint;
    // } 
}
