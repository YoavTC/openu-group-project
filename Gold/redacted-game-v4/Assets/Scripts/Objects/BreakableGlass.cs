using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOnContact : MonoBehaviour
{
    private ParticleSystem breakingParticle;
    private SpriteRenderer spriteRenderer;
    private bool played;

    private void Start()
    {
        played = false;
        
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        breakingParticle = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!played) BreakObject();
    }

    private void BreakObject()
    {
        played = true;
        spriteRenderer.enabled = false;
        breakingParticle.Play();
    }

    private void OnParticleSystemStopped()
    {
        Destroy(transform.gameObject);
    }
}
