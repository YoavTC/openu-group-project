using Udar.SceneManager;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SceneField gameScene;
    [SerializeField] private PlayerMovement playerController;
    [SerializeField] private CheckpointManager checkpointManager;
    [SerializeField] private PredatorSystem predatorSystem;
    [SerializeField] private PlayerSettingsScriptableObject playerSettings;
    [SerializeField] private SpeedrunTimer speedrunTimer;
    private Enemy[] enemies;

    private void Start()
    {
        if (checkpointManager == null) checkpointManager = FindObjectOfType<CheckpointManager>();
        if (playerController == null) playerController = FindObjectOfType<PlayerMovement>();
        if (predatorSystem == null) predatorSystem = FindObjectOfType<PredatorSystem>();

        enemies = FindObjectsOfType<Enemy>();

        AudioListener.volume = playerSettings.volume;
    }
    
    public void OnPlayerRespawnEvent()
    {
        //Respawn enemies
        foreach (Enemy enemy in enemies)
        {
            enemy.Resurrect();
        }
        
        //Get checkpoint point
        Vector2 playerRespawnPoint = checkpointManager.GetLastCheckpoint(true);
        Vector2 droneRespawnPoint = checkpointManager.GetLastCheckpoint(false);
        
        //Make the player & drone move
        playerController.Respawn(playerRespawnPoint);
        predatorSystem.Respawn(droneRespawnPoint);
        
        if (playerSettings.isSpeedrun) speedrunTimer.ResetTimer();
    }

    public void OnPlayerFinishEvent()
    {
        SceneManager.LoadScene(0);
    }
}
