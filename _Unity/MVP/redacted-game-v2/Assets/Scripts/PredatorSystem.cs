using Pathfinding;
using UnityEngine;

public class PredatorSystem : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform droneObject;
    [SerializeField] private Transform debugObject;
    [SerializeField] private AIPath droneAI;
    private Transform playerTransform;

    [Header("Drone Settings")] 
    [SerializeField] private float droneSpeed;
    [SerializeField] private float droneUpSpeed;
    private float lastSpeed;
    [SerializeField] private float droneReturnSpeed;
    [SerializeField] private float droneReturnDistance;
    [SerializeField] private float droneKillDistance;

    private void Start()
    {
        if (playerTransform == null) playerTransform = FindObjectOfType<PlayerMovement>().transform;
        droneAI.maxSpeed = droneSpeed;
    }
    
    private void Update()
    {
        float dronePlayerDistance = DronePlayerDistance();

        //Move drone
        droneAI.maxSpeed = (dronePlayerDistance > droneReturnDistance) ? droneReturnSpeed : droneSpeed;
        
        //Checks
        if (dronePlayerDistance < droneKillDistance)
        {
            Debug.Log("Player got caught!");
        }
    }

    private float DronePlayerDistance()
    {
        return Vector2.Distance(droneObject.position, playerTransform.position);
    }

    public void EnterSpeedZone(float newSpeed)
    {
        lastSpeed = droneSpeed;
        droneSpeed = (newSpeed / 100) * droneSpeed;
    }

    public void ExitSpeedZone()
    {
        droneSpeed = lastSpeed;
    }
}
