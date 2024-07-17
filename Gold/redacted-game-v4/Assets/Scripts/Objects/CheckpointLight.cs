using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointLight : MonoBehaviour
{
    [SerializeField] private Sprite litSprite;
    private bool lit;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !lit)
        {
            GetComponent<SpriteRenderer>().sprite = litSprite;
            lit = true;
        }
    }
}
