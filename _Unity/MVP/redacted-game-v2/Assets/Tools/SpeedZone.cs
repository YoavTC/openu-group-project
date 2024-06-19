using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SpeedZone : MonoBehaviour
{
    [MinValue(-100), MaxValue(100)]
    [SerializeField] private float speedRatePercentage;
    
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        //Add trigger box
        if (GetComponent<BoxCollider2D>() == null)
        {
            BoxCollider2D triggerBox = gameObject.AddComponent<BoxCollider2D>();
            triggerBox.isTrigger = true;
            triggerBox.size = GetComponent<SpriteRenderer>().size;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        transform.parent.GetComponent<PredatorSystem>().EnterSpeedZone(speedRatePercentage);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        transform.parent.GetComponent<PredatorSystem>().ExitSpeedZone();
    }
}
