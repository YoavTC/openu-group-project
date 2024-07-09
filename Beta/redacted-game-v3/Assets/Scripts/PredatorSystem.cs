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
    private float lastSpeed;
    [SerializeField] private float droneReturnSpeed;
    [SerializeField] private float droneReturnDistance;
    [SerializeField] private float droneKillDistance;
    private Vector3 startingPos;

    private void Start()
    {
        if (playerTransform == null) playerTransform = FindObjectOfType<PlayerMovement>().transform;
        droneAI.maxSpeed = droneSpeed;
        startingPos = droneObject.position;
    }
    
    private void Update()
    {
        float dronePlayerDistance = DronePlayerDistance();

        //Move drone
        droneAI.maxSpeed = (dronePlayerDistance > droneReturnDistance) ? droneReturnSpeed : droneSpeed;
        
        //Checks
        if (dronePlayerDistance < droneKillDistance)
        {
            playerTransform.GetComponent<PlayerMovement>().Respawn();
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

    public void OnPlayerDied()
    {
        //Check with checkpoint system
        droneObject.position = startingPos;
    }
}
