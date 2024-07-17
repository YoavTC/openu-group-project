using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private List<Transform> checkpoints;
    private Transform lastCheckpoint;
    
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<PointObject>().pointType == PointType.CHECKPOINT)
            {
                checkpoints.Add(transform.GetChild(i));
            }
        }

        lastCheckpoint = checkpoints[0];
    }

    public void ReachedNewCheckpoint(PointObject newCheckpoint)
    {
        lastCheckpoint = newCheckpoint.transform;
        InGameLogger.Log("Reached checkpoint number: " + (checkpoints.IndexOf(lastCheckpoint) + 1), Color.cyan);
    }

    public Vector2 GetLastCheckpoint(bool isPlayer)
    {
        if (isPlayer)
        {
            return lastCheckpoint.position;
        }

        return lastCheckpoint.GetChild(0).position;
    }
}
