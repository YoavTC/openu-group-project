using System;
using UnityEngine;

public class PointObject : MonoBehaviour
{
    public PointType pointType;
    private GameObject enemyPrefab;

    private void Start()
    {
        if (pointType == PointType.ENEMY_POINT)
        {
            enemyPrefab = transform.parent.GetComponent<PointsManager>().EnemyPrefab;
            Destroy(GetComponent<Collider2D>());
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.GetComponent<PointsManager>().PlayerActivatedPoint(this);
        }
    }
}
