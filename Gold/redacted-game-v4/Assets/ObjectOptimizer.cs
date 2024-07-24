using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class ObjectOptimizer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float checkRate;
    [SerializeField] private Vector2 checkMinRadius, checkMaxRadius;

    [SerializeField] private Sprite defaultSprite;
    
    void Start()
    {
        InvokeRepeating(nameof(Check), 0f, checkRate);
    }

    [Button]
    private void Check()
    {
        Vector2 center = playerTransform.position;
        
        List<Collider2D> collidersInArea = Physics2D.OverlapBoxAll(center, checkMinRadius, 0f).ToList();
        
        DrawBounds(GetCorners(new Bounds(center, checkMinRadius)), 1f, Color.red);
        DrawBounds(GetCorners(new Bounds(center, checkMaxRadius)), 1f, Color.yellow);
        
        List<Transform> objectsInArea = GetObjectsInChecks(center);
        
        for (int i = 0; i < objectsInArea.Count; i++)
        {
            Debug.Log("Object " + i + " is " + objectsInArea[i].gameObject, objectsInArea[i].gameObject);
            if (objectsInArea[i].GetComponent<SpriteRenderer>() != null)
            {
                objectsInArea[i].GetComponent<SpriteRenderer>().enabled = true;
            }
            if (objectsInArea[i].GetComponent<Collider2D>() != null)
            {
                objectsInArea[i].GetComponent<Collider2D>().enabled = true;
            }
        }
    }

    private void DrawBounds(Vector3[] corners, float duration, Color color)
    {
        Debug.DrawLine(corners[0], corners[1], color, duration);
        Debug.DrawLine(corners[2], corners[3], color, duration);
        Debug.DrawLine(corners[0], corners[3], color, duration);
        Debug.DrawLine(corners[1], corners[2], color, duration);
    }

    private List<Transform> GetObjectsInChecks(Vector2 center)
    {
        List<Collider2D> collidersInAreaA = Physics2D.OverlapBoxAll(center, checkMinRadius, 0f).ToList();
        List<Collider2D> collidersInAreaB = Physics2D.OverlapBoxAll(center, checkMaxRadius, 0f).ToList();
        List<Transform> returnList = new List<Transform>();

        for (int i = 0; i < collidersInAreaB.Count; i++)
        {
            if (collidersInAreaB[i].transform.root.CompareTag("Player")) continue;
            Sprite sprite = collidersInAreaB[i].GetComponent<SpriteRenderer>().sprite;
            if (sprite == null || sprite == defaultSprite) continue;
            if (!collidersInAreaA.Contains(collidersInAreaB[i]))
            {
                returnList.Add(collidersInAreaB[i].transform);
            }
        }

        return returnList;
    }

    private Vector3[] GetCorners(Bounds bounds)
    {
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        Vector3 topRight = new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z);
        Vector3 topLeft = new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z);
        Vector3 bottomLeft = new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z);
        Vector3 bottomRight = new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z);

        return new Vector3[] { topRight, topLeft, bottomLeft, bottomRight };
    }
}
