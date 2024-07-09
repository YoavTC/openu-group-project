using Udar.SceneManager;
using Unity.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SceneField gameScene;
    [SerializeField] private PlayerMovement playerController;
    [SerializeField] private CheckpointManager checkpointManager;
    [SerializeField] private PredatorSystem predatorSystem;

    private void Start()
    {
        if (checkpointManager == null) checkpointManager = FindObjectOfType<CheckpointManager>();
        if (playerController == null) playerController = FindObjectOfType<PlayerMovement>();
        if (predatorSystem == null) predatorSystem = FindObjectOfType<PredatorSystem>();
    }
    
    public void OnPlayerRespawnEvent()
    {
        //Get checkpoint point
        Vector2 playerRespawnPoint = checkpointManager.GetLastCheckpoint(true);
        Vector2 droneRespawnPoint = checkpointManager.GetLastCheckpoint(false);
        
        //Make the player & drone move
        playerController.Respawn(playerRespawnPoint);
        predatorSystem.Respawn(droneRespawnPoint);
    }
}
