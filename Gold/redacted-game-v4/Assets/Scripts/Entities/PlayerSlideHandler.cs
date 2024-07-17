using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Die();
        }
    }
}
