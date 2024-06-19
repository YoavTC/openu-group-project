using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Spline = UnityEngine.Splines.Spline;

public class PredatorSystem : MonoBehaviour
{
    [Header("Components")]
    private Spline splinePath;
    [SerializeField] private Transform droneObject;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform debugObject;

    [Header("Drone Settings")] 
    [SerializeField] private float droneSpeed;
    [SerializeField] private float droneKillDistance;
    [SerializeField] private float droneSeeDistance;
    private BezierKnot closestKnot;

    #region old

    // void Start()
    // {
    //     splinePath = GetComponent<SplineContainer>().Spline;
    //     if (playerTransform == null) playerTransform = FindObjectOfType<PlayerMovement>().transform;
    //     
    //     //InvokeRepeating(nameof(UpdateClosestKnot), 0f, 0.5f);
    // }
    //
    // void Update()
    // {
    //     UpdateClosestKnot();
    //     debugObject.localPosition = closestKnot.Position;
    // }
    //
    // private void UpdateClosestKnot()
    // {
    //     BezierKnot[] knots = splinePath.ToArray();
    //     closestKnot = knots[0];
    //     float knotDistance = KnotDroneDistance(knots[0]);
    //     for (int i = 0; i < knots.Length; i++)
    //     {
    //         float tempDistance = KnotDroneDistance(knots[i]);
    //         if (tempDistance < knotDistance)
    //         {
    //             knotDistance = tempDistance;
    //             closestKnot = knots[i];
    //         }
    //     }
    // }
    //
    // private float KnotDroneDistance(BezierKnot knot)
    // {
    //     return Vector3.Distance(knot.Position, droneObject.position);
    // }

    #endregion

    private void Start()
    {
        splinePath = GetComponent<SplineContainer>().Spline;
        if (playerTransform == null) playerTransform = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        //Update debug object
        //debugObject.position = playerTransform.position;
        
        //Update drone
        float3 closestPointFloat3;
        SplineUtility.GetNearestPoint(splinePath, playerTransform.localPosition, out closestPointFloat3, out float t);
        Vector3 closestPoint = closestPointFloat3;
        debugObject.position = splinePath.EvaluatePosition(t);
        
        droneObject.DOMove(debugObject.position, droneSpeed).SetEase(Ease.InOutSine);
        //droneObject.position = Vector3.Lerp(droneObject.position, debugObject.position, droneSpeed);
        //droneObject.position = Vector3.MoveTowards(droneObject.position, debugObject.position, droneSpeed);

        float dronePlayerDistance = Vector3.Distance(droneObject.position, playerTransform.position);
        
        

        if (dronePlayerDistance < droneKillDistance)
        {
            Debug.Log("Player got caught!");
        }
    }
}
