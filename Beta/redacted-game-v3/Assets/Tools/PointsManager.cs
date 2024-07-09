using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class PointsManager : Singleton<PointsManager>
{
    #region Utility
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject _enemyPrefab;

    public GameObject EnemyPrefab
    {
        get { return _enemyPrefab; }
        private set { _enemyPrefab = value; }
    }
    
    [Button]
    public void CreateNewCheckpoint()
    {
        PointObject newPoint = Instantiate(pointPrefab, transform).GetComponent<PointObject>();
        newPoint.pointType = PointType.CHECKPOINT;
        newPoint.gameObject.name = "CHECKPOINT";
    }
    
    [Button]
    public void CreateNewGoalPoint()
    {
        PointObject newPoint = Instantiate(pointPrefab, transform).GetComponent<PointObject>();
        newPoint.pointType = PointType.GOAL_POINT;
        newPoint.gameObject.name = "GOAL POINT";
    }
    
    [Button]
    public void CreateNewEnemyPoint()
    {
        PointObject newPoint = Instantiate(pointPrefab, transform).GetComponent<PointObject>();
        newPoint.pointType = PointType.ENEMY_POINT;
        newPoint.gameObject.name = "ENEMY POINT";
    }
    #endregion

    public void PlayerActivatedPoint(PointObject point)
    {
        if (point.pointType == PointType.GOAL_POINT)
        {
            playerReachedGoalPoint?.Invoke();
        }
        
        if (point.pointType == PointType.ENEMY_POINT)
        {
            playerInteractedWithEnemy?.Invoke();
        }
    }
    
    //Events
    public UnityEvent playerReachedGoalPoint;
    public UnityEvent playerInteractedWithEnemy;
}

public enum PointType
{
    CHECKPOINT, GOAL_POINT, ENEMY_POINT
}
