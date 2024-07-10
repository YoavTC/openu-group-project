using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    void Start()
    {
        int layerMask = ~(1 << gameObject.layer);
        
        RaycastHit2D hitPoint = Physics2D.Raycast(transform.position, Vector2.down, 5f, layerMask);
        Vector2 groundedPoint = new Vector2(hitPoint.point.x, hitPoint.point.y + transform.localScale.y);
        transform.position = groundedPoint;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger) PointsManager.Instance.playerInteractedWithEnemy?.Invoke();
    }

    public void Die()
    {
        Destroy(GetComponent<BoxCollider2D>());
        GetComponent<Animator>().SetTrigger("Die");
    }
}
