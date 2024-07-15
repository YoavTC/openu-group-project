using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClimbingPointsManager : Singleton<ClimbingPointsManager>
{
    private List<ClimbingCourse> courses = new List<ClimbingCourse>();

    private void Start()
    {
        courses = HelperFunctions.GetChildren(transform).Select(child => child.GetComponent<ClimbingCourse>()).ToList();
    }

    public Vector3 GetPoint(PlayerMovement player)
    {
        ClimbingCourse course = courses[courses.IndexOf(player.currentClimbingCourse)];
        return course.GetClosestAbovePlayer(player.transform.position);
    }
}
