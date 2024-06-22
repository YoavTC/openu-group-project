using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class PointsManager : MonoBehaviour
{
    #region Utility
    [SerializeField] private GameObject pointPrefab;
    
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

    public void PlayerActivatedPoint(Transform point)
    {
        if (point.GetComponent<PointObject>().pointType == PointType.GOAL_POINT)
        {
            playerReachedGoalPoint?.Invoke();
        }
    }
    
    //Events
    public UnityEvent playerReachedGoalPoint;
}

public enum PointType
{
    CHECKPOINT, GOAL_POINT, ENEMY_POINT
}
