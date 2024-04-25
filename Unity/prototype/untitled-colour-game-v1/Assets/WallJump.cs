using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    public bool rightWall => collidersInRight > 0;
    public bool leftWall => collidersInLeft > 0;
    
    [SerializeField] private BoxCollider2D rightCollider, leftCollider;
    private int collidersInRight, collidersInLeft;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (rightCollider.IsTouching(other)) collidersInRight++;
        if (leftCollider.IsTouching(other)) collidersInLeft++;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (rightCollider.IsTouching(other)) collidersInRight--;
        if (leftCollider.IsTouching(other)) collidersInLeft--;
    }
}
