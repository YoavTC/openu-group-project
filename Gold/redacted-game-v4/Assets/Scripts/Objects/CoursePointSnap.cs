using UnityEngine;

public class CoursePointSnap : MonoBehaviour
{
    [SerializeField] private float xOffset;
    void Start()
    {
        LayerMask groundLayers = LayerMask.GetMask("NonJumpable", "WallGround");
        Vector2 centerPos = transform.position;
        RaycastHit2D rightPoint = Physics2D.Raycast(centerPos, Vector2.right, 100f, groundLayers);
        RaycastHit2D leftPoint = Physics2D.Raycast(centerPos, Vector2.left, 100f, groundLayers);

        int isRightCloser = Vector2.Distance(rightPoint.point, centerPos) < Vector2.Distance(leftPoint.point, centerPos) ? 1 : -1;
        Vector2 wallPoint = isRightCloser == 1 ? rightPoint.point : leftPoint.point;
        Vector2 offset = new Vector2(xOffset * (isRightCloser * -1), 0);
        transform.position = wallPoint + offset;
    }
}
