using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] private float transitionDuration;
    [SerializeField] private Gradient[] colorGradients;
    [SerializeField] private TrailRenderer trailRenderer;
    private Coroutine gradientTransitionCoroutine;

    public void OnPlayerSpeedChange(int level)
    {
        Debug.Log("New speed level: " + level);
        //trailRenderer.colorGradient = colorGradients[level];
        if (gradientTransitionCoroutine != null)
        {
            StopCoroutine(gradientTransitionCoroutine);
        }

        gradientTransitionCoroutine = StartCoroutine(TransitionGradient(colorGradients[level]));
    }
    
    private IEnumerator TransitionGradient(Gradient targetGradient)
    {
        Gradient startGradient = trailRenderer.colorGradient;
        float timeElapsed = 0.0f;

        while (timeElapsed < transitionDuration)
        {
            float t = timeElapsed / transitionDuration;
            trailRenderer.colorGradient = LerpGradient(startGradient, targetGradient, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        trailRenderer.colorGradient = targetGradient;
    }
    
    private Gradient LerpGradient(Gradient a, Gradient b, float t)
    {
        Gradient result = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[a.colorKeys.Length];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[a.alphaKeys.Length];

        for (int i = 0; i < colorKeys.Length; i++)
        {
            colorKeys[i].color = Color.Lerp(a.colorKeys[i].color, b.colorKeys[i].color, t);
            colorKeys[i].time = Mathf.Lerp(a.colorKeys[i].time, b.colorKeys[i].time, t);
        }

        for (int i = 0; i < alphaKeys.Length; i++)
        {
            alphaKeys[i].alpha = Mathf.Lerp(a.alphaKeys[i].alpha, b.alphaKeys[i].alpha, t);
            alphaKeys[i].time = Mathf.Lerp(a.alphaKeys[i].time, b.alphaKeys[i].time, t);
        }

        result.SetKeys(colorKeys, alphaKeys);
        return result;
    }
}
