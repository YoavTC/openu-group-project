using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BreakOnContact : MonoBehaviour
{
    private ParticleSystem breakingParticle;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private bool played;

    private void Start()
    {
        played = false;
        
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        breakingParticle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
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
        float randomPitch = Random.Range(1.1f, 1.5f);
        audioSource.pitch = randomPitch;
        audioSource.Play();
    }

    private void OnParticleSystemStopped()
    {
        Destroy(transform.gameObject);
    }
}
