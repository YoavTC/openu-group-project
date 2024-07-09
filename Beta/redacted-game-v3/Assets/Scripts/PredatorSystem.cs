using NaughtyAttributes;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

public class PredatorSystem : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform droneObject;
    [SerializeField] private AIPath droneAI;
    private Transform playerTransform;

    [Header("Drone Settings")] 
    [SerializeField] private float droneSpeed;
    private float lastSpeed;
    [SerializeField] private float droneReturnSpeed;
    [SerializeField] private float droneReturnDistance;
    [SerializeField] private float droneKillDistance;

    [SerializeField] [ReadOnly] private float xProgress;
    [SerializeField] [ReadOnly] private float currentXProgress;
    [SerializeField] private float xProgressBuffer;
    [SerializeField] [ReadOnly] private bool isGoingBack; 

    private void Start()
    {
        if (playerTransform == null) playerTransform = FindObjectOfType<PlayerMovement>().transform;
        droneAI.maxSpeed = droneSpeed;
        xProgress = droneObject.position.x;
    }
    
    private void Update()
    {
        float dronePlayerDistance = DronePlayerDistance();
        currentXProgress = droneObject.position.x - xProgressBuffer;

        //Move drone
        droneAI.maxSpeed = (dronePlayerDistance > droneReturnDistance) ? droneReturnSpeed : droneSpeed;

        if (currentXProgress < xProgress)
        {
            isGoingBack = true;
            droneAI.maxSpeed = droneReturnSpeed * 1.5f;
        }
        else
        {
            isGoingBack = false;
            droneAI.maxSpeed = droneSpeed;
        }
        
        //Checks
        if (dronePlayerDistance < droneKillDistance)
        {
            onDroneCatchPlayer?.Invoke();
            Debug.Log("Player got caught!");
        }

        if (currentXProgress > xProgress) xProgress = currentXProgress;
        Debug.DrawRay(new Vector2(xProgress, -10f), Vector3.up * 50, Color.green);
        Debug.DrawRay(new Vector2(currentXProgress, -10f), Vector3.up * 50, Color.magenta);
    }

    public void Respawn(Vector2 newSpawnPoint)
    {
        droneObject.position = newSpawnPoint;
        currentXProgress = newSpawnPoint.x;
        xProgress = newSpawnPoint.x;
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

    public UnityEvent onDroneCatchPlayer;
}
