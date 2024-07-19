using UnityEngine;

public class CoursePointSnap : MonoBehaviour
{
    void Start()
    {
        LayerMask groundLayers = LayerMask.GetMask("NonJumpable", "WallGround");
        Vector2 centerPos = transform.position;
        RaycastHit2D rightPoint = Physics2D.Raycast(centerPos, Vector2.right, 100f, groundLayers);
        RaycastHit2D leftPoint = Physics2D.Raycast(centerPos, Vector2.left, 100f, groundLayers);
        
        Vector2 wallPoint = Vector2.Distance(rightPoint.point, centerPos) < Vector2.Distance(leftPoint.point, centerPos) ? rightPoint.point : leftPoint.point;
        transform.position = wallPoint;
    }
}
