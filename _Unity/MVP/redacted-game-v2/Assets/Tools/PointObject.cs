using System;
using UnityEngine;

public class PointObject : MonoBehaviour
{
    public PointType pointType;

    private void Start()
    {
        if (pointType == PointType.ENEMY_POINT)
        {
            Destroy(GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.GetComponent<PointsManager>().PlayerActivatedPoint(transform);
        }
    }
}
