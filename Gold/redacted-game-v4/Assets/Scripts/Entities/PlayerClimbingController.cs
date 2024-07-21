using UnityEngine;

public class PlayerClimbingController : MonoBehaviour
{
    [SerializeField] private ClimbingCourse currentCourse;
    [SerializeField] private float raycastLineLength;
    
    public void UpdateCurrentCourse(ClimbingCourse course) => currentCourse = course;
    public void UnsetCurrentCourse() => currentCourse = null;
    

    public (Vector3 point, bool isLastPoint) GetNextPoint(Vector3 playerPos, int layerMask, int isFacingRight)
    {
        if (currentCourse == null)
        {
            return (Vector3.zero, false);
        }
    
        Vector2 dir = new Vector2(((isFacingRight * -1) * 45), 45);
        RaycastHit2D hit = Physics2D.Raycast(playerPos, dir, raycastLineLength, layerMask);

        Vector2 hitPoint;
        if (hit.collider == null)
        {
            hitPoint = (Vector2) playerPos + dir.normalized * raycastLineLength;
        } else hitPoint = hit.point;
    
        Debug.DrawLine(playerPos, hitPoint, Color.magenta, 1f);
        Transform closestPoint = currentCourse.GetClosestPoint(hitPoint);
        bool isLastPoint = currentCourse.IsLastPoint(closestPoint);
        return (closestPoint.position, isLastPoint);
    }

}
